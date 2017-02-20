using System.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using PluginApi.Plugins;
using RemoteFork.Plugins;

namespace RemoteFork.Plugins
{

    [PluginAttribute(Id = "acetorrentplay", Version = "0.1", Author = "ORAMAN", Name = "AceTorrentPlay", Description = "Воспроизведение файлов TORRENT через меда-сервер Ace Stream", ImageLink = "http://static.acestream.net/sites/acestream/img/ACE-logo.png")]
    public class AceTorrentPlay : IPlugin {
 

        private string IPAdress = "192.168.1.40";
        private string PortRemoteFork = "8027";
        private string PortAce = "6878";
        private bool AceProxEnabl;
        private string PLUGIN_PATH = "pluginPath";



        public Playlist GetList(IPluginContext context)
        {
                         var path = context.GetRequestParams().Get(PLUGIN_PATH);
                         path = path == null ? "plugin" : "plugin;" + path;
           
                      var playlist = new PluginApi.Plugins.Playlist();

            

            switch (path)
            {
                case "plugin":
                                    
                    return GetTopList(context);

                case "plugin;torrenttv":
                       return GetTorrentTV(context);
            }

            List<Item> items = new List<Item>();

           

            

            string[] PathSpliter = path.Split(';');




            switch (PathSpliter[PathSpliter.Length - 1])
            {
                case "ent":
                    return LastModifiedPlayList("ent",context);
                case "child":
                    return LastModifiedPlayList("child", context);
                case "common":
                    return LastModifiedPlayList("common", context);
                case "discover":
                    return LastModifiedPlayList("discover", context);
                case "HD":
                    return LastModifiedPlayList("HD", context);
                case "film":
                    return LastModifiedPlayList("film", context);
                case "man":
                    return LastModifiedPlayList("man", context);
                case "music":
                    return LastModifiedPlayList("music", context);
                case "news":
                    return LastModifiedPlayList("news", context);
                case "region":
                    return LastModifiedPlayList("region", context);
                case "relig":
                    return LastModifiedPlayList("relig", context);
                case "sport":
                    return LastModifiedPlayList("sport", context);
            }

            string PathFiles = ((string)(PathSpliter[PathSpliter.Length - 1])).Replace("|", "\\");

            switch (System.IO.Path.GetExtension(PathFiles))
            {

                case ".torrent":
                    PlayLister[] PlayListtoTorrent = GetFileListJSON(PathFiles, IPAdress);
                    string Description = SearchDescriptions(System.IO.Path.GetFileNameWithoutExtension(PathFiles.Split('(', '.', '[', '|')[0]));

                    foreach (PlayLister PlayListen in PlayListtoTorrent)
                    {
                        Item Item = new Item();
                        Item.Name = PlayListen.Name;
                        Item.ImageLink = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597291videofile.png";
                        Item.Link = PlayListen.Link;
                        Item.Type = ItemType.FILE;
                        Item.Description = Description;
                        items.Add(Item);
                    }

                    //Информация о запущенном файле 
                    //Dim WC As New System.Net.WebClient
                    //WC.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0")
                    //WC.Encoding = System.Text.Encoding.UTF8
                    //Dim AceMadiaInfo As String
                    //AceMadiaInfo = WC.DownloadString("http://127.0.0.1:6878/ace/manifest.m3u8?id=" & GetID(PathFiles, IPAdress) & "&format=json&use_api_events=1&use_stop_notifications=1")
                    //System.IO.File.WriteAllText("d:\My Desktop\инфо.txt", AceMadiaInfo)

                    playlist.Items = items.ToArray();

                    return playlist;

                case ".m3u":
                    return GetM3uList(PathFiles, context);
            }



            string[] ListFolders = System.IO.Directory.GetDirectories(PathFiles);
            foreach (string Fold in ListFolders)
            {
                Item Item = new Item();
                Item.Name = System.IO.Path.GetFileName(Fold);
                Item.Link = Fold.Replace("\\", "|");
                Item.ImageLink = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597246folder.png";
                Item.Type = ItemType.DIRECTORY;
                items.Add(Item);
            }

            if (AceProxEnabl == true)
            {
                foreach (string File in System.IO.Directory.EnumerateFiles(PathFiles, "*.*", System.IO.SearchOption.TopDirectoryOnly).Where((s) => s.EndsWith(".torrent")))
                {
                    Item Item = new Item();
                    Item.ImageLink = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597291utorrent2.png";
                    Item.Name = System.IO.Path.GetFileNameWithoutExtension(File);
                    Item.Link = File.Replace("\\", "|");
                    Item.Description = Item.Name;
                    Item.Type = ItemType.DIRECTORY;
                    items.Add(Item);
                }
            }

            foreach (string File in System.IO.Directory.EnumerateFiles(PathFiles, "*.*", System.IO.SearchOption.TopDirectoryOnly).Where((s) => s.EndsWith(".mkv") || s.EndsWith(".avi") || s.EndsWith(".mp4")))
            {
                Item Item = new Item();
                Item.ImageLink = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597291videofile.png";
                Item.Name = System.IO.Path.GetFileNameWithoutExtension(File);
                Item.Link = ((string)("http://" + IPAdress + ":" + PortRemoteFork + "/?file:///" + File)).Replace("\\", "/");
                Item.Description = Item.Link;
                Item.Type = ItemType.FILE;
                items.Add(Item);
            }

            foreach (string File in System.IO.Directory.EnumerateFiles(PathFiles, "*.*", System.IO.SearchOption.TopDirectoryOnly).Where((s) => s.EndsWith(".mp3")))
            {
                Item Item = new Item();
                Item.ImageLink = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597240aimp.png";
                Item.Name = System.IO.Path.GetFileNameWithoutExtension(File);
                Item.Link = ((string)("http://" + IPAdress + ":" + PortRemoteFork + "/?file:///" + File)).Replace("\\", "/");
                Item.Description = Item.Link;
                Item.Type = ItemType.FILE;
                items.Add(Item);
            }

            foreach (string File in System.IO.Directory.EnumerateFiles(PathFiles, "*.*", System.IO.SearchOption.TopDirectoryOnly).Where((s) => s.EndsWith(".jpg") || s.EndsWith(".png") || s.EndsWith(".gif") || s.EndsWith(".bmp")))
            {
                Item Item = new Item();
                Item.ImageLink = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597278imagefile.png";
                Item.Name = System.IO.Path.GetFileNameWithoutExtension(File);
                Item.Link = ((string)("http://" + IPAdress + ":" + PortRemoteFork + "/?file:///" + File)).Replace("\\", "/");
                Item.Description = Item.Link;
                Item.Type = ItemType.FILE;
                items.Add(Item);
            }

            foreach (string File in System.IO.Directory.EnumerateFiles(PathFiles, "*.*", System.IO.SearchOption.TopDirectoryOnly).Where((s) => s.EndsWith(".m3u")))
            {
                Item Item = new Item();
                Item.ImageLink = "http://s1.iconbird.com/ico/0912/VannillACreamIconSet/w128h1281348320736M3U.png";
                Item.Name = System.IO.Path.GetFileNameWithoutExtension(File);
                Item.Link = ((string)("http://" + IPAdress + ":" + PortRemoteFork + "/?file:///" + File)).Replace("\\", "/");
                Item.Description = Item.Link;
                Item.Type = ItemType.DIRECTORY;
                items.Add(Item);
            }




            playlist.Items = items.ToArray();

            foreach (var item in playlist.Items)
            {
                if (ItemType.DIRECTORY == item.Type)
                {
                    var pluginParams = new NameValueCollection();

                    pluginParams[PLUGIN_PATH] = item.Link;

                    item.Link = context.CreatePluginUrl(pluginParams);
                }
            }
            return playlist;





        }

         
         public   Playlist GetTorrentTV(IPluginContext context)
        {
            List<Item> items = new List<Item>();
            Item Item = new Item();

            Item.Type = ItemType.DIRECTORY;
            Item.Name = "РАЗВЛЕКАТЕЛЬНЫЕ";
            Item.Link = "ent";
            Item.ImageLink = "http://torrent-tv.ru/images/all_channels.png";
            items.Add(Item);

            Item = new Item();
            Item.Type = ItemType.DIRECTORY;
            Item.Name = "ДЕТСКИЕ";
            Item.Link = "child";
            Item.ImageLink = "http://torrent-tv.ru/images/all_channels.png";
            items.Add(Item);

            Item = new Item();
            Item.Type = ItemType.DIRECTORY;
            Item.Name = "ПОЗНАВАТЕЛЬНЫЕ";
            Item.Link = "discover";
            Item.ImageLink = "http://torrent-tv.ru/images/all_channels.png";
            items.Add(Item);

            Item = new Item();
            Item.Type = ItemType.DIRECTORY;
            Item.Name = "HD";
            Item.Link = "HD";
            Item.ImageLink = "http://torrent-tv.ru/images/all_channels.png";
            items.Add(Item);

            Item = new Item();
            Item.Type = ItemType.DIRECTORY;
            Item.Name = "ОБЩИЕ";
            Item.Link = "common";
            Item.ImageLink = "http://torrent-tv.ru/images/all_channels.png";
            items.Add(Item);

            Item = new Item();
            Item.Type = ItemType.DIRECTORY;
            Item.Name = "ФИЛЬМЫ";
            Item.Link = "film";
            Item.ImageLink = "http://torrent-tv.ru/images/all_channels.png";
            items.Add(Item);

            Item = new Item();
            Item.Type = ItemType.DIRECTORY;
            Item.Name = "МУЖСКИЕ";
            Item.Link = "man";
            Item.ImageLink = "http://torrent-tv.ru/images/all_channels.png";
            items.Add(Item);

            Item = new Item();
            Item.Type = ItemType.DIRECTORY;
            Item.Name = "МУЗЫКАЛЬНЫЕ";
            Item.Link = "music";
            Item.ImageLink = "http://torrent-tv.ru/images/all_channels.png";
            items.Add(Item);

            Item = new Item();
            Item.Type = ItemType.DIRECTORY;
            Item.Name = "НОВОСТИ";
            Item.Link = "news";
            Item.ImageLink = "http://torrent-tv.ru/images/all_channels.png";
            items.Add(Item);

            Item = new Item();
            Item.Type = ItemType.DIRECTORY;
            Item.Name = "РЕГИОНАЛЬНЫЕ";
            Item.Link = "region";
            Item.ImageLink = "http://torrent-tv.ru/images/all_channels.png";
            items.Add(Item);

            Item = new Item();
            Item.Type = ItemType.DIRECTORY;
            Item.Name = "РЕЛИГИОЗНЫЕ";
            Item.Link = "relig";
            Item.ImageLink = "http://torrent-tv.ru/images/all_channels.png";
            items.Add(Item);

            Item = new Item();
            Item.Type = ItemType.DIRECTORY;
            Item.Name = "СПОРТ";
            Item.Link = "sport";
            Item.ImageLink = "http://torrent-tv.ru/images/all_channels.png";
            items.Add(Item);

            PluginApi.Plugins.Playlist playlist = new PluginApi.Plugins.Playlist();
            playlist.Items = items.ToArray();

                        foreach (var item in playlist.Items)
            {
                if (ItemType.DIRECTORY == item.Type)
                {
                    var pluginParams = new NameValueCollection();

                    pluginParams[PLUGIN_PATH] = item.Link;

                    item.Link = context.CreatePluginUrl(pluginParams);
                }
            }
            return playlist;
        }


