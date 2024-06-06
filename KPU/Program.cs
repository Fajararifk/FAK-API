using FAK.Domain.Entities;
using FAK.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Data.Entity;
using System.Linq;
using System.Text.Json;

string connectionstring = "Data Source=PF3KN7CQ-TPL14;Initial Catalog = FAKDb; Integrated Security = true;TrustServerCertificate=True";
var host = Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) =>
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionstring));
    })
    .Build();

using var scope = host.Services.CreateScope();
var serviceProvider = scope.ServiceProvider;
var dbContext = serviceProvider.GetRequiredService<AppDbContext>();

using (var client = new HttpClient())
{
    /*var wilayah = new Uri("https://sirekap-obj-data.kpu.go.id/wilayah/pemilu/ppwp/0.json");
    var hasilWilayah = client.GetAsync(wilayah).Result;
    var json = hasilWilayah.Content.ReadAsStringAsync().Result;
    List<Province> provinces = JsonSerializer.Deserialize<List<Province>>(json);
    var kode = provinces.Select(x => x.kode).ToList();
    if (dbContext.Provinces.Select(x => x.kode).Any())
    {
        var deleteDb = dbContext.Provinces.ExecuteDelete();
        dbContext.AddRange(provinces);
        await dbContext.SaveChangesAsync();
    }
    else
    {
        dbContext.AddRange(provinces);
        await dbContext.SaveChangesAsync();
    }*/
    /*  string input = Console.ReadLine();
      Console.WriteLine(input);
      var selectProvince = dbContext.Provinces.FirstOrDefault(x=>x.kode == input).kode;*/
    /*Console.WriteLine(selectProvince);*/
    /*foreach (var province in dbContext.Provinces.OrderBy(x=>x.kode).ToList())
    {
        var kabupaten = new Uri($"https://sirekap-obj-data.kpu.go.id/wilayah/pemilu/ppwp/{province.kode}.json");
        var hasilKabupaten = client.GetAsync(kabupaten).Result;
        var jsonKabupaten = hasilKabupaten.Content.ReadAsStringAsync().Result;
        List<Kabupaten> kabupatenList = JsonSerializer.Deserialize<List<Kabupaten>>(jsonKabupaten);
        var kodeKabupeten = kabupatenList.Select(x=>x.kode).ToArray();
        var kodeDb = dbContext.Kabupaten?.Where(x=>kodeKabupeten.Contains(x.kode)).Select(x => x.kode).ToList();
        if (kodeDb.Any())
        {
            dbContext.Kabupaten.ExecuteDelete();
            dbContext.AddRange(kabupatenList);
            dbContext.SaveChanges();
        }
        else
        {
            dbContext.AddRange(kabupatenList);
            dbContext.SaveChanges();
        }
    }*/
    /*foreach (var province in dbContext.Provinces.OrderBy(x => x.kode).ToList())
    {
        foreach (var kabupaten in dbContext.Kabupaten.OrderBy(x=>x.kode).ToList())
        {
            if (kabupaten.kode.StartsWith(province.kode))
            {
                var kecamatan = new Uri($"https://sirekap-obj-data.kpu.go.id/wilayah/pemilu/ppwp/{province.kode}/{kabupaten.kode}.json");
                var hasilKecamatan = client.GetAsync(kecamatan).Result;
                var jsonKecamatan = hasilKecamatan.Content.ReadAsStringAsync().Result;
                List<Kecamatan> kecamatanList = JsonSerializer.Deserialize<List<Kecamatan>>(jsonKecamatan);
                var kodeKecamatan = kecamatanList.Select(x => x.kode).ToArray();
                var kodeDb = dbContext.Kabupaten?.Where(x => kodeKecamatan.Contains(x.kode)).Select(x => x.kode).ToList();
                if (kodeDb.Any())
                {
                    dbContext.Kecamatan.ExecuteDelete();
                    dbContext.AddRange(kecamatanList);
                    dbContext.SaveChanges();
                }
                else
                {
                    dbContext.AddRange(kecamatanList);
                    dbContext.SaveChanges();
                }
            }
        }
        
    }*/
    /*Console.WriteLine($"INSERT DATABASE TO DESA : {DateTime.Now}");

    foreach (var province in dbContext.Provinces.OrderBy(x => x.kode).ToList())
    {
        foreach (var kabupaten in dbContext.Kabupaten.OrderBy(x => x.kode).ToList())
        {
            foreach (var kecamatan in dbContext.Kecamatan.OrderBy(x => x.kode).ToList())
            {
                if (kabupaten.kode.StartsWith(province.kode) && kecamatan.kode.StartsWith(kabupaten.kode))
                {
                    var desa = new Uri($"https://sirekap-obj-data.kpu.go.id/wilayah/pemilu/ppwp/{province.kode}/{kabupaten.kode}/{kecamatan.kode}.json");//https://sirekap-obj-data.kpu.go.id/wilayah/pemilu/ppwp/32/3276/327611/3276111005.json
                    var hasilDesa = client.GetAsync(desa).Result;
                    var jsonDesa = hasilDesa.Content.ReadAsStringAsync().Result;
                    List<Desa> desaList = JsonSerializer.Deserialize<List<Desa>>(jsonDesa);
                    var kodeDesa = desaList.Select(x => x.kode).ToArray();
                    var kodeDb = dbContext.Kecamatan?.Where(x => kodeDesa.Contains(x.kode)).Select(x => x.kode).ToList();
                    if (kodeDb.Any())
                    {
                        dbContext.Desa.ExecuteDelete();
                        dbContext.AddRange(desaList);
                        dbContext.SaveChanges();
                    }
                    else
                    {
                        dbContext.AddRange(desaList);
                        dbContext.SaveChanges();
                    }
                }
            }

            
        }

    }
    Console.WriteLine($"SUCCESSFULLY INSERT DATABASE TO DESA : {DateTime.Now}");
*/

    /*Console.WriteLine($"INSERT DATABASE TO TPS : {DateTime.Now}");
    foreach (var province in dbContext.Provinces.OrderBy(x => x.kode).ToList())
    {
        foreach (var kabupaten in dbContext.Kabupaten.Where(x=>x.kode.StartsWith(province.kode)).OrderBy(x => x.kode).ToList())
        {
            foreach (var kecamatan in dbContext.Kecamatan.Where(x=>x.kode.StartsWith(kabupaten.kode)).OrderBy(x => x.kode).ToList())
            {
                foreach (var desa in dbContext.Desa.Where(x=>x.kode.StartsWith(kecamatan.kode)).OrderBy(x => x.kode).ToList())
                {
                    if (kabupaten.kode.StartsWith(province.kode) && kecamatan.kode.StartsWith(kabupaten.kode) && desa.kode.StartsWith(kecamatan.kode))
                    {
                        var tps = new Uri($"https://sirekap-obj-data.kpu.go.id/wilayah/pemilu/ppwp/{province.kode}/{kabupaten.kode}/{kecamatan.kode}/{desa.kode}.json");//
                        var hasilTPS = client.GetAsync(tps).Result;
                        var jsonTPS = hasilTPS.Content.ReadAsStringAsync().Result;
                        List<TPS> tpsList = JsonSerializer.Deserialize<List<TPS>>(jsonTPS);
                        var kodeTPS = tpsList.Select(x => x.kode).ToArray();
                        var kodeDb = dbContext.Desa?.Where(x => kodeTPS.Contains(x.kode)).Select(x => x.kode).ToList();
                        if (kodeDb.Any())
                        {
                            dbContext.TPS.ExecuteDelete();
                            dbContext.AddRange(tpsList);
                            dbContext.SaveChanges();
                        }
                        else
                        {
                            dbContext.AddRange(tpsList);
                            dbContext.SaveChanges();
                        }
                        Console.WriteLine($"INSERT DATABASE TO TPS : {DateTime.Now}. {province.nama}|{kabupaten.nama}|{kecamatan.nama}|{desa.nama}");
                    }
                }

            }


        }

    }
    Console.WriteLine($"INSERT DATABASE TO TPS : {DateTime.Now}");*/
    var start = DateTime.Now;
    
    Console.WriteLine($"INSERT DATABASE TO TPS : {start}");
    var provinceDb = dbContext.Provinces.OrderBy(x => x.kode).ToList();
    var kabupatenDb = dbContext.Kabupaten.OrderBy(x => x.kode).ToList();
    var kecamatanDb = dbContext.Kecamatan.OrderBy(x => x.kode).ToList();
    var desaDb = dbContext.Desa.OrderBy(x => x.kode).ToList();
    var tpsDb = dbContext.TPS.OrderBy(x => x.kode).ToList();

    foreach (var province in provinceDb)
    {
        foreach (var kabupaten in kabupatenDb.Where(x => x.kode.StartsWith(province.kode)).OrderBy(x => x.kode).ToList())
        {
            foreach (var kecamatan in kecamatanDb.Where(x => x.kode.StartsWith(kabupaten.kode)).OrderBy(x => x.kode).ToList())
            {
                foreach (var desa in desaDb.Where(x => x.kode.StartsWith(kecamatan.kode)).OrderBy(x => x.kode).ToList())
                {
                    foreach (var tps in tpsDb.Where(x => x.kode.StartsWith(kecamatan.kode)).OrderBy(x => x.kode).ToList())

                    if (kabupaten.kode.StartsWith(province.kode) && kecamatan.kode.StartsWith(kabupaten.kode) && desa.kode.StartsWith(kecamatan.kode) && tps.kode.StartsWith(desa.kode))
                    {
                        var tpss = new Uri($"https://sirekap-obj-data.kpu.go.id/pemilu/hhcw/ppwp/{province.kode}/{kabupaten.kode}/{kecamatan.kode}/{desa.kode}/{tps.kode}.json");//
                        var hasilTPS = client.GetAsync(tpss).Result;
                        var jsonTPS = hasilTPS.Content.ReadAsStringAsync().Result;
                        List<TPS> tpsList = JsonSerializer.Deserialize<List<TPS>>(jsonTPS);
                        var kodeTPS = tpsList.Select(x => x.kode).ToArray();
                        var kodeDb = dbContext.Desa?.Where(x => kodeTPS.Contains(x.kode)).Select(x => x.kode).ToList();
                        if (kodeDb.Any())
                        {
                            dbContext.TPS.ExecuteDelete();
                            dbContext.AddRange(tpsList);
                            dbContext.SaveChanges();
                        }
                        else
                        {
                            dbContext.AddRange(tpsList);
                            dbContext.SaveChanges();
                        }
                        Console.WriteLine($"INSERT DATABASE TO TPS : {DateTime.Now}. {province.nama}|{kabupaten.nama}|{kecamatan.nama}|{desa.nama}");
                    }
                }

            }


        }

    }
    var finish = DateTime.Now;
    Console.WriteLine($"INSERT DATABASE TO TPS : start=> {start} - finish => {finish} = total {start-finish}");
}