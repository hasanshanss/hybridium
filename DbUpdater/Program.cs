using CsvHelper;
using DbUpdater.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static DbUpdater.Models.CsvMappers;

namespace DbUpdater
{
    class Program
    {
        private static string ipv4 = "GeoLite2-City-Blocks-IPv4",
                   locations = "GeoLite2-City-Locations-en";
        private static StreamReader streamReader;
        private static CsvReader csv;
        private static Func<string, int, string> headerFilter = (string header, int index) => header.ToLower().Replace("_", String.Empty);
        private static int elementAmount = 100 * 1000;

        static async System.Threading.Tasks.Task Main(string[] args)
        {
            DownloadUpdatePackage();
            await UpdateLocationsAsync();
            await UpdateBlocksAsync();
        }



        private static void DownloadUpdatePackage()
        {
            using (var client = new WebClient())
            {
                Console.WriteLine("Downloading updated zip...");
                //download updated package
                client.DownloadFile("https://download.maxmind.com/app/geoip_download?edition_id=GeoLite2-City-CSV&license_key=TjZ9zxfdFwEH2q5e&suffix=zip", @"1.zip");

                //create "Extract" folder if there is no one
                string extractPath = "extract";
                if (!Directory.Exists(extractPath))
                {
                    Directory.CreateDirectory(extractPath);
                }

                Console.WriteLine("Extract files...\n");
                //unzip necessary files
                using (ZipArchive zip = ZipFile.OpenRead("1.zip"))
                {
                    foreach (ZipArchiveEntry zipEntry in zip.Entries)
                    {
                        if (zipEntry.FullName.Contains("IPv4"))
                        {
                            zipEntry.ExtractToFile($"{extractPath}/{zipEntry.Name}", true);
                        }
                        else if (zipEntry.FullName.Contains(locations))
                        {
                            zipEntry.ExtractToFile($"{extractPath}/{zipEntry.Name}", true);
                        }
                    }
                }
            }
        }

        private async static Task UpdateBlocksAsync()
        {
            Console.WriteLine("UPDATING BLOCKS...");
            using (streamReader = new StreamReader(@$"extract\{ipv4}.csv"))
            using (csv = new CsvReader(streamReader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.PrepareHeaderForMatch = headerFilter;
                csv.Configuration.RegisterClassMap<GeoipBlocksMap>();

                using (hybridContext _dbContext = new hybridContext())
                {
                    try
                    {
                        int counter = 0;
                        Console.WriteLine($"Parsing CSV...\n");
                        List<List<GeoipBlocks>> geoipBlockChunkList = csv.GetRecords<GeoipBlocks>().ToList().ChunkBy(elementAmount);
                        Console.WriteLine($"Data divided into {geoipBlockChunkList.Count()} chunks!");

                        foreach (List<GeoipBlocks> geoipBlockChunk in geoipBlockChunkList)
                        {
                            Console.WriteLine($"GeoipBlock - Chunk no: {counter + 1}");
                            Console.WriteLine($"Deleting old information...");
                            _dbContext.GeoipBlocks.RemoveRange(_dbContext.GeoipBlocks
                                                                         .Skip(counter * elementAmount)
                                                                         .Take(elementAmount));
                            await _dbContext.SaveChangesAsync();

                            Console.WriteLine($"Inserting updated information...\n");
                            await _dbContext.GeoipBlocks.AddRangeAsync(geoipBlockChunk);
                            await _dbContext.SaveChangesAsync();

                            counter++;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        throw;

                    }
                }


            }
            Console.WriteLine("Updated successfully! :)\n");

        }

        private async static Task UpdateLocationsAsync()
        {
            Console.WriteLine("UPDATING LOCATIONS...");
            using (streamReader = new StreamReader(@$"extract\{locations}.csv"))
            using (csv = new CsvReader(streamReader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.PrepareHeaderForMatch = headerFilter;
                csv.Configuration.RegisterClassMap<GeoipLocationsMap>();

                using (hybridContext _dbContext = new hybridContext())
                {
                    try
                    {
                        int counter = 0;

                        Console.WriteLine($"Parsing CSV...\n");
                        List<List<GeoipLocations>> geoipLocationChunkList = csv.GetRecords<GeoipLocations>().ToList().ChunkBy(elementAmount);
                        Console.WriteLine($"Data divided into {geoipLocationChunkList.Count()} chunks!");

                        foreach (List<GeoipLocations> geoipLocationChunk in geoipLocationChunkList)
                        {
                            Console.WriteLine($"GeoipLocation - Chunk no: {counter + 1}");
                            Console.WriteLine($"Deleting old information...");
                            _dbContext.GeoipLocations.RemoveRange(_dbContext.GeoipLocations
                                                                         .Skip(counter * elementAmount)
                                                                         .Take(elementAmount));
                            await _dbContext.SaveChangesAsync();

                            Console.WriteLine($"Inserting updated information...\n");
                            await _dbContext.GeoipLocations.AddRangeAsync(geoipLocationChunk);
                            await _dbContext.SaveChangesAsync();

                            counter++;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        throw;
                    }

                    Console.WriteLine("Updated successfully! :)\n");
                }
            }
        }
    }
}