        public PluginApi.Plugins.Playlist LastModifiedPlayList(string NamePlayList , IPluginContext context)
        {

            string PathFileUpdateTime = System.IO.Path.GetTempPath() + NamePlayList + ".UpdateTime.tmp";
            string PathFilePlayList = System.IO.Path.GetTempPath() + NamePlayList + ".PlayList.tmp";

            System.Net.WebRequest request = System.Net.WebRequest.Create("http://super-pomoyka.us.to/trash/ttv-list/ttv." + NamePlayList + ".iproxy.m3u?ip=" + IPAdress + ":" + PortAce);
            request.Method = "HEAD";
            System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)(request.GetResponse());
            var responHeader = response.GetResponseHeader("Last-Modified");
            response.Close();

            System.Net.WebClient WC = new System.Net.WebClient();
            WC.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0");
            WC.Encoding = System.Text.Encoding.UTF8;

            List<Item> items = new List<Item>();
            Item Item = new Item();
            PluginApi.Plugins.Playlist playlist = new PluginApi.Plugins.Playlist();



            if ((System.IO.File.Exists(PathFileUpdateTime) && System.IO.File.Exists(PathFilePlayList)) == false)
            {
                UpdatePlayList(NamePlayList, PathFilePlayList, PathFileUpdateTime, responHeader);
                Item.Type = ItemType.DIRECTORY;
                Item.Description = WC.DownloadString(PathFilePlayList);
                items.Add(Item);
                
                playlist.Items = items.ToArray();

                foreach (var item in playlist.Items)
                {
                    if (ItemType.DIRECTORY == item.Type)
                    {
                        var pluginParams = new NameValueCollection();

                        pluginParams[PLUGIN_PATH] = item.Link;

                        item.Link = context.CreatePluginUrl(pluginParams);
                    }
                }
                return playlist;
            }

