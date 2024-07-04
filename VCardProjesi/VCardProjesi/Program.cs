using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VCardProjesi.Models;

namespace VCardProjesi;

class Program
{
    static async Task Main(string[] args)
    {
        //kullanıcıdan kaç adet vcard oluşturmak istediğini alıyoruz
        Console.WriteLine("lutfen kac adet vcard olusturmak istediginizi giriniz: ");
        int count = Convert.ToInt32(Console.ReadLine());

        //data almak için kullanılan url tanımlıyoruz
        string url = "https://randomuser.me/api/?results=1";

        //request yaratmak için httpclient yaratıyoruz
        using HttpClient client = new HttpClient();

        //basePath değişeni vcf fayllarını oluşturduğumuz yeri gösteriyor
        string basePath = Directory.GetCurrentDirectory();
        basePath = Path.Combine(basePath, "..", "..", "..", "VCards");

        try
        {

            //döngü kullanarak istenilen sayda user bilgisi alıyoruz ve deserialize ediyoruz
            for (int i = 0; i < count; i++)
            {
                using HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                //var vCards = JsonConvert.DeserializeObject<VCard[]>(responseBody);  //bunu ben yazdım ama yanlış, nedenini bilmiyorum
                //aşağıdakı 3 satırı gpt yazdı, anladım mı ? hayır
                var jsonObject = JsonConvert.DeserializeObject<JObject>(responseBody);
                var results = jsonObject["results"].ToString();
                var vCards = JsonConvert.DeserializeObject<VCard[]>(results);

                //aldığımız bilgileri formatlayıb bir "ad_soyad" şeklinde vcf faylına kayd ediyoruz
                string filePath;
                foreach (var vcard in vCards)
                {
                    string vcardStr = vcard.VCardDonusturme();
                    Console.WriteLine(vcardStr);
                    filePath = Path.Combine(basePath, $"{vcard.Name.First}_{vcard.Name.Last}.vcf");

                    using FileStream fStream = File.Create(filePath);

                    using (StreamWriter writer = new StreamWriter(fStream))
                    {
                        writer.Write(vcardStr);
                    }
                }
            }
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine(e.Message);
        }
    }
}