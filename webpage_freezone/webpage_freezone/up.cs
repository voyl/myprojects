using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.GData.Client;
using Google.GData.Extensions.MediaRss;
using Google.GData.YouTube;
using Google.YouTube;




namespace uploader
{
    class up
    {
        string developerKey,hesapAdi,hesapSifresi,videoAdi,aciklama,etiketler,videoTur,videoYolu;
        public up(string d, string ha, string hs, string va, string ac, string et, string vt, string vy)
        {
            developerKey = d;
            hesapAdi = ha;
            hesapSifresi = hs;
            videoAdi = va;
            aciklama = ac;
            etiketler = et;
            videoTur = vt;
            videoYolu = vy;
        }
        public void upload()
        {
            Random a = new Random();
            string id = a.Next(100000, 999999).ToString();
            YouTubeRequestSettings settings = new YouTubeRequestSettings(id, developerKey, hesapAdi, hesapSifresi);
            YouTubeRequest request = new YouTubeRequest(settings);

            Video newVideo = new Video();
            ((GDataRequestFactory)request.Service.RequestFactory).Timeout = 9999999;
            newVideo.Title = videoAdi;
            newVideo.Tags.Add(new MediaCategory(videoTur, YouTubeNameTable.CategorySchema));
            newVideo.Keywords = etiketler;
            newVideo.Description = aciklama;
                newVideo.YouTubeEntry.Private = false;
            newVideo.YouTubeEntry.MediaSource = new MediaFileSource(videoYolu, "video/x-flv");

            request.Upload(newVideo);
        }
    }
}