            if (responHeader != System.IO.File.ReadAllText(PathFileUpdateTime))
            {
                UpdatePlayList(NamePlayList, PathFilePlayList, PathFileUpdateTime, responHeader);
                Item.Type = ItemType.DIRECTORY;
                Item.Description = WC.DownloadString(PathFilePlayList);
                items.Add(Item);
                playlist.Items = items.ToArray();

                foreach (var item in playlist.Items)
                {
                    if (ItemType.DIRECTORY == item.Type)
                    {
                        var pluginParams = new NameValueCollection();

                        pluginParams[PLUGIN_PATH] = item.Link;

                        item.Link = context.CreatePluginUrl(pluginParams);
                    }
                }
                return playlist;
            }

            Item.Type = ItemType.DIRECTORY;
            Item.Description = WC.DownloadString(PathFilePlayList);
            items.Add(Item);

            playlist.Items = items.ToArray();

            foreach (var item in playlist.Items)
            {
                if (ItemType.DIRECTORY == item.Type)
                {
                    var pluginParams = new NameValueCollection();

                    pluginParams[PLUGIN_PATH] = item.Link;

                    item.Link = context.CreatePluginUrl(pluginParams);
                }
            }
            return playlist;

        }

        public void UpdatePlayList(string NamePlayList, string PathFilePlayList, string PathFileUpdateTime, string LastModified)
        {
            System.IO.File.WriteAllText(PathFileUpdateTime, LastModified);
            System.Net.WebClient WC = new System.Net.WebClient();
            WC.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0");
            WC.Encoding = System.Text.Encoding.UTF8;
            WC.Headers.Add("Accept-Encoding", "gzip, deflate");
            byte[] Dat = WC.DownloadData("http://super-pomoyka.us.to/trash/ttv-list/ttv." + NamePlayList + ".iproxy.m3u?ip=" + IPAdress + ":" + PortAce);


            System.IO.FileStream decompressedFileStream = System.IO.File.Create(PathFilePlayList);
            System.IO.Compression.GZipStream decompressionStream = new System.IO.Compression.GZipStream(new System.IO.MemoryStream(Dat), System.IO.Compression.CompressionMode.Decompress);
            decompressionStream.CopyTo(decompressedFileStream);
            decompressedFileStream.Close();
            decompressionStream.Close();
        }

        public PluginApi.Plugins.Playlist GetM3uList(string PATH, IPluginContext context)
        {
            System.Net.WebClient WC = new System.Net.WebClient();
            WC.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0");
            WC.Encoding = System.Text.Encoding.UTF8;

            List<Item> items = new List<Item>();
            Item Item = new Item();
            PluginApi.Plugins.Playlist playlist = new PluginApi.Plugins.Playlist();

            Item.Type = ItemType.DIRECTORY;
            Item.Description = WC.DownloadString(PATH);

            items.Add(Item);

            playlist.Items = items.ToArray();

            foreach (var item in playlist.Items)
            {
                if (ItemType.DIRECTORY == item.Type)
                {
                    var pluginParams = new NameValueCollection();

                    pluginParams[PLUGIN_PATH] = item.Link;

                    item.Link = context.CreatePluginUrl(pluginParams);
                }
            }
            return playlist;
        }

        public PluginApi.Plugins.Playlist GetTopList(IPluginContext context)
        {
            List<Item> items = new List<Item>();

            System.Net.WebClient WC = new System.Net.WebClient();
            WC.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0");
            WC.Encoding = System.Text.Encoding.UTF8;

            Item ItemTop = new Item();
            Item ItemTorrentTV = new Item();
            try
            {
                AceProxEnabl = true;
                string AceMadiaGet = null;
                AceMadiaGet = WC.DownloadString("http://" + IPAdress + ":" + PortAce + "/webui/api/service?method=get_version&format=jsonp&callback=mycallback");
                AceMadiaGet = "<html> Ответ от движка Ace Media получен: " + "<div>" + AceMadiaGet + "</div></html>";

                ItemTorrentTV.Name = "Torrent TV";
                ItemTorrentTV.Type = ItemType.DIRECTORY;
                ItemTorrentTV.Link = "torrenttv";
                ItemTorrentTV.ImageLink = "http://s1.iconbird.com/ico/1112/Television/w256h25613523820647.png";
                ItemTorrentTV.Description = "<html><img src=\"http://torrent-tv.ru/images/logo.png\"></html>" + WC.DownloadString("http://super-pomoyka.us.to/trash/ttv-list/MyTraf.php");

                ItemTop.ImageLink = "http://static.acestream.net/sites/acestream/img/ACE-logo.png";
                ItemTop.Name = "        - AceTorrentPlay -        ";
                ItemTop.Link = "";
                ItemTop.Type = ItemType.FILE;
                ItemTop.Description = AceMadiaGet + "<html><p><p><img src=\"http://static.acestream.net/sites/acestream/img/ACE-logo.png\"></html>";

                items.Add(ItemTop);
                items.Add(ItemTorrentTV);
            }
            catch
            {
                AceProxEnabl = false;
                ItemTop.ImageLink = "http://errorfix48.ru/uploads/posts/2014-09/1409846068_400px-warning_icon.png";
                ItemTop.Name = "        - AceTorrentPlay -        ";
                ItemTop.Link = "";
                ItemTop.Type = ItemType.FILE;
                ItemTop.Description = "Ответ от движка Ace Media не получен!";
                items.Add(ItemTop);
            }

            System.IO.DriveInfo[] ListDisk = System.IO.DriveInfo.GetDrives();
            foreach (System.IO.DriveInfo Disk in ListDisk)
            {
                if (Disk.DriveType == System.IO.DriveType.Fixed)
                {
                    Item Item = new Item();
                    Item.Name = Disk.Name + "  " + "(" + Math.Round(Disk.TotalFreeSpace / 1024 / 1024.0 / 1024, 2) + "ГБ свободно из " + Math.Round(Disk.TotalSize / 1024 / 1024.0 / 1024, 2) + "ГБ)";
                    Item.ImageLink = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597268hddwin.png";
                    Item.Link = Disk.Name.Replace("\\", "|");
                    Item.Type = ItemType.DIRECTORY;
                    Item.Description = Disk.Name + "\n" + "\r" + " <html> Метка диска: <div>" + Disk.VolumeLabel + "</div></html>";

                    items.Add(Item);
                }
            }

            var playlist = new PluginApi.Plugins.Playlist();
            playlist.Items = items.ToArray();
         
            foreach (var item in playlist.Items)
            {
                if (ItemType.DIRECTORY == item.Type)
                {
                    var pluginParams = new NameValueCollection();

                    pluginParams[PLUGIN_PATH] = item.Link;

                    item.Link = context.CreatePluginUrl(pluginParams);
                }
            }
            return playlist;
        }

        public struct PlayLister
        {
            public string IDX;
            public string Name;
            public string Link;
            public string Description;
        }


        public string GetID(string PathTorrent, string ServerAdress)
        {
            System.Net.WebClient WC = new System.Net.WebClient();
            WC.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0");
            WC.Encoding = System.Text.Encoding.UTF8;
            byte[] FileTorrent = WC.DownloadData(PathTorrent);

            string FileTorrentString = System.Convert.ToBase64String(FileTorrent);
            FileTorrent = System.Text.Encoding.Default.GetBytes(FileTorrentString);

            System.Net.WebRequest request = System.Net.WebRequest.Create("http://api.torrentstream.net/upload/raw");
            request.Method = "POST";
            request.ContentType = "application/octet-stream\\r\\n";
            request.ContentLength = FileTorrent.Length;
            System.IO.Stream dataStream = request.GetRequestStream();
            dataStream.Write(FileTorrent, 0, FileTorrent.Length);
            dataStream.Close();

            System.Net.WebResponse response = request.GetResponse();
            dataStream = response.GetResponseStream();
            System.IO.StreamReader reader = new System.IO.StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();

            string[] responseSplit = responseFromServer.Split('\"');
            return responseSplit[3];
        }

        public string GetM3UisTorrent(string PathTorrent, string ServerAdress)
        {
            //Возвращает неверный плейлист если в торренте только один видео файл
            System.Net.WebClient WC = new System.Net.WebClient();
            WC.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0");
            WC.Encoding = System.Text.Encoding.UTF8;

            return WC.DownloadString("http://" + ServerAdress + ":" + PortAce + "/ace/manifest.m3u8?id=" + GetID(PathTorrent, ServerAdress));
        }

        public PlayLister[] GetFileListJSON(string PathTorrent, string ServerAdress)
        {
            System.Net.WebClient WC = new System.Net.WebClient();
            WC.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0");
            WC.Encoding = System.Text.Encoding.UTF8;

            string[] CodeZnaki = { "\\U0430", "\\U0431", "\\U0432", "\\U0433", "\\U0434", "\\U0435", "\\U0451", "\\U0436", "\\U0437", "\\U0438", "\\U0439", "\\U043A", "\\U043B", "\\U043C", "\\U043D", "\\U043E", "\\U043F", "\\U0440", "\\U0441", "\\U0442", "\\U0443", "\\U0444", "\\U0445", "\\U0446", "\\U0447", "\\U0448", "\\U0449", "\\U044A", "\\U044B", "\\U044C", "\\U044D", "\\U044E", "\\U044F", "\\U0410", "\\U0411", "\\U0412", "\\U0413", "\\U0414", "\\U0415", "\\U0401", "\\U0416", "\\U0417", "\\U0418", "\\U0419", "\\U041A", "\\U041B", "\\U041C", "\\U041D", "\\U041E", "\\U041F", "\\U0420", "\\U0421", "\\U0422", "\\U0423", "\\U0424", "\\U0425", "\\U0426", "\\U0427", "\\U0428", "\\U0429", "\\U042A", "\\U042B", "\\U042C", "\\U042D", "\\U042E", "\\U042F" };
            string[] DecodeZnaki = { "а", "б", "в", "г", "д", "е", "ё", "ж", "з", "и", "й", "к", "л", "м", "н", "о", "п", "р", "с", "т", "у", "ф", "х", "ц", "ч", "ш", "щ", "ъ", "ы", "ь", "э", "ю", "я", "А", "Б", "В", "Г", "Д", "Е", "Ё", "Ж", "З", "И", "Й", "К", "Л", "М", "Н", "О", "П", "Р", "С", "Т", "У", "Ф", "Х", "Ц", "Ч", "Ш", "Щ", "Ъ", "Ы", "Ь", "Э", "Ю", "Я" };

            string ContentID = GetID(PathTorrent, ServerAdress);
            string ItogStr = WC.DownloadString("http://" + ServerAdress + ":" + PortAce + "/server/api?method=get_media_files&content_id=" + ContentID);
            for (int I = 0; I <= 65; I++)
            {
                ItogStr = ItogStr.Replace(CodeZnaki[I].ToLower(), DecodeZnaki[I]);
            }
            WC.Dispose();

            string PlayListJson = ItogStr;
            PlayListJson = PlayListJson.Replace(",", null);
            PlayListJson = PlayListJson.Replace(":", null);
            PlayListJson = PlayListJson.Replace("}", null);
            PlayListJson = PlayListJson.Replace("{", null);
            PlayListJson = PlayListJson.Replace("result", null);
            PlayListJson = PlayListJson.Replace("error", null);
            PlayListJson = PlayListJson.Replace("null", null);
            PlayListJson = PlayListJson.Replace("\"\"", "\"");
            PlayListJson = PlayListJson.Replace("\" \"", "\"");

            string[] ListSplit = PlayListJson.Split('\"');
            PlayLister[] PlayListTorrent = new PlayLister[(ListSplit.Length / 2) - 2 + 1];
            int N = 0;
            for (int I = 1; I <= ListSplit.Length - 2; I++)
            {
                PlayListTorrent[N].IDX = ListSplit[I];
                PlayListTorrent[N].Name = ListSplit[I + 1];
                PlayListTorrent[N].Link = "http://" + ServerAdress + ":" + PortAce + "/ace/getstream?id=" + ContentID + "&_idx=" + PlayListTorrent[N].IDX;

                I += 1;
                N += 1;
            }

            // Dim XDoc As New System.Xml.Linq.XDocument(JSON.Parse(jsonLiteral))
            //Dim N As Integer
            //For Each Xel As System.Xml.Linq.XElement In System.Xml.Linq.XDoc.Element("object").Element("object").Elements("string")
            //    PlayListTorrent(N).IDX = Xel.LastAttribute.Value.ToString
            //    PlayListTorrent(N).Name = Xel.Nodes(0).ToString
            //    PlayListTorrent(N).Link = "http://" & ServerAdress & ":6878/ace/getstream?id=" & ContentID & "&_idx=" & PlayListTorrent(N).IDX
            //    N += 1
            //Next

            return PlayListTorrent;
        }


        public string SearchDescriptions(string Name)
        {
            string HtmlFile = null;

            try
            {
                System.Net.WebClient WC = new System.Net.WebClient();
                WC.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0");
                WC.Encoding = System.Text.Encoding.UTF8;
                string Str = WC.DownloadString("http://www.kinomania.ru/search/?q=" + System.IO.Path.GetFileName(Name));

                System.Text.RegularExpressions.Regex Regul = new System.Text.RegularExpressions.Regex("<header><h3>По вашему запросу ничего не найдено</h3></header>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                bool Bool = Regul.IsMatch(Str);


                if (Bool == true)
                {
                    HtmlFile = "<html><div>Описание не найдено.</div><div>Попробуйте переименовать торрент файл</div></html>";
                }
                else
                {
                    Regul = new System.Text.RegularExpressions.Regex("(?<=fid=\").*(?=\">)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    System.Text.RegularExpressions.MatchCollection HTML = Regul.Matches(Str);
                    string FidStr = HTML[0].Value;



                    Str = WC.DownloadString("http://www.kinomania.ru/film/" + FidStr);

                    Regul = new System.Text.RegularExpressions.Regex("(?<=<title>).*(?=</title>)");
                    HTML = Regul.Matches(Str);
                    string TitleStr = "<div>" + HTML[0].Value.Replace("| KINOMANIA.RU", "") + "</div>";

                    Regul = new System.Text.RegularExpressions.Regex("(<img width=\"295\")(\\n|.)*?(/>)");
                    HTML = Regul.Matches(Str);
                    string OblojkaStr = HTML[0].Value.Replace("width=\"295\" height=\"434\"", "width=\"200\" height=\"320\"");

                    Regul = new System.Text.RegularExpressions.Regex("(<div class=\"l-col l-col-2\">)(\\n|.)*?(</div>)");
                    HTML = Regul.Matches(Str);
                    string ObzorStr = HTML[0].Value.Replace("<div class=\"l-col l-col-2\">", "<div class=\"l-col l-col-2\"><font size=\"1\">");

                    Regul = new System.Text.RegularExpressions.Regex("(<div class=\"l-col l-col-3\">)(\\n|.)*?(<section)");
                    HTML = Regul.Matches(Str);
                    string SozdateliStr = HTML[0].Value.Replace("<div class=\"l-col l-col-3\">", "<div class=\"l-col l-col-3\"><font size=\"1\">");

                    Regul = new System.Text.RegularExpressions.Regex("(<h2 class=\"b-switcher\">)(\\n|.)*?(</div>)");
                    HTML = Regul.Matches(Str);
                    string OFilmeStr = "<div " + System.Environment.NewLine + HTML[0].Value.Replace("<h2 class=\"b-switcher\">", "<h2 class=\"b-switcher\"><font size=\"2\">") + "</div></div></div>";



                    HtmlFile = "<html>" + TitleStr + "<table><tbody><tr><td valign=\"top\">" + OblojkaStr + "</td><td valign=\"top\">" + ObzorStr + " <div></td><td valign=\"top\">" + SozdateliStr + "</div>" + "</td></tr></tbody></table>" + OFilmeStr + "</html>";

                }

            }
            catch (Exception ex)
            {
                HtmlFile = ex.Message;
            }
            return HtmlFile;

        }
    }
}
