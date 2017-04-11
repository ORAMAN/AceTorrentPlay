using System.Linq;
using PluginApi.Plugins;
using RemoteFork.Plugins;
using System.Collections.Generic;
using System.Collections.Specialized;
using System;

namespace RemoteFork.Plugins
{
    [PluginAttribute(Id = "acetorrentplaycs", Version = "0.6", Author = "ORAMAN", Name = "AceTorrentPlay CS", Description = "Воспроизведение файлов TORRENT через меда-сервер Ace Stream", ImageLink = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597291utorrent2.png")]
    public class acetorrentplaycs : IPlugin
    {
        private string IPAdress;
        private string PortRemoteFork = "8027";
        private string PLUGIN_PATH = "pluginPath";
        private PluginApi.Plugins.Playlist PlayList = new PluginApi.Plugins.Playlist();
        private string next_page_url;



        #region Настройки

        #region Иконки
        private string ICO_FolderVideo = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597246foldervideos.png";
        private string ICO_FolderVideo2 = "http://s1.iconbird.com/ico/1112/Concave/w256h2561352644144Videos.png";
        private string LOGO_RuTr = "https://rutrk.org/logo/logo.gif";
        private string ICO_Folder = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597246folder.png";

        private string ICO_Settings = "http://s1.iconbird.com/ico/2013/11/504/w128h1281385326483gear.png";
        private string ICO_SettingsFolder = "http://s1.iconbird.com/ico/2013/11/504/w128h1281385326516tools.png";
        private string ICO_SettingsParam = "http://s1.iconbird.com/ico/2013/11/504/w128h1281385326449check.png";
        private string ICO_SettingsReset = "http://s1.iconbird.com/ico/2013/11/504/w128h1281385326539check.png";

        private string ICO_VideoFile = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597291videofile.png";
        private string ICO_MusicFile = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597240aimp.png";
        private string ICO_TorrentFile = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597291utorrent2.png";
        private string ICO_TorrentFile2 = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597291utorrent.png";
        private string ICO_ImageFile = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597278imagefile.png";
        private string ICO_M3UFile = "http://s1.iconbird.com/ico/0912/VannillACreamIconSet/w128h1281348320736M3U.png";
        private string ICO_NNMClub = "http://s1.iconbird.com/ico/0912/MorphoButterfly/w128h1281348669898RhetenorMorpho.png";
        private string ICO_Search = "http://s1.iconbird.com/ico/0612/MustHave/w256h2561339195991Search256x256.png";
        private string ICO_Search2 = "http://s1.iconbird.com/ico/0912/MetroUIDock/w512h5121347464996Search.png";
        private string ICO_Error = "http://s1.iconbird.com/ico/0912/ToolbarIcons/w256h2561346685474SymbolError.png";
        private string ICO_Error2 = "http://errorfix48.ru/uploads/posts/2014-09/1409846068_400px-warning_icon.png";
        private string ICO_Save = "http://s1.iconbird.com/ico/2013/6/355/w128h1281372334742check.png";
        private string ICO_Delete = "http://s1.iconbird.com/ico/2013/6/355/w128h1281372334742delete.png";
        private string ICO_Pusto = "https://avatanplus.com/files/resources/mid/5788db3ecaa49155ee986d6e.png";
        private string ICO_Login = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597246portal2.png";
        private string ICO_Password = "http://s1.iconbird.com/ico/0612/GooglePlusInterfaceIcons/w128h1281338911371password.png";
        private string ICO_LoginKey = "http://s1.iconbird.com/ico/0912/ToolbarIcons/w256h2561346685464Login.png";

        private string LOGO_TrackerRutor = "/s/logo.jpg";
        #endregion

        #region Параметры
        private string ProxyServr = "proxy.antizapret.prostovpn.org";
        private int ProxyPort = 3128;

        private string FunctionsGetTorrentPlayList;
        private bool ProxyEnablerNNM;
        private string TrackerServerNNM;
        #endregion


        public void Load_Settings()
        {

            string TempStr = System.Convert.ToString(Microsoft.Win32.Registry.GetValue("HKEY_CURRENT_USER\\Software\\RemoteFork\\Plugins\\acetorrentplaycs\\", "FunctionsGetTorrentPlayList", ""));
            if (TempStr == "")
            {
                FunctionsGetTorrentPlayList = "GetFileListM3U";
                Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\\Software\\RemoteFork\\Plugins\\acetorrentplaycs\\", "FunctionsGetTorrentPlayList", FunctionsGetTorrentPlayList);
            }
            else
            {
                FunctionsGetTorrentPlayList = TempStr;
            }

            TempStr = System.Convert.ToString(Microsoft.Win32.Registry.GetValue("HKEY_CURRENT_USER\\Software\\RemoteFork\\Plugins\\acetorrentplaycs\\", "ProxyEnablerNNM", ""));
            if (TempStr == "")
            {
                ProxyEnablerNNM = true;
                Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\\Software\\RemoteFork\\Plugins\\acetorrentplaycs\\", "ProxyEnablerNNM", ProxyEnablerNNM);
            }
            else
            {
                ProxyEnablerNNM = System.Convert.ToBoolean(TempStr);
            }

            TempStr = System.Convert.ToString(Microsoft.Win32.Registry.GetValue("HKEY_CURRENT_USER\\Software\\RemoteFork\\Plugins\\acetorrentplaycs\\", "TrackerServerNNM", ""));
            if (TempStr == "")
            {
                TrackerServerNNM = "http://nnmclub.to";
                Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\\Software\\RemoteFork\\Plugins\\acetorrentplaycs\\", "TrackerServerNNM", TrackerServerNNM);
            }
            else
            {
                TrackerServerNNM = TempStr;
            }

        }
        public void Save_Settings()
        {
            Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\\Software\\RemoteFork\\Plugins\\acetorrentplaycs\\", "FunctionsGetTorrentPlayList", FunctionsGetTorrentPlayList);
            Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\\Software\\RemoteFork\\Plugins\\acetorrentplaycs\\", "ProxyEnablerNNM", ProxyEnablerNNM);
            Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\\Software\\RemoteFork\\Plugins\\acetorrentplaycs\\", "TrackerServerNNM", TrackerServerNNM);
        }


        public PluginApi.Plugins.Playlist GetListSettingsNNM(IPluginContext context)
        {
            return GetListSettingsNNM(context, "");
        }

        //INSTANT C# NOTE: Overloaded method(s) are created above to convert the following method having optional parameters:
        //ORIGINAL LINE: Function GetListSettingsNNM(context As IPluginContext, Optional ByVal ParametrSettings As String = "") As PluginApi.Plugins.Playlist
        public PluginApi.Plugins.Playlist GetListSettingsNNM(IPluginContext context, string ParametrSettings)
        {


            switch (ParametrSettings)
            {
                case "ProxyNNM":
                    ProxyEnablerNNM = !ProxyEnablerNNM;
                    break;
                case "TrackerServerNNM":
                    if (TrackerServerNNM == "http://nnmclub.to")
                    {
                        TrackerServerNNM = "http://nnm-club.me";
                    }
                    else
                    {
                        TrackerServerNNM = "http://nnmclub.to";
                    }
                    break;
            }
            Save_Settings();

            System.Collections.Generic.List<Item> Items = new System.Collections.Generic.List<Item>();
            Item Item_Top = new Item();
            Item Item_ProxEnabl = new Item();
            Item Item_TrackerServerNNM = new Item();

            Item_Top.Name = " - N N M - C l u b -";
            Item_Top.ImageLink = ICO_Pusto;
            Item_Top.Type = ItemType.FILE;
            Items.Add(Item_Top);

            Item_ProxEnabl.Name = "Вкл/Выкл proxy";
            Item_ProxEnabl.Link = "ProxyNNM;SETTINGS_NNM";
            Item_ProxEnabl.ImageLink = ICO_SettingsParam;
            if (ProxyEnablerNNM == true)
            {
                Item_ProxEnabl.Description = "Доступ к ресурсу " + TrackerServerNNM + " через прокси сервер включён.";
            }
            else
            {
                Item_ProxEnabl.Description = "Доступ к ресурсу " + TrackerServerNNM + " через прокси отключён.";
            }
            Items.Add(Item_ProxEnabl);

            Item_TrackerServerNNM.Name = "Адрес сервера NNM-Club";
            Item_TrackerServerNNM.Link = "TrackerServerNNM;SETTINGS_NNM";
            Item_TrackerServerNNM.ImageLink = ICO_SettingsParam;
            Item_TrackerServerNNM.Description = "Доступ к ресурсу осуществляется через " + TrackerServerNNM;
            Items.Add(Item_TrackerServerNNM);

            PlayList.IsIptv = "false";
            return PlayListPlugPar(Items, context);
        }


        public PluginApi.Plugins.Playlist GetListSettings(IPluginContext context)
        {
            return GetListSettings(context, "");
        }

        //INSTANT C# NOTE: Overloaded method(s) are created above to convert the following method having optional parameters:
        //ORIGINAL LINE: Function GetListSettings(context As IPluginContext, Optional ByVal ParametrSettings As String = "") As PluginApi.Plugins.Playlist
        public PluginApi.Plugins.Playlist GetListSettings(IPluginContext context, string ParametrSettings)
        {

            switch (ParametrSettings)
            {

                case "FunctionsGetTorrentPlayList":
                    switch (FunctionsGetTorrentPlayList)
                    {
                        case "GetFileListJSON":
                            FunctionsGetTorrentPlayList = "GetFileListM3U";
                            break;
                        case "GetFileListM3U":
                            FunctionsGetTorrentPlayList = "GetFileListJSON";
                            break;
                    }
                    ParametrSettings = "";
                    break;
                case "NNM-Club_Settings":
                    break;
                case "DeleteSettings":
                    Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree("Software\\RemoteFork\\Plugins\\acetorrentplaycs\\", false);
                    Load_Settings();
                    ParametrSettings = "";
                    break;
            }
            Save_Settings();

            System.Collections.Generic.List<Item> Items = new System.Collections.Generic.List<Item>();
            Item Item_Top = new Item();
            Item Item_FGPL = new Item();
            Item Item_NNM = new Item();
            Item Item_DelSettings = new Item();
            Item_Top.Name = "- Н А С Т Р О Й К И -";
            Item_Top.Link = "";
            Item_Top.ImageLink = ICO_Pusto;
            Item_Top.Type = ItemType.FILE;
            Items.Add(Item_Top);


            Item_NNM.Name = "Настройка доступа к NNM-Club";
            Item_NNM.Link = ";SETTINGS_NNM";
            Item_NNM.ImageLink = ICO_SettingsFolder;
            Items.Add(Item_NNM);

            Item_FGPL.Name = "Обработка содержимого torrent файла";
            Item_FGPL.Link = "FunctionsGetTorrentPlayList;SETTINGS";
            Item_FGPL.ImageLink = ICO_SettingsParam;
            Item_FGPL.Description = "<html>Выбор метода для запроса содержимого торрент файла <p> Текущий метод: <b>" + FunctionsGetTorrentPlayList + "</b></p><p>Измените параметр если при открытии торрентов содержащих более одного файла происходит ошибка.</html>";
            Items.Add(Item_FGPL);


            Item_DelSettings.Name = "Настройки по умолчанию";
            Item_DelSettings.Link = "DeleteSettings;SETTINGS";
            Item_DelSettings.ImageLink = ICO_SettingsReset;
            Items.Add(Item_DelSettings);


            PlayList.IsIptv = "false";
            return PlayListPlugPar(Items, context);
        }
        #endregion


        public PluginApi.Plugins.Playlist GetList(IPluginContext context)
        {

          
            IPAdress = context.GetRequestParams()["host"].Split(':')[0];

            var path = context.GetRequestParams().Get(PLUGIN_PATH);
            path = ((((path == null)) ? "plugin" : "plugin;" + path));

           
                if (context.GetRequestParams().Get("search") != null)
                {
                switch (path)
                {
                    case "plugin;Search_NNM":
                        return SearchListNNM(context, context.GetRequestParams()["search"]);
                    case "plugin;Search_RuTor":
                        return GetPAGERUTOR(context, TrackerServerRuTor + "/search/0/0/100/2/" + context.GetRequestParams()["search"]);
                    case "plugin;Search_rutracker":
                        return SearchListRuTr(context, context.GetRequestParams()["search"]);
                    case "plugin;RuTr_Login":
                        Login = context.GetRequestParams().Get("search");
                        return SetPassword(context);
                    case "plugin;RuTr_Password":
                        Password = context.GetRequestParams().Get("search");
                        return AuthorizationRuTr(context);
                    case "plugin;RuTr_Capcha_Key":
                        Capcha = context.GetRequestParams().Get("search");
                        return AuthorizationRuTr(context);
                }
            }


            switch (path)
            {
                case "plugin":
                    return GetTopList(context);
                case "plugin;torrenttv":
                    return GetTorrentTV(context);
                case "plugin;nnmclub":
                    return GetTopNNMClubList(context);
                case "plugin;rutor":
                    return GetTopListRuTor(context);
                case "plugin;rutr":
                    return GetTopListRuTr(context);
            }



            string[] PathSpliter = path.Split(';');

            switch (PathSpliter[PathSpliter.Length - 1])
            {
                //Трекер NNM
                case "PAGENNM":
                    return GetPAGENNM(context, PathSpliter[PathSpliter.Length - 2]);
                case "PAGEFILMNNM":
                    return GetTorrentPAGENNM(context, PathSpliter[PathSpliter.Length - 2]);
                //Трекер РуТор
                case "PAGERUTOR":
                    return GetPAGERUTOR(context, PathSpliter[PathSpliter.Length - 2]);
                case "PAGEFILMRUTOR":
                    return GetTorrentPageRuTor(context, PathSpliter[PathSpliter.Length - 2]);
                //Трекер RuTracker
                case "RuTrSubGroop":
                    return GetListRuTrCategory(context, PathSpliter[PathSpliter.Length - 3], PathSpliter[PathSpliter.Length - 2]);
                case "RuTrGroop":
                    return GetListRuTrCategory(context, PathSpliter[PathSpliter.Length - 2]);
                case "Search_Groop_RuTr":
                    return SearchListRuTr(context, context.GetRequestParams().Get("search"), PathSpliter[PathSpliter.Length - 2]);
                case "PAGERUTR":
                    return GetPageRuTr(context, PathSpliter[PathSpliter.Length - 2]);
                case "PAGEFILMRUTR":
                    return GetTorrentPageRuTr(context, PathSpliter[PathSpliter.Length - 2]);
                case "RuTrNonAuthorization":
                    Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree("Software\\RemoteFork\\Plugins\\RuTracker\\", false);
                    return GetTopListRuTr(context);

                //Торрент тв
                case "ent":
                    return LastModifiedPlayList("ent", context);
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
                //Взрослый контент
                case "porn":
                    return LastModifiedPlayList("porn", context);
                case "all":
                    return LastModifiedPlayList("all", context);

                //Настройки
                case "SETTINGS":
                    return GetListSettings(context, PathSpliter[PathSpliter.Length - 2]);

                case "SETTINGS_NNM":
                    return GetListSettingsNNM(context, PathSpliter[PathSpliter.Length - 2]);
            }

            string PathFiles = ((string)(PathSpliter[PathSpliter.Length - 1])).Replace("|", "\\");
            System.Collections.Generic.List<Item> items = new System.Collections.Generic.List<Item>();

            switch (System.IO.Path.GetExtension(PathFiles))
            {

                case ".torrent":
                    {
                        string Description = SearchDescriptions(System.IO.Path.GetFileNameWithoutExtension(PathFiles.Split('(', '.', '[', '|')[0]));

                        TorrentPlayList[] PlayListtoTorrent = GetFileList(PathFiles);

                        foreach (TorrentPlayList PlayListItem in PlayListtoTorrent)
                        {
                            Item Item = new Item();
                            Item.Name = PlayListItem.Name;
                            Item.ImageLink = PlayListItem.ImageLink;
                            Item.Link = PlayListItem.Link;
                            Item.Type = ItemType.FILE;
                            Item.Description = Description;
                            items.Add(Item);
                        }


                        return PlayListPlugPar(items, context);

                    }
                case ".m3u8":
                case ".m3u":
                    {
                        Item Item = new Item();
                        System.Net.WebClient WC = new System.Net.WebClient();
                        WC.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0");
                        WC.Encoding = System.Text.Encoding.UTF8;
                        Item.Type = ItemType.DIRECTORY;
                        Item.GetInfo = "http://" + IPAdress + ":" + PortRemoteFork + "/treeview?pluginacetorrentplaycs%5c.xml&host=" + IPAdress + "%3a8027&pluginPath=getinfo&ID=" + WC.DownloadString(PathFiles);
                        // .GetInfo = WC.DownloadString(PathFiles)
                        items.Add(Item);
                        PlayList.IsIptv = "true";
                        return PlayListPlugPar(items, context);
                    }
            }

            string[] ListFolders = System.IO.Directory.GetDirectories(PathFiles);
            foreach (string Fold in ListFolders)
            {
                Item Item = new Item();
                Item.Name = System.IO.Path.GetFileName(Fold);
                Item.Link = Fold.Replace("\\", "|");
                Item.ImageLink = ICO_Folder;
                Item.Type = ItemType.DIRECTORY;
                items.Add(Item);
            }

            if (AceProxEnabl == true)
            {
                foreach (string File in System.IO.Directory.EnumerateFiles(PathFiles, "*.*", System.IO.SearchOption.TopDirectoryOnly).Where((s) => s.EndsWith(".torrent")))
                {
                    Item Item = new Item();
                    Item.ImageLink = ICO_TorrentFile;
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
                Item.ImageLink = ICO_VideoFile;
                Item.Name = System.IO.Path.GetFileNameWithoutExtension(File);
                Item.Link = ((string)("http://" + IPAdress + ":" + PortRemoteFork + "/?file:/" + File)).Replace("\\", "/");
                Item.Description = Item.Link;
                Item.Type = ItemType.FILE;
                items.Add(Item);
            }

            foreach (string File in System.IO.Directory.EnumerateFiles(PathFiles, "*.*", System.IO.SearchOption.TopDirectoryOnly).Where((s) => s.EndsWith(".mp3")))
            {
                Item Item = new Item();
                Item.ImageLink = ICO_MusicFile;
                Item.Name = System.IO.Path.GetFileNameWithoutExtension(File);
                Item.Link = ((string)("http://" + IPAdress + ":" + PortRemoteFork + "/?file:/" + File)).Replace("\\", "/");
                Item.Description = Item.Link;
                Item.Type = ItemType.FILE;
                items.Add(Item);
            }

            foreach (string File in System.IO.Directory.EnumerateFiles(PathFiles, "*.*", System.IO.SearchOption.TopDirectoryOnly).Where((s) => s.EndsWith(".jpg") || s.EndsWith(".png") || s.EndsWith(".gif") || s.EndsWith(".bmp")))
            {
                Item Item = new Item();
                Item.ImageLink = ICO_ImageFile;
                Item.Name = System.IO.Path.GetFileNameWithoutExtension(File);
                Item.Link = ((string)("http://" + IPAdress + ":" + PortRemoteFork + "/?file:/" + File)).Replace("\\", "/");
                Item.Description = Item.Link;
                Item.Type = ItemType.FILE;
                items.Add(Item);
            }

            foreach (string File in System.IO.Directory.EnumerateFiles(PathFiles, "*.*", System.IO.SearchOption.TopDirectoryOnly).Where((s) => s.EndsWith(".m3u") || s.EndsWith(".m3u8")))
            {
                Item Item = new Item();
                Item.ImageLink = ICO_M3UFile;
                Item.Name = System.IO.Path.GetFileNameWithoutExtension(File);
                Item.Link = ((string)("http://" + IPAdress + ":" + PortRemoteFork + "/?file:/" + File)).Replace("\\", "/");
                Item.Description = Item.Link;
                Item.Type = ItemType.DIRECTORY;
                items.Add(Item);
            }

            PlayList.IsIptv = "false";
            return PlayListPlugPar(items, context);

        }

        public Playlist GetInfo(IPluginContext context)
        {
            var playlist = new PluginApi.Plugins.Playlist();
            List<Item> items = new List<Item>();
            Item Item = new Item();
            Item.Name = "information";
            Item.Link = "2";
            Item.Type = ItemType.FILE;
            Item.Description = "peers:2<br>";
            items.Add(Item);
            playlist.Items = items.ToArray();
            return playlist;
        }


        public PluginApi.Plugins.Playlist PlayListPlugPar(System.Collections.Generic.List<Item> items, IPluginContext context)
        {
            return PlayListPlugPar(items, context, "");
        }

        //INSTANT C# NOTE: Overloaded method(s) are created above to convert the following method having optional parameters:
        //ORIGINAL LINE: Function PlayListPlugPar(ByVal items As System.Collections.Generic.List(Of Item), ByVal context As IPluginContext, Optional ByVal next_page_url As String = "") As PluginApi.Plugins.Playlist
        public PluginApi.Plugins.Playlist PlayListPlugPar(System.Collections.Generic.List<Item> items, IPluginContext context, string next_page_url)
        {
            if (next_page_url != "")
            {
                var pluginParams = new NameValueCollection();
                pluginParams[PLUGIN_PATH] = next_page_url;
                PlayList.NextPageUrl = context.CreatePluginUrl(pluginParams);
            }
            else
            {
                PlayList.NextPageUrl = null;
            }
            PlayList.Timeout = "60"; //sec

            PlayList.Items = items.ToArray();
            foreach (Item Item in PlayList.Items)
            {
                if (ItemType.DIRECTORY == Item.Type)
                {
                    var pluginParams = new NameValueCollection();
                    pluginParams[PLUGIN_PATH] = Item.Link;
                    Item.Link = context.CreatePluginUrl(pluginParams);
                }
            }
            return PlayList;
        }

        public PluginApi.Plugins.Playlist GetTopList(IPluginContext context)
        {
            Load_Settings();
            System.Collections.Generic.List<Item> items = new System.Collections.Generic.List<Item>();

            System.Net.WebClient WC = new System.Net.WebClient();
            WC.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0");
            WC.Encoding = System.Text.Encoding.UTF8;

            Item ItemTop = new Item();
            Item ItemTorrentTV = new Item();
            Item ItemNNMClub = new Item();
            Item ItemRuTor = new Item();
            Item ItemRuTracker = new Item();
            try
            {
                AceProxEnabl = true;
                string AceMadiaGet = null;
                AceMadiaGet = WC.DownloadString("http://" + IPAdress + ":" + PortAce + "/webui/api/service?method=get_version&format=jsonp&callback=mycallback");
                AceMadiaGet = "<html> Ответ от движка Ace Media получен: " + "<div>" + AceMadiaGet + "</div></html>";


                ItemTop.ImageLink = "http://cs5-2.4pda.to/8001667.png";
                ItemTop.Name = "<span style=\"color:#9DB1CC\"> - AceTorrentPlay - </span>";
                ItemTop.Link = "";
                ItemTop.Type = ItemType.FILE;
                ItemTop.Description = AceMadiaGet + "<html><p><p><img src=\" http://static.acestream.net/sites/acestream/img/ACE-logo.png\"></html>";


                ItemTorrentTV.Name = "Torrent TV";
                ItemTorrentTV.Type = ItemType.DIRECTORY;
                ItemTorrentTV.Link = "torrenttv";
                ItemTorrentTV.ImageLink = "http://s1.iconbird.com/ico/1112/Television/w256h25613523820647.png";

                if (System.IO.File.Exists(System.IO.Path.GetTempPath() + "MyTraf.tmp") == false)
                {
                    WC.DownloadFile("http://super-pomoyka.us.to/trash/ttv-list/MyTraf.php", System.IO.Path.GetTempPath() + "MyTraf.tmp");
                }
                ItemTorrentTV.Description = "<html><img src=\" http://torrent-tv.ru/images/logo.png\"></html><p>" + WC.DownloadString(System.IO.Path.GetTempPath() + "MyTraf.tmp");

                ItemNNMClub.ImageLink = ICO_NNMClub;
                ItemNNMClub.Name = "Tracker NoNaMe-Club";
                ItemNNMClub.Link = "nnmclub";
                ItemNNMClub.Type = ItemType.DIRECTORY;

                string Description_NNMC = "Добро пожаловать на интеллигентный битторрент. Наш торрент-трекер - это место, где можно не только скачать фильмы, музыку и программы, но и найти друзей или просто пообщаться на интересующие Вас темы. Для того, чтобы скачать с помощью торрента не нужно платить. Главное правило торрент-трекера: скачал сам, останься на раздаче. Для этого просто не надо удалять торрент из клиента.";
                ItemNNMClub.Description = "<html><font face=\" Arial\" size=\" 5\"><b>Трекер " + ItemNNMClub.Name + "</font></b><p><img src=\" http://assets.nnm-club.ws/forum/images/logos/10let8.png\" /> <p>" + Description_NNMC + "</html>";

                ItemRuTor.ImageLink = "http://s1.iconbird.com/ico/2013/12/566/w128h1281387223970serpmolot128x128.png";
                ItemRuTor.Name = "Tracker Rutor";
                ItemRuTor.Link = "rutor";
                ItemRuTor.Type = ItemType.DIRECTORY;
                string Description_RuTor = "Файлы для обмена предоставлены пользователями сайта. Администрация не несёт ответственности за их содержание. На сервере хранятся только торрент-файлы. Это значит, что мы не храним никаких нелегальных материалов.";
                ItemRuTor.Description = "<html><font face=\"Arial\" size=\"5\"><b>" + ItemRuTor.Name + "</font></b><p><img src=\"" + TrackerServerRuTor + LOGO_TrackerRutor + "\" /><p>" + Description_RuTor + "</html>";

                ItemRuTracker.ImageLink = "http://s1.iconbird.com/ico/0612/Inside/w256h2561339745864icontextoinsidefavorites.png";
                ItemRuTracker.Name = "Tracker RuTracker";
                ItemRuTracker.Link = "rutr";
                ItemRuTracker.Type = ItemType.DIRECTORY;
                string Description_RuTr = "RuTracker.org (ранее — Torrents.ru) — крупнейший русскоязычный BitTorrent-трекер, насчитывающий более 15,3 миллиона зарегистрированных учётных записей. На трекере зарегистрировано 1,728 миллиона раздач (из которых более 1,60 миллиона — «живых»), суммарный размер которых составляет 3.20 петабайта";
                ItemRuTracker.Description = "<html><font face=\"Arial\" size=\"5\"><b>" + ItemRuTracker.Name + "</font></b><p><img src=\"" + LOGO_RuTr + "\" /><p>" + Description_RuTr + "</html>";

                items.Add(ItemTop);
                items.Add(ItemTorrentTV);
                items.Add(ItemRuTor);
                items.Add(ItemRuTracker);
                items.Add(ItemNNMClub);

            }
            catch (Exception ex)
            {
                AceProxEnabl = false;
                ItemTop.ImageLink = ICO_Error2;
                ItemTop.Name = "<span style=\"color:#FF380A\"> - AceTorrentPlay - </span>";
                ItemTop.Link = "";
                ItemTop.Type = ItemType.FILE;
                ItemTop.Description = "Ответ от движка Ace Media не получен!" + "<p>" + ex.Message + "</p>";
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
                    Item.Description = Item.Name + "\n" + "\r" + " <html><p> Метка диска: " + Disk.VolumeLabel + "</html>";

                    items.Add(Item);
                }
            }

            Item ItemSettings = new Item();
            ItemSettings.Name = "Настройки";
            ItemSettings.Link = ";SETTINGS";
            ItemSettings.Type = ItemType.DIRECTORY;
            ItemSettings.ImageLink = ICO_Settings;
            items.Add(ItemSettings);

            PlayList.IsIptv = "false";
            return PlayListPlugPar(items, context);
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

                System.Text.RegularExpressions.Regex Regex = new System.Text.RegularExpressions.Regex("<header><h3>По вашему запросу ничего не найдено</h3></header>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                bool Bool = Regex.IsMatch(Str);


                if (Bool == true)
                {
                    HtmlFile = "<html><div>Описание не найдено.</div><div>Попробуйте переименовать торрент файл</div></html>";
                }
                else
                {
                    Regex = new System.Text.RegularExpressions.Regex("(?<=fid=\").*(?=\">)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                    string FidStr = Regex.Matches(Str)[0].Value;
                    Str = WC.DownloadString("http://www.kinomania.ru/film/" + FidStr);

                    string Title = null;
                    try
                    {
                        Regex = new System.Text.RegularExpressions.Regex("(?<=<title>).*(?=</title>)");
                        Title = Regex.Matches(Str)[0].Value.Replace("| KINOMANIA.RU", "");
                    }
                    catch (Exception ex)
                    {
                    }

                    string ImagePath = null;
                    try
                    {
                        Regex = new System.Text.RegularExpressions.Regex("(?<=src=\").*?(.jpg)");
                        ImagePath = Regex.Matches(Str)[0].Value;
                        ImagePath = "http://" + IPAdress + ":8027/proxym3u8B" + Base64Encode(ImagePath + "OPT:ContentType--image/jpegOPEND:/") + "/";
                    }
                    catch (Exception ex)
                    {
                    }

                    string Opisanie = null;
                    try
                    {
                        Regex = new System.Text.RegularExpressions.Regex("(<div class=\"l-col l-col-2\">)(\\n|.)*?(</div>)");
                        Opisanie = Regex.Matches(Str)[0].Value;
                    }
                    catch (Exception ex)
                    {
                    }

                    string InfoFile = null;
                    try
                    {
                        Regex = new System.Text.RegularExpressions.Regex("(<h2 class=\"b-switcher\">)(\\n|.)*?(</div>)");
                        InfoFile = Regex.Matches(Str)[0].Value;
                    }
                    catch (Exception ex)
                    {

                    }


                    HtmlFile = "<div id=\"poster\" style=\"float:left;padding:4px;        background-color:#EEEEEE;margin:0px 13px 1px 0px;\">" + "<img src=\"" + ImagePath + "\" style=\"width:180px;float:left;\" /></div><span style=\"color:#3090F0\">" + Title + "</span>" + Opisanie + "<span style=\"color:#3090F0\">Информация</span><br>" + InfoFile;

                }

            }
            catch (Exception ex)
            {
                HtmlFile = ex.Message;
            }
            return HtmlFile;

        }
        #region Rutracker
        private bool ProxyEnablerRuTr = true;
        private string TrackerServer = "https://rutracker.org";
        #region Авторизация
        private string Login;
        private string Password;
        private string Cap_Sid;
        private string Cap_Code;
        private string Capcha;
        private string Cookies;

        private string UserAuthorization;
        public bool AuthorizationTest()
        {
            Cookies = System.Convert.ToString(Microsoft.Win32.Registry.GetValue("HKEY_CURRENT_USER\\Software\\RemoteFork\\Plugins\\RuTracker\\", "Cookies", ""));
            if (Cookies == "")
            {
                Cookies = "bb_ssl=1";
            }
            System.Net.HttpWebRequest Request = System.Net.HttpWebRequest.CreateHttp("https://rutracker.org/forum/index.php");
            Request.Method = "GET";
            Request.Headers.Add("Cookie", Cookies);
            Request.Host = "rutracker.org";
            Request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36";
            Request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            Request.Headers.Add(System.Net.HttpRequestHeader.AcceptLanguage, "ru-ru,ru;q=0.8,en-us;q=0.5,en;q=0.3");
            Request.Headers.Add(System.Net.HttpRequestHeader.AcceptEncoding, "gzip,deflate");
            Request.Headers.Add(System.Net.HttpRequestHeader.AcceptCharset, "windows-1251,utf-8;q=0.7,*;q=0.7");
            Request.KeepAlive = true;
            Request.Referer = "https://rutracker.org/forum/index.php";

            Request.ContentType = "application/x-www-form-urlencoded";
            Request.AllowAutoRedirect = false;
            Request.AutomaticDecompression = System.Net.DecompressionMethods.GZip;
            if (ProxyEnablerRuTr == true)
            {
                Request.Proxy = new System.Net.WebProxy(ProxyServr, ProxyPort);
            }


            System.Net.WebResponse Response = Request.GetResponse();
            System.IO.Stream Stream = Response.GetResponseStream();
            Stream = Response.GetResponseStream();
            System.IO.StreamReader Reader = new System.IO.StreamReader(Stream, System.Text.Encoding.GetEncoding(1251));
            string OtvetServera = Reader.ReadToEnd().Replace("\n", " ");
            Reader.Close();
            Stream.Close();

            System.Text.RegularExpressions.Regex Reg = new System.Text.RegularExpressions.Regex("(>Вход</span>).*?(</span>)");
            System.Text.RegularExpressions.MatchCollection Matchs = Reg.Matches(OtvetServera);

            if (Matchs.Count > 0)
            {
                Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\\Software\\RemoteFork\\Plugins\\RuTracker\\", "Cookies", "bb_ssl=1");
                return false;
            }
            else
            {
                Reg = new System.Text.RegularExpressions.Regex("(<span class=\"logged-in-as-cap\">).*?(</div>)");
                Matchs = Reg.Matches(OtvetServera);
                if (Matchs.Count > 0)
                {
                    Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\\Software\\RemoteFork\\Plugins\\RuTracker\\", "Cookies", Cookies);
                    UserAuthorization = Matchs[0].Value;
                    return true;
                }
            }
            return false;
        }

        public PluginApi.Plugins.Playlist AuthorizationRuTr(IPluginContext context)
        {
            System.Collections.Generic.List<Item> items = new System.Collections.Generic.List<Item>();
            Cookies = "bb_ssl=1";
            System.Net.HttpWebRequest Request = System.Net.HttpWebRequest.CreateHttp("https://rutracker.org/forum/login.php?redirect=tracker.php");
            Request.Method = "POST";
            Request.Headers.Add("Cookie", Cookies);
            Request.Host = "rutracker.org";
            Request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36";
            Request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            Request.Headers.Add(System.Net.HttpRequestHeader.AcceptLanguage, "ru-ru,ru;q=0.8,en-us;q=0.5,en;q=0.3");
            Request.Headers.Add(System.Net.HttpRequestHeader.AcceptEncoding, "gzip,deflate");
            Request.Headers.Add(System.Net.HttpRequestHeader.AcceptCharset, "windows-1251,utf-8;q=0.7,*;q=0.7");
            Request.KeepAlive = true;
            Request.Referer = "https://rutracker.org/forum/index.php";
            Request.ContentType = "application/x-www-form-urlencoded";
            Request.AllowAutoRedirect = false;
            Request.AutomaticDecompression = System.Net.DecompressionMethods.GZip;
            if (ProxyEnablerRuTr == true)
            {
                Request.Proxy = new System.Net.WebProxy(ProxyServr, ProxyPort);
            }

            string StringData = null;
            if (Capcha == "")
            {
                StringData = "redirect=tracker.php&login_username=" + Login + "&login_password=" + Password + "&login=Вход";
            }
            else
            {
                StringData = "redirect=tracker.php&login_username=" + Login + "&login_password=" + Password + "&cap_sid=" + Cap_Sid + "&" + Cap_Code + "=" + Capcha + "&login=%C2%F5%EE%E4";
            }
            Capcha = "";
            System.IO.Stream Stream = Request.GetRequestStream();
            byte[] ByteData = System.Text.Encoding.GetEncoding(1251).GetBytes(StringData);
            Stream.Write(ByteData, 0, ByteData.Length);
            Stream.Close();


            System.Net.WebResponse Response = Request.GetResponse();
            Stream = Response.GetResponseStream();
            System.IO.StreamReader Reader = new System.IO.StreamReader(Stream, System.Text.Encoding.GetEncoding(1251));
            string OtvetServera = Reader.ReadToEnd().Replace("\n", " ");

            if (!(string.IsNullOrEmpty(Response.Headers["Set-Cookie"])))
            {

                Cookies = Response.Headers["Set-Cookie"];
                Request = System.Net.HttpWebRequest.CreateHttp("https://rutracker.org/forum/index.php");
                Request.Method = "GET";
                Request.Headers.Add("Cookie", Cookies);
                Request.Host = "rutracker.org";
                Request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36";
                Request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                Request.Headers.Add(System.Net.HttpRequestHeader.AcceptLanguage, "ru-ru,ru;q=0.8,en-us;q=0.5,en;q=0.3");
                Request.Headers.Add(System.Net.HttpRequestHeader.AcceptEncoding, "gzip,deflate");
                Request.Headers.Add(System.Net.HttpRequestHeader.AcceptCharset, "windows-1251,utf-8;q=0.7,*;q=0.7");
                Request.KeepAlive = true;
                Request.Referer = "https://rutracker.org/forum/login.php?redirect=tracker.php";
                Request.Headers.Add(System.Net.HttpRequestHeader.Cookie, "spylog_test=1");
                Request.ContentType = "application/x-www-form-urlencoded";
                Request.AllowAutoRedirect = false;
                Request.AutomaticDecompression = System.Net.DecompressionMethods.GZip;
                if (ProxyEnablerRuTr == true)
                {
                    Request.Proxy = new System.Net.WebProxy(ProxyServr, ProxyPort);
                }

                Response = Request.GetResponse();
                Stream = Response.GetResponseStream();
                Reader = new System.IO.StreamReader(Stream, System.Text.Encoding.GetEncoding(1251));
                OtvetServera = Reader.ReadToEnd().Replace("\n", " ");
                Reader.Close();
                Stream.Close();

                System.Text.RegularExpressions.Regex Reg = new System.Text.RegularExpressions.Regex("(<a href=\"profile.php?mode=register\"><b>).*?(</span>)");
                System.Text.RegularExpressions.MatchCollection Matchs = Reg.Matches(OtvetServera);

                if (Matchs.Count > 0)
                {
                    Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\\Software\\RemoteFork\\Plugins\\RuTracker\\", "Cookies", "bb_ssl=1");
                    return SetLogin(context);
                }
                else
                {
                    Reg = new System.Text.RegularExpressions.Regex("(<span class=\"logged-in-as-cap\">).*?(</div>)");
                    Matchs = Reg.Matches(OtvetServera);
                    if (Matchs.Count > 0)
                    {
                        Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\\Software\\RemoteFork\\Plugins\\RuTracker\\", "Cookies", Cookies);
                        return GetTopListRuTr(context);
                    }
                }
            }

            string AdressCapha = null;
            System.Text.RegularExpressions.Regex Regex = new System.Text.RegularExpressions.Regex("(?<=<td class=\"tRight nowrap\">Код:</td> 					<td> 			<div><img src=\"//).*?(.jpg)");
            System.Text.RegularExpressions.MatchCollection Matches = Regex.Matches(OtvetServera);
            if (Matches.Count > 0)
            {
                AdressCapha = "http://" + Regex.Matches(OtvetServera)[0].Value;
                Regex = new System.Text.RegularExpressions.Regex("(?<=name=\"cap_sid\" value=\").*?(?=\">)");
                Cap_Sid = Regex.Matches(OtvetServera)[0].Value;
                Regex = new System.Text.RegularExpressions.Regex("(cap_code_).*?(?=\")");
                Cap_Code = Regex.Matches(OtvetServera)[0].Value;


                Regex = new System.Text.RegularExpressions.Regex("(<h4 class=\"warnColor1 tCenter mrg_16\">).*?(</h4>)");
                Matches = Regex.Matches(OtvetServera);

                Item ItemCap = new Item();
                ItemCap.Name = "Capcha";
                ItemCap.SearchOn = "Введите код";
                ItemCap.Link = "RuTr_Capcha_Key";
                ItemCap.Description = Matches[0].Value + "<img src=\"" + AdressCapha + "\" width=\"120\" height=\"72\">";
                ItemCap.ImageLink = AdressCapha;
                items.Add(ItemCap);

            }




            PlayList.IsIptv = "false";
            return PlayListPlugPar(items, context);
        }

        public PluginApi.Plugins.Playlist SetLogin(IPluginContext context)
        {
            System.Collections.Generic.List<Item> items = new System.Collections.Generic.List<Item>();
            Item Item = new Item();

            Item.Name = "Login";
            Item.Link = "RuTr_Login";
            Item.Type = ItemType.DIRECTORY;
            Item.SearchOn = "Login";
            Item.ImageLink = ICO_Login;
            items.Add(Item);

            PlayList.IsIptv = "false";
            return PlayListPlugPar(items, context);
        }


        public PluginApi.Plugins.Playlist SetPassword(IPluginContext context)
        {
            System.Collections.Generic.List<Item> items = new System.Collections.Generic.List<Item>();
            Item Item = new Item();

            Item = new Item();
            Item.Name = "Password";
            Item.Link = "RuTr_Password";
            Item.Type = ItemType.DIRECTORY;
            Item.SearchOn = "Password";
            Item.ImageLink = ICO_Password;
            items.Add(Item);

            PlayList.IsIptv = "false";
            return PlayListPlugPar(items, context);
        }
        #endregion

        private string KategoriRuTracker;

        public PluginApi.Plugins.Playlist GetListRuTrCategory(IPluginContext context, string Groop)
        {
            return GetListRuTrCategory(context, Groop, null);
        }

        //INSTANT C# NOTE: Overloaded method(s) are created above to convert the following method having optional parameters:
        //ORIGINAL LINE: Public Function GetListRuTrCategory(context As IPluginContext, ByVal Groop As String, Optional ByVal SubGroop As String = null) As PluginApi.Plugins.Playlist
        public PluginApi.Plugins.Playlist GetListRuTrCategory(IPluginContext context, string Groop, string SubGroop)
        {

            List<Item> items = new List<Item>();


            System.Text.RegularExpressions.Regex Regex = new System.Text.RegularExpressions.Regex("(<optgroup label=\"&nbsp;" + Groop + ").*?(optgroup>)");
            string GroopText = Regex.Matches(KategoriRuTracker)[0].Value.Replace(" |- ", "::").Replace("(", " ").Replace(")", " ");



            switch (SubGroop)
            {
                case null:
                    {

                        Regex = new System.Text.RegularExpressions.Regex("(<option).*?(option>)");
                        System.Text.RegularExpressions.MatchCollection Matches = Regex.Matches(GroopText);
                        System.Text.RegularExpressions.Regex RegexOptionsID = new System.Text.RegularExpressions.Regex("(?<=value=\").*?(?=\")");
                        string IDSubGroops = null;
                        foreach (System.Text.RegularExpressions.Match SSGroop in Matches)
                        {
                            IDSubGroops = IDSubGroops + RegexOptionsID.Match(SSGroop.Value).Value + ",";
                        }
                        Item ItemSearch = new Item();
                        ItemSearch.Name = "Поиск";
                        ItemSearch.Link = IDSubGroops.Remove(IDSubGroops.Length - 1) + ";Search_Groop_RuTr";
                        ItemSearch.Type = ItemType.DIRECTORY;
                        ItemSearch.SearchOn = "Поик в категории";
                        ItemSearch.ImageLink = ICO_Search;
                        ItemSearch.Description = "<html><font face=\"Arial\" size=\"5\"><b>" + ItemSearch.Name + "</font></b><p><img src=\"" + LOGO_RuTr + "\" /> <p>" + UserAuthorization;
                        items.Add(ItemSearch);


                        Regex = new System.Text.RegularExpressions.Regex("(?<=class=\"root_forum has_sf\">).*?(?=&nbsp;)");
                        foreach (System.Text.RegularExpressions.Match SGroop in Regex.Matches(GroopText))
                        {
                            Item Item = new Item();
                            Item.Name = SGroop.Value;
                            switch (Item.Name)
                            {
                                case "F.A.Q.":
                                    break;
                                default:
                                    Item.Type = ItemType.DIRECTORY;
                                    Item.ImageLink = ICO_Folder;
                                    Item.Description = "<html><font face=\"Arial\" size=\"5\"><b>" + Item.Name + "</font></b><p><img src=\"" + LOGO_RuTr + "\" /> <p>" + UserAuthorization;
                                    Item.Link = Groop + ";" + Item.Name + ";RuTrSubGroop";
                                    items.Add(Item);
                                    break;
                            }
                        }
                        break;
                    }
                default:
                    {




                        Regex = new System.Text.RegularExpressions.Regex("(<option).*?(class=\"root_forum has_sf\">" + SubGroop + ")");
                        string SubSubGroopStart = Regex.Match(GroopText.Replace("^", "\n")).Value;


                        Regex = new System.Text.RegularExpressions.Regex("(" + SubSubGroopStart + ").*?(?=class=\"root_forum has_sf\">|optgroup>)");
                        string Options = Regex.Match(GroopText).Value;

                        Regex = new System.Text.RegularExpressions.Regex("(<option).*?(option>)");
                        System.Text.RegularExpressions.Regex RegexOptionsName = new System.Text.RegularExpressions.Regex("(?<=\"root_forum has_sf\">|\"root_forum\">|::).*?(?=&nbsp;)");
                        System.Text.RegularExpressions.Regex RegexOptionsID = new System.Text.RegularExpressions.Regex("(?<=value=\").*?(?=\")");


                        System.Text.RegularExpressions.MatchCollection Matches = Regex.Matches(Options);

                        string IDSubGroops = null;
                        foreach (System.Text.RegularExpressions.Match SSGroop in Matches)
                        {
                            IDSubGroops = IDSubGroops + RegexOptionsID.Match(SSGroop.Value).Value + ",";
                        }
                        Item ItemSearch = new Item();
                        ItemSearch.Name = "Поиск";
                        ItemSearch.Link = IDSubGroops.Remove(IDSubGroops.Length - 1) + ";Search_Groop_RuTr";
                        ItemSearch.Type = ItemType.DIRECTORY;
                        ItemSearch.SearchOn = "Поик в подкатегории";
                        ItemSearch.ImageLink = ICO_Search;
                        ItemSearch.Description = "<html><font face=\"Arial\" size=\"5\"><b>" + ItemSearch.Name + "</font></b><p><img src=\"" + LOGO_RuTr + "\" /> <p>" + UserAuthorization;
                        items.Add(ItemSearch);

                        foreach (System.Text.RegularExpressions.Match SSGroop in Matches)
                        {
                            Item Item = new Item();
                            Item.Name = RegexOptionsName.Match(SSGroop.Value).Value;
                            Item.Type = ItemType.DIRECTORY;
                            Item.ImageLink = ICO_FolderVideo;
                            Item.Description = "<html><font face=\"Arial\" size=\"5\"><b>" + Item.Name + "</font></b><p><img src=\"" + LOGO_RuTr + "\" /> <p>" + UserAuthorization;
                            Item.Link = TrackerServer + "/forum/tracker.php?f=" + RegexOptionsID.Match(SSGroop.Value).Value + ";PAGERUTR";
                            items.Add(Item);
                        }



                        break;
                    }
            }


            PlayList.IsIptv = "False";
            return PlayListPlugPar(items, context);
        }

        public PluginApi.Plugins.Playlist GetPageRuTr(IPluginContext context, string URL)
        {
            System.Net.WebRequest RequestPost = System.Net.WebRequest.Create(URL);

            if (ProxyEnablerRuTr == true)
            {
                RequestPost.Proxy = new System.Net.WebProxy(ProxyServr, ProxyPort);
            }
            RequestPost.Method = "POST";
            RequestPost.ContentType = "text/html; charset=windows-1251";
            RequestPost.Headers.Add("Cookie", Cookies);
            RequestPost.ContentType = "application/x-www-form-urlencoded";
            System.IO.Stream myStream = RequestPost.GetRequestStream();
            byte[] DataByte = System.Text.Encoding.GetEncoding(1251).GetBytes("prev_new=0&prev_oop=0&o=1&s=2&tm=-1&pn=&nm=");
            myStream.Write(DataByte, 0, DataByte.Length);
            myStream.Close();

            System.Net.WebResponse Response = RequestPost.GetResponse();
            System.IO.Stream dataStream = Response.GetResponseStream();
            System.IO.StreamReader reader = new System.IO.StreamReader(dataStream, System.Text.Encoding.GetEncoding(1251));
            string ResponseFromServer = reader.ReadToEnd();

            System.Collections.Generic.List<Item> items = new System.Collections.Generic.List<Item>();

            System.Text.RegularExpressions.Regex Regex = new System.Text.RegularExpressions.Regex("start|f-1");
            if (Regex.IsMatch(URL) == false)
            {
                Regex = new System.Text.RegularExpressions.Regex("(?<=f=).*?(?=&|$)");
                Item ItemSearch = new Item();
                ItemSearch.Name = "Поиск";
                ItemSearch.Link = Regex.Match(URL).Value + ";Search_Groop_RuTr";
                ItemSearch.Type = ItemType.DIRECTORY;
                ItemSearch.SearchOn = "Поик в категории";
                ItemSearch.ImageLink = ICO_Search;
                ItemSearch.Description = "<html><font face=\"Arial\" size=\"5\"><b>" + ItemSearch.Name + "</font></b><p><img src=\"" + LOGO_RuTr + "\" /> <p>" + UserAuthorization;
                items.Add(ItemSearch);
            }

            Regex = new System.Text.RegularExpressions.Regex("(<tr class=\"tCenter hl-tr\">).*?(</tr>)");
            System.Text.RegularExpressions.MatchCollection Result = Regex.Matches(ResponseFromServer.Replace("\n", " "));
            if (Result.Count > 0)
            {
                foreach (System.Text.RegularExpressions.Match Match in Result)
                {
                    Item Item = new Item();
                    Regex = new System.Text.RegularExpressions.Regex("(?<=<a data-topic_id=\").*?(?=\")");
                    string LinkID = Regex.Matches(Match.Value)[0].Value;
                    Item.Link = TrackerServer + "/forum/viewtopic.php?t=" + LinkID + ";PAGEFILMRUTR";
                    Regex = new System.Text.RegularExpressions.Regex("(?<=" + LinkID + "\">).*?(?=</a>)");
                    Item.Name = Regex.Matches(Match.Value)[0].Value;
                    Item.ImageLink = ICO_TorrentFile;
                    Item.Description = GetDescriptionRuTr(Match.Value);
                    items.Add(Item);
                }
            }
            else
            {
                return NonSearch(context, true);
            }


            next_page_url = null;
            Regex = new System.Text.RegularExpressions.Regex("(?<=&amp;start=).*?(?=\")");
            Result = Regex.Matches(ResponseFromServer);
            if (Result.Count > 0)
            {
                Regex = new System.Text.RegularExpressions.Regex("(.*).*(?=&start)");
                System.Text.RegularExpressions.MatchCollection Matchs = Regex.Matches(URL);
                if (Matchs.Count > 0)
                {
                    next_page_url = Matchs[0].Value + "&start=" + Result[Result.Count - 1].Value + ";PAGERUTR";
                }
                else
                {
                    next_page_url = URL + "&start=" + Result[Result.Count - 1].Value + ";PAGERUTR";
                }
            }


            PlayList.IsIptv = "false";
            return PlayListPlugPar(items, context, next_page_url);
        }

        public string GetDescriptionRuTr(string HTML)
        {


            string Title = null;
            System.Text.RegularExpressions.Regex RegexTop = new System.Text.RegularExpressions.Regex("(?<=href=\"viewtopic.php).*?(?=</a>)");
            System.Text.RegularExpressions.Regex RegexSub = new System.Text.RegularExpressions.Regex("(?<=>).*(.*)");
            try
            {
                Title = RegexSub.Matches(RegexTop.Matches(HTML)[0].Value)[0].Value;
            }
            catch (Exception ex)
            {
            }




            RegexTop = new System.Text.RegularExpressions.Regex("(?<=<a class=\"small tr-dl dl-stub\").*?(?=;</a>)");
            RegexSub = new System.Text.RegularExpressions.Regex("(?<=\">).*(?=&)");
            string SizeFile = null;
            try
            {
                SizeFile = "<br>Размер: " + RegexSub.Matches(RegexTop.Matches(HTML)[0].Value)[0].Value;
            }
            catch (Exception ex)
            {
            }

            RegexTop = new System.Text.RegularExpressions.Regex("(?<=title=\"Личи\"><b>).*?(</b>)");
            RegexSub = new System.Text.RegularExpressions.Regex("(?<=<b class=\"seedmed\">).*?(?=</b>)");
            string SidsPirs = null;
            try
            {
                SidsPirs = "<br><br>Seeders: " + RegexSub.Matches(HTML)[0].Value + "<br>Leechers: " + RegexTop.Matches(HTML)[0].Value;
            }
            catch (Exception ex)
            {
            }

            RegexTop = new System.Text.RegularExpressions.Regex("(?<=<a class=\"gen f\").*?(?=</a>)");
            RegexSub = new System.Text.RegularExpressions.Regex("(?<=\">).*(.*)");
            string Razdel = null;
            try
            {
                Razdel = "<br><br>Раздел: " + RegexSub.Matches(RegexTop.Matches(HTML)[0].Value)[0].Value;
            }
            catch (Exception ex)
            {
            }

            RegexTop = new System.Text.RegularExpressions.Regex("(?<=<td class=\"row4 small nowrap\").*?(?=</td>)");
            RegexSub = new System.Text.RegularExpressions.Regex("(?<=<p>).*(?=</p>)");
            string DataCreate = null;
            try
            {

                DataCreate = "<br><br>Создан: " + RegexSub.Matches(RegexTop.Matches(HTML)[0].Value)[0].Value;
            }
            catch (Exception ex)
            {
            }




            return "<span style=\"color:#3090F0\">" + Title + "</span><br>" + SizeFile + SidsPirs + Razdel + DataCreate;
        }

        public PluginApi.Plugins.Playlist GetTopListRuTr(IPluginContext context)
        {
            if (AuthorizationTest() == false)
            {
                return SetLogin(context);
            }

            System.Collections.Generic.List<Item> items = new System.Collections.Generic.List<Item>();
            Item Item = new Item();

            Item.Name = "Поиск";
            Item.Link = "Search_rutracker";
            Item.Type = ItemType.DIRECTORY;
            Item.SearchOn = "Поик на RuTracker";
            Item.ImageLink = ICO_Search;
            Item.Description = "<html><font face=\"Arial\" size=\"5\"><b>" + Item.Name + "</font></b><p><img src=\"" + LOGO_RuTr + "\" /> <p>" + UserAuthorization;
            items.Add(Item);

            Item = new Item();
            Item.Name = "Торренты за сегодня";
            Item.Link = TrackerServer + "/forum/tracker.php?f-1;PAGERUTR";
            Item.Type = ItemType.DIRECTORY;
            Item.ImageLink = ICO_FolderVideo;
            Item.Description = "<html><font face=\"Arial\" size=\"5\"><b>" + Item.Name + "</font></b><p><img src=\"" + LOGO_RuTr + "\" /> <p>" + UserAuthorization;
            items.Add(Item);


            System.Net.WebRequest RequestPost = System.Net.WebRequest.Create(TrackerServer + "/forum/tracker.php");
            if (ProxyEnablerRuTr == true)
            {
                RequestPost.Proxy = new System.Net.WebProxy(ProxyServr, ProxyPort);
            }
            RequestPost.Method = "POST";
            RequestPost.ContentType = "text/html; charset=windows-1251";
            RequestPost.Headers.Add("Cookie", Cookies);
            RequestPost.ContentType = "application/x-www-form-urlencoded";
            System.IO.Stream myStream = RequestPost.GetRequestStream();
            byte[] DataByte = System.Text.Encoding.GetEncoding(1251).GetBytes("prev_new=0&prev_oop=0&o=1&s=2&tm=-1&pn=&nm=");
            myStream.Write(DataByte, 0, DataByte.Length);
            myStream.Close();

            System.Net.WebResponse Response = RequestPost.GetResponse();
            System.IO.Stream dataStream = Response.GetResponseStream();
            System.IO.StreamReader reader = new System.IO.StreamReader(dataStream, System.Text.Encoding.GetEncoding(1251));
            string ResponseFromServer = reader.ReadToEnd().Replace("\n", "^");

            System.Text.RegularExpressions.Regex Regex = new System.Text.RegularExpressions.Regex("(<p class=\"select\">).*?(</select>)");
            KategoriRuTracker = Regex.Matches(ResponseFromServer)[0].Value;


            Regex = new System.Text.RegularExpressions.Regex("(<optgroup).*?(</optgroup>)");
            foreach (System.Text.RegularExpressions.Match Groop in Regex.Matches(KategoriRuTracker))
            {

                Regex = new System.Text.RegularExpressions.Regex("(?<=label=\"&nbsp;).*?(?=\")");

                switch (Regex.Matches(Groop.Value)[0].Value)
                {
                    case "Новости":
                    case "Книги и журналы":
                    case "Игры":
                    case "Программы и Дизайн":
                    case "Обсуждения, встречи, общение":
                        break;
                    default:
                        Item = new Item();
                        Item.Name = Regex.Matches(Groop.Value)[0].Value;
                        Item.Link = Item.Name + ";RuTrGroop";
                        Item.Type = ItemType.DIRECTORY;
                        Item.ImageLink = ICO_Folder;
                        Item.Description = "<html><font face=\"Arial\" size=\"5\"><b>" + Item.Name + "</font></b><p><img src=\"" + LOGO_RuTr + "\" /> <p>" + UserAuthorization;
                        items.Add(Item);
                        break;
                }
            }


            Item = new Item();
            Item.Name = "";
            Item.Link = "";
            Item.Type = ItemType.FILE;
            Item.ImageLink = ICO_Pusto;
            items.Add(Item);


            Item = new Item();
            Item.Name = "Выйти с RuTracker";
            Item.Link = "RuTrNonAuthorization";
            Item.Type = ItemType.DIRECTORY;
            Item.ImageLink = ICO_Delete;
            items.Add(Item);

            PlayList.IsIptv = "False";
            return PlayListPlugPar(items, context);
        }

        public PluginApi.Plugins.Playlist GetTorrentPageRuTr(IPluginContext context, string URL)
        {

            System.Net.WebRequest RequestGet = System.Net.WebRequest.Create(URL);
            if (ProxyEnablerRuTr == true)
            {
                RequestGet.Proxy = new System.Net.WebProxy(ProxyServr, ProxyPort);
            }
            RequestGet.Method = "Get";
            RequestGet.Headers.Add("Cookie", Cookies);

            System.Net.WebResponse Response = RequestGet.GetResponse();
            System.IO.Stream dataStream = Response.GetResponseStream();
            System.IO.StreamReader reader = new System.IO.StreamReader(dataStream, System.Text.Encoding.GetEncoding(1251));
            string responseFromServer = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            Response.Close();



            System.Text.RegularExpressions.Regex Regex = new System.Text.RegularExpressions.Regex("(?<=<p><a href=\").*?(?=\")");
            string TorrentPath = TrackerServer + "/forum/" + Regex.Matches(responseFromServer)[0].Value;


            System.Net.WebRequest RequestTorrent = System.Net.WebRequest.Create(TorrentPath);
            if (ProxyEnablerRuTr == true)
            {
                RequestTorrent.Proxy = new System.Net.WebProxy(ProxyServr, ProxyPort);
            }
            RequestTorrent.Method = "Get";
            RequestTorrent.Headers.Add("Cookie", Cookies);

            Response = RequestTorrent.GetResponse();
            dataStream = Response.GetResponseStream();
            reader = new System.IO.StreamReader(dataStream, System.Text.Encoding.GetEncoding(1251));
            string FileTorrent = reader.ReadToEnd();
            System.IO.File.WriteAllText(System.IO.Path.GetTempPath() + "TorrentTemp.torrent", FileTorrent, System.Text.Encoding.GetEncoding(1251));
            reader.Close();
            dataStream.Close();
            Response.Close();


            TorrentPlayList[] PlayListtoTorrent = GetFileList(System.IO.Path.GetTempPath() + "TorrentTemp.torrent");

            System.Collections.Generic.List<Item> items = new System.Collections.Generic.List<Item>();

            string Description = FormatDescriptionFileRuTr(responseFromServer);
            foreach (TorrentPlayList PlayListItem in PlayListtoTorrent)
            {

                Item Item = new Item();
                Item.Name = PlayListItem.Name;
                Item.ImageLink = PlayListItem.ImageLink;
                Item.Link = PlayListItem.Link;
                Item.Type = ItemType.FILE;
                Item.Description = Description;
                items.Add(Item);
            }



            PlayList.IsIptv = "false";
            return PlayListPlugPar(items, context);
        }

        public string FormatDescriptionFileRuTr(string HTML)
        {

            HTML = HTML.Replace("\n", "");

            string Title = null;
            try
            {
                System.Text.RegularExpressions.Regex Regex = new System.Text.RegularExpressions.Regex("(?<=<title>).*?(</title>)");
                Title = Regex.Matches(HTML)[0].Value;
            }
            catch (Exception ex)
            {
                Title = ex.Message;
            }

            string SidsPirs = null;
            try
            {
                System.Text.RegularExpressions.Regex Regex = new System.Text.RegularExpressions.Regex("(<td class=\"catTitle\">).*?(?=<td class=\"row3 pad_4\">)");
                SidsPirs = Regex.Matches(HTML)[0].Value;
            }
            catch (Exception ex)
            {
                SidsPirs = ex.Message;
            }


            string ImagePath = null;
            try
            {
                System.Text.RegularExpressions.Regex Regex = new System.Text.RegularExpressions.Regex("(?<=<var class=\"postImg postImgAligned img-right\" title=\").*?(?=\">)");
                ImagePath = Regex.Matches(HTML)[0].Value;
            }
            catch (Exception ex)
            {
            }


            string InfoFile = null;
            try
            {
                System.Text.RegularExpressions.Regex Regex = new System.Text.RegularExpressions.Regex("(<span class=\"post-b\">).*(?=<div class=\"sp-wrap\">)");
                InfoFile = Regex.Matches(HTML)[0].Value;
            }
            catch (Exception ex)
            {
                InfoFile = ex.Message;
            }

            return "<div id=\"poster\" style=\"float:left;padding:4px;        background-color:#EEEEEE;margin:0px 13px 1px 0px;\">" + "<img src=\"" + ImagePath + "\" style=\"width:180px;float:left;\" /></div><span style=\"color:#3090F0\">" + Title + "</span><br><font face=\"Arial Narrow\" size=\"4\"><span style=\"color:#70A4A3\">" + SidsPirs + "</font></span>" + InfoFile;

        }

        public PluginApi.Plugins.Playlist SearchListRuTr(IPluginContext context, string search)
        {
            return SearchListRuTr(context, search, null);
        }

        //INSTANT C# NOTE: Overloaded method(s) are created above to convert the following method having optional parameters:
        //ORIGINAL LINE: Public Function SearchListRuTr(context As IPluginContext, ByVal search As String, Optional ByVal Category As String = null) As PluginApi.Plugins.Playlist
        public PluginApi.Plugins.Playlist SearchListRuTr(IPluginContext context, string search, string Category)
        {
            System.Net.WebRequest RequestPost = null;
            if (Category == "")
            {
                RequestPost = System.Net.WebRequest.Create(TrackerServer + "/forum/tracker.php?nm=" + search);
            }
            else
            {
                RequestPost = System.Net.WebRequest.Create(TrackerServer + "/forum/tracker.php?f=" + Category + "&nm=" + search);
            }



            if (ProxyEnablerRuTr == true)
            {
                RequestPost.Proxy = new System.Net.WebProxy(ProxyServr, ProxyPort);
            }
            RequestPost.Method = "POST";
            RequestPost.ContentType = "text/html; charset=windows-1251";
            RequestPost.Headers.Add("Cookie", Cookies);
            RequestPost.ContentType = "application/x-www-form-urlencoded";
            System.IO.Stream myStream = RequestPost.GetRequestStream();
            string DataStr = "prev_new=0&prev_oop=0&o=10&s=2&pn=&nm=" + search;
            byte[] DataByte = System.Text.Encoding.GetEncoding(1251).GetBytes(DataStr);
            myStream.Write(DataByte, 0, DataByte.Length);
            myStream.Close();

            System.Net.WebResponse Response = RequestPost.GetResponse();
            System.IO.Stream dataStream = Response.GetResponseStream();
            System.IO.StreamReader reader = new System.IO.StreamReader(dataStream, System.Text.Encoding.GetEncoding(1251));
            string ResponseFromServer = reader.ReadToEnd();


            System.Collections.Generic.List<Item> items = new System.Collections.Generic.List<Item>();
            System.Text.RegularExpressions.Regex Regex = new System.Text.RegularExpressions.Regex("(<tr class=\"tCenter hl-tr\">).*?(</tr>)");
            System.Text.RegularExpressions.MatchCollection Result = Regex.Matches(ResponseFromServer.Replace("\n", " "));

            if (Result.Count > 0)
            {

                foreach (System.Text.RegularExpressions.Match Match in Result)
                {
                    Item Item = new Item();
                    Regex = new System.Text.RegularExpressions.Regex("(?<=<a data-topic_id=\").*?(?=\")");
                    string LinkID = Regex.Matches(Match.Value)[0].Value;
                    Item.Link = TrackerServer + "/forum/viewtopic.php?t=" + LinkID + ";PAGEFILMRUTR";
                    Regex = new System.Text.RegularExpressions.Regex("(?<=" + LinkID + "\">).*?(?=</a>)");
                    Item.Name = Regex.Matches(Match.Value)[0].Value;
                    Item.ImageLink = ICO_TorrentFile;
                    Item.Description = GetDescriptionRuTr(Match.Value);
                    items.Add(Item);
                }
            }
            else
            {
                return NonSearch(context);
            }

            next_page_url = null;

            PlayList.IsIptv = "false";
            return PlayListPlugPar(items, context);
        }

        #endregion


        #region RuTor
        private string TrackerServerRuTor = "http://mega-tor.org";

        public PluginApi.Plugins.Playlist GetTorrentPageRuTor(IPluginContext context, string URL)
        {
            System.Collections.Generic.List<Item> items = new System.Collections.Generic.List<Item>();
            System.Net.WebClient WC = new System.Net.WebClient();
            WC.Encoding = System.Text.Encoding.UTF8;
            WC.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36");
            string ResponseFromServer = WC.DownloadString(URL).Replace("\n", " ");


            System.Text.RegularExpressions.Regex Regex = new System.Text.RegularExpressions.Regex("(/download/).*?(?=\">)");
            string TorrentPath = TrackerServerRuTor + Regex.Matches(ResponseFromServer)[0].Value;
            TorrentPlayList[] PlayListtoTorrent = GetFileList(TorrentPath);

            string Description = FormatDescriptionFileRuTor(ResponseFromServer);
            foreach (TorrentPlayList PlayListItem in PlayListtoTorrent)
            {

                Item Item = new Item();
                Item.Name = PlayListItem.Name;
                Item.ImageLink = PlayListItem.ImageLink;
                Item.Link = PlayListItem.Link;
                Item.Type = ItemType.FILE;
                Item.Description = Description;
                items.Add(Item);
            }

            Regex = new System.Text.RegularExpressions.Regex("(Связанные раздачи).*?(Файлы)");
            System.Text.RegularExpressions.MatchCollection Matches = Regex.Matches(ResponseFromServer);

            if (Matches.Count > 0)
            {

                Item Item = new Item();
                Item.Name = "<span style=\"color:#C0DAE3\">" + "- СВЯЗАННЫЕ РАЗДАЧИ -" + "</span>";
                Item.ImageLink = ICO_Pusto;
                Item.Type = ItemType.FILE;
                items.Add(Item);

                Regex = new System.Text.RegularExpressions.Regex("(?<=<a href=\").*?(?=\")");
                System.Text.RegularExpressions.MatchCollection MatchesSearchNext = Regex.Matches(Matches[0].Value);
                Item ItemSearchNext = new Item();
                ItemSearchNext.ImageLink = ICO_Search2;
                ItemSearchNext.Name = "<span style=\"color:#C0E3D3\">" + "Искать ещё похожие раздачи" + "</span>";
                ItemSearchNext.Link = TrackerServerRuTor + MatchesSearchNext[MatchesSearchNext.Count - 1].Value + ";PAGERUTOR";



                Regex = new System.Text.RegularExpressions.Regex("(<a href=\"magnet:).*?(</span></td></tr>)");

                Matches = Regex.Matches(Matches[0].Value);

                foreach (System.Text.RegularExpressions.Match Macth in Matches)
                {
                    Item = new Item();

                    Item.ImageLink = ICO_TorrentFile2;
                    Regex = new System.Text.RegularExpressions.Regex("(?<=<a href=\").*?(?=\">)");
                    Item.Link = TrackerServerRuTor + Regex.Matches(Macth.Value)[1].Value + ";PAGEFILMRUTOR";

                    Regex = new System.Text.RegularExpressions.Regex("(?<=\">).*?(?=</a>)");
                    Item.Name = "<span style=\"color:#E3D6C0\">" + Regex.Matches(Macth.Value)[1].Value + "</span>";

                    Regex = new System.Text.RegularExpressions.Regex("(<td align=\"right\">).*?(</td>)");
                    System.Text.RegularExpressions.MatchCollection MatchSize = Regex.Matches(Macth.Value);
                    string SizeFile = MatchSize[MatchSize.Count - 1].Value;
                    SizeFile = "Размер: " + SizeFile;

                    Regex = new System.Text.RegularExpressions.Regex("(?<=alt=\"S\" />&nbsp;).*?(?=<)");
                    string Seeders = "Seeders: " + Regex.Matches(Macth.Value)[0].Value;


                    Regex = new System.Text.RegularExpressions.Regex("(?<=alt=\"L\" /><span class=\"red\">&nbsp;).*?(?=</span>)");
                    string Leechers = "Leechers: " + Regex.Matches(Macth.Value)[0].Value;

                    Item.Description = "</div><span style=\"color:#3090F0\">" + Item.Name + "</span><p><br>" + SizeFile + "<br><p>" + Seeders + "<br>" + Leechers;

                    items.Add(Item);
                }
                items.Add(ItemSearchNext);

            }





            PlayList.IsIptv = "false";
            return PlayListPlugPar(items, context);
        }

        public string FormatDescriptionFileRuTor(string HTML)
        {


            string Title = null;
            try
            {
                System.Text.RegularExpressions.Regex Regex = new System.Text.RegularExpressions.Regex("(?<=<h1>).*?(?=<h1>)");
                Title = Regex.Matches(HTML)[0].Value;
            }
            catch (Exception ex)
            {
                Title = ex.Message;
            }




            try
            {
                System.Text.RegularExpressions.Regex Regex = new System.Text.RegularExpressions.Regex("(<table id=\"details\">).*?(</table>)");
                HTML = Regex.Matches(HTML)[0].Value;
            }
            catch (Exception ex)
            {

            }


            string SidsPirs = null;
            try
            {
                System.Text.RegularExpressions.Regex Regex = new System.Text.RegularExpressions.Regex("(<td class=\"header\">Раздают).*?(?=<td class=\"header\" nowrap=\"nowrap\">Добавить)");
                SidsPirs = Regex.Matches(HTML)[0].Value.Replace("</td><td>", ":(</td><td>").Replace("</td></tr>", "</td></tr>) ");
            }
            catch (Exception ex)
            {
                SidsPirs = ex.Message;
            }

            string ImagePath = null;
            try
            {
                System.Text.RegularExpressions.Regex Regex = new System.Text.RegularExpressions.Regex("(?<=<img src=\").*?(?=\")");
                ImagePath = "http://" + IPAdress + ":8027/proxym3u8B" + Base64Encode(Regex.Matches(HTML)[1].Value + "OPT:ContentType--image/jpegOPEND:/") + "/";
            }
            catch (Exception ex)
            {
            }


            string InfoFile = HTML;

            InfoFile = replacetags(InfoFile).Replace("<br /><br />", "");
            Title = replacetags(Title);



            return "<div id=\"poster\" style=\"float:left;padding:4px;        background-color:#EEEEEE;margin:0px 13px 1px 0px;\">" + "<img src=\"" + ImagePath + "\" style=\"width:180px;float:left;\" /></div><span style=\"color:#3090F0\">" + Title + "</span><br><font face=\"Arial Narrow\" size=\"4\"><span style=\"color:#70A4A3\">" + SidsPirs + "</font></span>" + InfoFile;

        }


        public PluginApi.Plugins.Playlist GetPAGERUTOR(IPluginContext context, string URL)
        {

            System.Collections.Generic.List<Item> items = new System.Collections.Generic.List<Item>();
            System.Net.WebClient WC = new System.Net.WebClient();
            WC.Encoding = System.Text.Encoding.UTF8;
            WC.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36");
            string ResponseFromServer = WC.DownloadString(URL).Replace("\n", " ");

            System.Text.RegularExpressions.Regex Regex = new System.Text.RegularExpressions.Regex("(<a href=\"magnet).*?(</span></td></tr>)");
            System.Text.RegularExpressions.MatchCollection Matches = Regex.Matches(ResponseFromServer);

            if (Matches.Count == 0)
            {
                return NonSearch(context);
            }


            foreach (System.Text.RegularExpressions.Match Macth in Matches)
            {
                Item Item = new Item();
                Item.ImageLink = ICO_TorrentFile;

                Regex = new System.Text.RegularExpressions.Regex("(?<=<a href=\").*?(?=\">)");
                Item.Link = TrackerServerRuTor + Regex.Matches(Macth.Value)[1].Value + ";PAGEFILMRUTOR";

                Regex = new System.Text.RegularExpressions.Regex("(?<=\">).*?(?=</a>)");
                Item.Name = Regex.Matches(Macth.Value)[1].Value;

                Regex = new System.Text.RegularExpressions.Regex("(<td align=\"right\">).*?(</td>)");
                System.Text.RegularExpressions.MatchCollection MatchSize = Regex.Matches(Macth.Value);
                string SizeFile = MatchSize[MatchSize.Count - 1].Value;
                SizeFile = "Размер: " + SizeFile;

                Regex = new System.Text.RegularExpressions.Regex("(?<=alt=\"S\" />&nbsp;).*?(?=<)");
                string Seeders = "Seeders: " + Regex.Matches(Macth.Value)[0].Value;


                Regex = new System.Text.RegularExpressions.Regex("(?<=alt=\"L\" /><span class=\"red\">&nbsp;).*?(?=</span>)");
                string Leechers = "Leechers: " + Regex.Matches(Macth.Value)[0].Value;

                Item.Description = "</div><span style=\"color:#3090F0\">" + Item.Name + "</span><p><br>" + SizeFile + "<br><p>" + Seeders + "<br>" + Leechers;

                items.Add(Item);
            }

            Regex = new System.Text.RegularExpressions.Regex("(?<=&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href=\").*?(?=\"><b>След)");
            System.Text.RegularExpressions.MatchCollection MatchNext = Regex.Matches(ResponseFromServer);
            if (MatchNext.Count > 0)
            {
                next_page_url = TrackerServerRuTor + MatchNext[0].Value + ";PAGERUTOR";
            }
            else
            {
                next_page_url = null;
            }

            PlayList.IsIptv = "false";
            return PlayListPlugPar(items, context, next_page_url);
        }

        public PluginApi.Plugins.Playlist GetTopListRuTor(IPluginContext context)
        {
            System.Collections.Generic.List<Item> items = new System.Collections.Generic.List<Item>();
            Item Item = new Item();


            Item.Name = "Поиск";
            Item.Link = "Search_RuTor";
            Item.Type = ItemType.DIRECTORY;
            Item.SearchOn = "Поик на RuTor";
            Item.ImageLink = ICO_Search;
            Item.Description = "<html><font face=\"Arial\" size=\"5\"><b>" + Item.Name + "</font></b><p><img src=\"" + TrackerServerRuTor + LOGO_TrackerRutor + "\" />";
            items.Add(Item);

            Item = new Item();
            Item.Name = "Торренты за последние 24 часа";
            Item.Link = TrackerServerRuTor + "/;PAGERUTOR";
            Item.ImageLink = ICO_Folder;
            Item.Description = "<html><font face=\"Arial\" size=\"5\"><b>" + Item.Name + "</font></b><p><img src=\"" + TrackerServerRuTor + LOGO_TrackerRutor + "\" />";
            items.Add(Item);

            Item = new Item();
            Item.Name = "Зарубежные фильмы";
            Item.Link = TrackerServerRuTor + "/browse/0/1/0/0/;PAGERUTOR";
            Item.ImageLink = ICO_Folder;
            Item.Description = "<html><font face=\"Arial\" size=\"5\"><b>" + Item.Name + "</font></b><p><img src=\"" + TrackerServerRuTor + LOGO_TrackerRutor + "\" />";
            items.Add(Item);

            Item = new Item();
            Item.Name = "Наши фильмы";
            Item.Link = TrackerServerRuTor + "/browse/0/5/0/0/;PAGERUTOR";
            Item.ImageLink = ICO_Folder;
            Item.Description = "<html><font face=\"Arial\" size=\"5\"><b>" + Item.Name + "</font></b><p><img src=\"" + TrackerServerRuTor + LOGO_TrackerRutor + "\" />";
            items.Add(Item);


            Item = new Item();
            Item.Name = "Научно-популярные фильмы";
            Item.Link = TrackerServerRuTor + "/browse/0/12/0/0/;PAGERUTOR";
            Item.ImageLink = ICO_Folder;
            Item.Description = "<html><font face=\"Arial\" size=\"5\"><b>" + Item.Name + "</font></b><p><img src=\"" + TrackerServerRuTor + LOGO_TrackerRutor + "\" />";
            items.Add(Item);


            Item = new Item();
            Item.Name = "Сериалы";
            Item.Link = TrackerServerRuTor + "/browse/0/4/0/0/;PAGERUTOR";
            Item.ImageLink = ICO_Folder;
            Item.Description = "<html><font face=\"Arial\" size=\"5\"><b>" + Item.Name + "</font></b><p><img src=\"" + TrackerServerRuTor + LOGO_TrackerRutor + "\" />";
            items.Add(Item);

            Item = new Item();
            Item.Name = "Телевизор";
            Item.Link = TrackerServerRuTor + "/browse/0/6/0/0/;PAGERUTOR";
            Item.ImageLink = ICO_Folder;
            Item.Description = "<html><font face=\"Arial\" size=\"5\"><b>" + Item.Name + "</font></b><p><img src=\"" + TrackerServerRuTor + LOGO_TrackerRutor + "\" />";
            items.Add(Item);


            Item = new Item();
            Item.Name = "Мультипликация";
            Item.Link = TrackerServerRuTor + "/browse/0/7/0/0/;PAGERUTOR";
            Item.ImageLink = ICO_Folder;
            Item.Description = "<html><font face=\"Arial\" size=\"5\"><b>" + Item.Name + "</font></b><p><img src=\"" + TrackerServerRuTor + LOGO_TrackerRutor + "\" />";
            items.Add(Item);

            Item = new Item();
            Item.Name = "Аниме";
            Item.Link = TrackerServerRuTor + "/browse/0/10/0/0/;PAGERUTOR";
            Item.ImageLink = ICO_Folder;
            Item.Description = "<html><font face=\"Arial\" size=\"5\"><b>" + Item.Name + "</font></b><p><img src=\"" + TrackerServerRuTor + LOGO_TrackerRutor + "\" />";
            items.Add(Item);

            Item = new Item();
            Item.Name = "Музыка";
            Item.Link = TrackerServerRuTor + "/browse/0/2/0/0/;PAGERUTOR";
            Item.ImageLink = ICO_Folder;
            Item.Description = "<html><font face=\"Arial\" size=\"5\"><b>" + Item.Name + "</font></b><p><img src=\"" + TrackerServerRuTor + LOGO_TrackerRutor + "\" />";
            items.Add(Item);

            Item = new Item();
            Item.Name = "Юмор";
            Item.Link = TrackerServerRuTor + "/browse/0/15/0/0/;PAGERUTOR";
            Item.ImageLink = ICO_Folder;
            Item.Description = "<html><font face=\"Arial\" size=\"5\"><b>" + Item.Name + "</font></b><p><img src=\"" + TrackerServerRuTor + LOGO_TrackerRutor + "\" />";
            items.Add(Item);

            Item = new Item();
            Item.Name = "Спорт и Здоровье";
            Item.Link = TrackerServerRuTor + "/browse/0/13/0/0/;PAGERUTOR";
            Item.ImageLink = ICO_Folder;
            Item.Description = "<html><font face=\"Arial\" size=\"5\"><b>" + Item.Name + "</font></b><p><img src=\"" + TrackerServerRuTor + LOGO_TrackerRutor + "\" />";
            items.Add(Item);
            PlayList.IsIptv = "false";
            return PlayListPlugPar(items, context);
        }

        #endregion

        #region NNM Club
        private string CookiesNNM = "phpbb2mysql_4_data=a%3A2%3A%7Bs%3A11%3A%22autologinid%22%3Bs%3A32%3A%2296229c9a3405ae99cce1f3bc0cefce2e%22%3Bs%3A6%3A%22userid%22%3Bs%3A8%3A%2213287549%22%3B%7D";

        public PluginApi.Plugins.Playlist SearchListNNM(IPluginContext context, string search)
        {

            System.Net.HttpWebRequest RequestPost = System.Net.HttpWebRequest.CreateHttp(TrackerServerNNM + "/forum/tracker.php");
            if (ProxyEnablerNNM == true)
            {
                RequestPost.Proxy = new System.Net.WebProxy(ProxyServr, ProxyPort);
            }
            RequestPost.Method = "POST";
            RequestPost.ContentType = "text/html; charset=windows-1251";
            RequestPost.Headers.Add("Cookie", CookiesNNM);
            RequestPost.ContentType = "application/x-www-form-urlencoded";
            System.IO.Stream myStream = RequestPost.GetRequestStream();
            string DataStr = "prev_sd=1&prev_a=1&prev_my=0&prev_n=0&prev_shc=0&prev_shf=0&prev_sha=0&prev_shs=0&prev_shr=0&prev_sht=0&f%5B%5D=724&f%5B%5D=725&f%5B%5D=729&f%5B%5D=731&f%5B%5D=733&f%5B%5D=730&f%5B%5D=732&f%5B%5D=230&f%5B%5D=659&f%5B%5D=658&f%5B%5D=231&f%5B%5D=660&f%5B%5D=661&f%5B%5D=890&f%5B%5D=232&f%5B%5D=734&f%5B%5D=742&f%5B%5D=735&f%5B%5D=738&f%5B%5D=967&f%5B%5D=907&f%5B%5D=739&f%5B%5D=1109&f%5B%5D=736&f%5B%5D=737&f%5B%5D=898&f%5B%5D=935&f%5B%5D=871&f%5B%5D=973&f%5B%5D=960&f%5B%5D=1239&f%5B%5D=740&f%5B%5D=741&f%5B%5D=216&f%5B%5D=270&f%5B%5D=218&f%5B%5D=219&f%5B%5D=954&f%5B%5D=888&f%5B%5D=217&f%5B%5D=266&f%5B%5D=318&f%5B%5D=320&f%5B%5D=677&f%5B%5D=1177&f%5B%5D=319&f%5B%5D=678&f%5B%5D=885&f%5B%5D=908&f%5B%5D=909&f%5B%5D=910&f%5B%5D=911&f%5B%5D=912&f%5B%5D=220&f%5B%5D=221&f%5B%5D=222&f%5B%5D=882&f%5B%5D=889&f%5B%5D=224&f%5B%5D=225&f%5B%5D=226&f%5B%5D=227&f%5B%5D=891&f%5B%5D=682&f%5B%5D=694&f%5B%5D=884&f%5B%5D=1211&f%5B%5D=693&f%5B%5D=913&f%5B%5D=228&f%5B%5D=1150&f%5B%5D=254&f%5B%5D=321&f%5B%5D=255&f%5B%5D=906&f%5B%5D=256&f%5B%5D=257&f%5B%5D=258&f%5B%5D=883&f%5B%5D=955&f%5B%5D=905&f%5B%5D=271&f%5B%5D=1210&f%5B%5D=264&f%5B%5D=265&f%5B%5D=272&f%5B%5D=1262&f%5B%5D=1219&f%5B%5D=1221&f%5B%5D=1220&f%5B%5D=768&f%5B%5D=779&f%5B%5D=778&f%5B%5D=788&f%5B%5D=1288&f%5B%5D=787&f%5B%5D=1196&f%5B%5D=1141&f%5B%5D=777&f%5B%5D=786&f%5B%5D=803&f%5B%5D=776&f%5B%5D=785&f%5B%5D=1265&f%5B%5D=1289&f%5B%5D=774&f%5B%5D=775&f%5B%5D=1242&f%5B%5D=1140&f%5B%5D=782&f%5B%5D=773&f%5B%5D=1142&f%5B%5D=784&f%5B%5D=1195&f%5B%5D=772&f%5B%5D=771&f%5B%5D=783&f%5B%5D=1144&f%5B%5D=804&f%5B%5D=1290&f%5B%5D=770&f%5B%5D=922&f%5B%5D=780&f%5B%5D=781&f%5B%5D=769&f%5B%5D=799&f%5B%5D=800&f%5B%5D=791&f%5B%5D=798&f%5B%5D=797&f%5B%5D=790&f%5B%5D=793&f%5B%5D=794&f%5B%5D=789&f%5B%5D=796&f%5B%5D=792&f%5B%5D=795&f%5B%5D=713&f%5B%5D=706&f%5B%5D=577&f%5B%5D=894&f%5B%5D=578&f%5B%5D=580&f%5B%5D=579&f%5B%5D=953&f%5B%5D=581&f%5B%5D=806&f%5B%5D=714&f%5B%5D=761&f%5B%5D=809&f%5B%5D=924&f%5B%5D=812&f%5B%5D=576&f%5B%5D=590&f%5B%5D=591&f%5B%5D=588&f%5B%5D=823&f%5B%5D=589&f%5B%5D=598&f%5B%5D=652&f%5B%5D=596&f%5B%5D=600&f%5B%5D=819&f%5B%5D=599&f%5B%5D=956&f%5B%5D=959&f%5B%5D=597&f%5B%5D=594&f%5B%5D=593&f%5B%5D=595&f%5B%5D=582&f%5B%5D=587&f%5B%5D=583&f%5B%5D=584&f%5B%5D=586&f%5B%5D=585&f%5B%5D=614&f%5B%5D=603&f%5B%5D=1287&f%5B%5D=1282&f%5B%5D=1206&f%5B%5D=1200&f%5B%5D=1194&f%5B%5D=1062&f%5B%5D=974&f%5B%5D=609&f%5B%5D=1263&f%5B%5D=951&f%5B%5D=975&f%5B%5D=608&f%5B%5D=607&f%5B%5D=606&f%5B%5D=750&f%5B%5D=605&f%5B%5D=604&f%5B%5D=950&f%5B%5D=610&f%5B%5D=613&f%5B%5D=612&f%5B%5D=655&f%5B%5D=653&f%5B%5D=654&f%5B%5D=611&f%5B%5D=656&f%5B%5D=615&f%5B%5D=616&f%5B%5D=617&f%5B%5D=619&f%5B%5D=620&f%5B%5D=623&f%5B%5D=622&f%5B%5D=635&f%5B%5D=621&f%5B%5D=632&f%5B%5D=643&f%5B%5D=624&f%5B%5D=627&f%5B%5D=626&f%5B%5D=636&f%5B%5D=625&f%5B%5D=633&f%5B%5D=644&f%5B%5D=628&f%5B%5D=631&f%5B%5D=630&f%5B%5D=637&f%5B%5D=629&f%5B%5D=634&f%5B%5D=642&f%5B%5D=645&f%5B%5D=639&f%5B%5D=640&f%5B%5D=648&f%5B%5D=638&f%5B%5D=646&f%5B%5D=695&o=10&s=2&tm=-1&a=1&sd=1&ta=-1&sns=-1&sds=-1&nm=" + search + "&pn=&submit=Поиск";
            byte[] DataByte = System.Text.Encoding.GetEncoding(1251).GetBytes(DataStr);
            myStream.Write(DataByte, 0, DataByte.Length);
            myStream.Close();

            System.Net.WebResponse Response = RequestPost.GetResponse();
            System.IO.Stream dataStream = Response.GetResponseStream();
            System.IO.StreamReader reader = new System.IO.StreamReader(dataStream, System.Text.Encoding.GetEncoding(1251));
            string ResponseFromServer = reader.ReadToEnd();


            System.Collections.Generic.List<Item> items = new System.Collections.Generic.List<Item>();
            System.Text.RegularExpressions.Regex Regex = new System.Text.RegularExpressions.Regex("(<tr class=\"prow).*?(</tr>)");
            System.Text.RegularExpressions.MatchCollection Result = Regex.Matches(ResponseFromServer.Replace("\n", "   "));


            if (Result.Count > 0)
            {

                foreach (System.Text.RegularExpressions.Match Match in Result)
                {
                    Regex = new System.Text.RegularExpressions.Regex("(?<=href=\").*?(?=&amp;)");
                    Item Item = new Item();
                    Item.Link = TrackerServerNNM + "/forum/" + Regex.Matches(Match.Value)[0].Value + ";PAGEFILMNNM";
                    Regex = new System.Text.RegularExpressions.Regex("(?<=\"><b>).*?(?=</b>)");
                    Item.Name = Regex.Matches(Match.Value)[0].Value;
                    Item.ImageLink = ICO_TorrentFile;
                    Item.Description = GetDescriptionSearhNNM(Match.Value);
                    items.Add(Item);
                }
            }
            else
            {
                return NonSearch(context);
            }

            PlayList.IsIptv = "false";
            return PlayListPlugPar(items, context);
        }


        public PluginApi.Plugins.Playlist NonSearch(IPluginContext context)
        {
            return NonSearch(context, false);
        }

        //INSTANT C# NOTE: Overloaded method(s) are created above to convert the following method having optional parameters:
        //ORIGINAL LINE: Function NonSearch(context As IPluginContext, Optional ByVal Categor As Boolean = false) As PluginApi.Plugins.Playlist
        public PluginApi.Plugins.Playlist NonSearch(IPluginContext context, bool Categor)
        {
            System.Collections.Generic.List<Item> items = new System.Collections.Generic.List<Item>();
            Item Item = new Item();
            Item.Link = "";
            Item.ImageLink = ICO_Pusto;
            if (Categor == true)
            {
                Item.Name = "<span style=\"color#F68648\">" + " - Здесь ничего нет - " + "</span>";
                Item.Description = "Нет информации для отображения";
            }
            else
            {
                Item.Name = "<span style=\"color#F68648\">" + " - Ничего не найдено - " + "</span>";
                Item.Description = "Поиск не дал результатов";
            }


            items.Add(Item);

            PlayList.IsIptv = "false";
            return PlayListPlugPar(items, context);
        }

        public string GetDescriptionSearhNNM(string HTML)
        {

            string NameFilm = null;
            try
            {
                System.Text.RegularExpressions.Regex Regex = new System.Text.RegularExpressions.Regex("(?<=\"><b>).*?(?=</b>)");
                NameFilm = Regex.Matches(HTML)[0].Value;
            }
            catch (Exception ex)
            {
            }

            string SizeFile = null;
            string DobavlenFile = null;
            try
            {
                System.Text.RegularExpressions.Regex Regex = new System.Text.RegularExpressions.Regex("(?<=</u>).*?(?=</td>)");
                SizeFile = "<p> Размер: <b>" + Regex.Matches(HTML)[0].Value + "</b>";
                DobavlenFile = "<p> Добавлен: <b>" + Regex.Matches(HTML)[1].Value.Replace("<br>", " ") + "</b>";
            }
            catch (Exception ex)
            {
            }

            string Seeders = null;
            try
            {
                System.Text.RegularExpressions.Regex Regex = new System.Text.RegularExpressions.Regex("(?<=class=\"seedmed\">).*?(?=</td>)");
                Seeders = "<p> Seeders: <b> " + Regex.Matches(HTML)[0].Value + "</b>";
            }
            catch (Exception ex)
            {
            }
            string Leechers = null;

            try
            {
                System.Text.RegularExpressions.Regex Regex = new System.Text.RegularExpressions.Regex("(?<=ass=\"leechmed\">).*?(?=</td>)");
                Leechers = "<p> Leechers: <b> " + Regex.Matches(HTML)[0].Value + "</b>";
            }
            catch (Exception ex)
            {
            }

            return "<html><font face=\"Arial\" size=\"5\"><b>" + NameFilm + "</font></b><p><font face=\"Arial Narrow\" size=\"4\">" + SizeFile + DobavlenFile + Seeders + Leechers + "</font></html>";
        }

        public PluginApi.Plugins.Playlist GetTopNNMClubList(IPluginContext context)
        {

            System.Collections.Generic.List<Item> items = new System.Collections.Generic.List<Item>();
            Item Item = new Item();

            Item.Name = "Поиск";
            Item.Link = "Search_NNM";
            Item.Type = ItemType.DIRECTORY;
            Item.SearchOn = "Поиск видео на NNM-Club";
            Item.ImageLink = ICO_Search;

            Item.Description = "<html><font face=\"Arial\" size=\"5\"><b>" + Item.Name + "</font></b><p><img src=\"http://assets.nnm-club.ws/forum/images/logos/10let8.png\" />";


            items.Add(Item);

            Item = new Item();
            Item.Name = "Новинки кино";
            Item.Link = TrackerServerNNM + "/forum/portal.php?c=10;PAGENNM";
            Item.ImageLink = ICO_Folder;
            Item.Description = "<html><font face=\"Arial\" size=\"5\"><b>" + Item.Name + "</font></b><p><img src=\"http://assets.nnm-club.ws/forum/images/logos/10let8.png\" />";

            items.Add(Item);

            Item = new Item();
            Item.Name = "Наше кино";
            Item.Link = TrackerServerNNM + "/forum/portal.php?c=13;PAGENNM";
            Item.ImageLink = ICO_Folder;
            Item.Description = "<html><font face=\"Arial\" size=\"5\"><b>" + Item.Name + "</font></b><p><img src=\"http://assets.nnm-club.ws/forum/images/logos/10let8.png\" />";
            items.Add(Item);

            Item = new Item();
            Item.Name = "Зарубежное кино";
            Item.Link = TrackerServerNNM + "/forum/portal.php?c=6;PAGENNM";
            Item.ImageLink = ICO_Folder;
            Item.Description = "<html><font face=\"Arial\" size=\"5\"><b>" + Item.Name + "</font></b><p><img src=\"http://assets.nnm-club.ws/forum/images/logos/10let8.png\" />";
            items.Add(Item);

            Item = new Item();
            Item.Name = "HD (3D) Кино";
            Item.Link = TrackerServerNNM + "/forum/portal.php?c=11;PAGENNM";
            Item.ImageLink = ICO_Folder;
            Item.Description = "<html><font face=\"Arial\" size=\"5\"><b>" + Item.Name + "</font></b><p><img src=\"http://assets.nnm-club.ws/forum/images/logos/10let8.png\" />";
            items.Add(Item);

            Item = new Item();
            Item.Name = "Артхаус";
            Item.Link = TrackerServerNNM + "/forum/portal.php?c=17;PAGENNM";
            Item.ImageLink = ICO_Folder;
            Item.Description = "<html><font face=\"Arial\" size=\"5\"><b>" + Item.Name + "</font></b><p><img src=\"http://assets.nnm-club.ws/forum/images/logos/10let8.png\" />";
            items.Add(Item);

            Item = new Item();
            Item.Name = "Наши сериалы";
            Item.Link = TrackerServerNNM + "/forum/portal.php?c=4;PAGENNM";
            Item.ImageLink = ICO_Folder;
            Item.Description = "<html><font face=\"Arial\" size=\"5\"><b>" + Item.Name + "</font></b><p><img src=\"http://assets.nnm-club.ws/forum/images/logos/10let8.png\" />";
            items.Add(Item);

            Item = new Item();
            Item.Name = "Зарубежные сериалы";
            Item.Link = TrackerServerNNM + "/forum/portal.php?c=3;PAGENNM";
            Item.ImageLink = ICO_Folder;
            Item.Description = "<html><font face=\"Arial\" size=\"5\"><b>" + Item.Name + "</font></b><p><img src=\"http://assets.nnm-club.ws/forum/images/logos/10let8.png\" />";
            items.Add(Item);

            Item = new Item();
            Item.Name = "Театр, МузВидео, Разное";
            Item.Link = TrackerServerNNM + "/forum/portal.php?c=21;PAGENNM";
            Item.ImageLink = ICO_Folder;
            Item.Description = "<html><font face=\"Arial\" size=\"5\"><b>" + Item.Name + "</font></b><p><img src=\"http://assets.nnm-club.ws/forum/images/logos/10let8.png\" />";
            items.Add(Item);

            Item = new Item();
            Item.Name = "Док. TV-бренды";
            Item.Link = TrackerServerNNM + "/forum/portal.php?c=22;PAGENNM";
            Item.ImageLink = ICO_Folder;
            Item.Description = "<html><font face=\"Arial\" size=\"5\"><b>" + Item.Name + "</font></b><p><img src=\"http://assets.nnm-club.ws/forum/images/logos/10let8.png\" />";
            items.Add(Item);

            Item = new Item();
            Item.Name = "Док. и телепередачи";
            Item.Link = TrackerServerNNM + "/forum/portal.php?c=23;PAGENNM";
            Item.ImageLink = ICO_Folder;
            Item.Description = "<html><font face=\"Arial\" size=\"5\"><b>" + Item.Name + "</font></b><p><img src=\"http://assets.nnm-club.ws/forum/images/logos/10let8.png\" />";
            items.Add(Item);

            Item = new Item();
            Item.Name = "Спорт и Юмор";
            Item.Link = TrackerServerNNM + "/forum/portal.php?c=24;PAGENNM";
            Item.ImageLink = ICO_Folder;
            Item.Description = "<html><font face=\"Arial\" size=\"5\"><b>" + Item.Name + "</font></b><p><img src=\"http://assets.nnm-club.ws/forum/images/logos/10let8.png\" />";
            items.Add(Item);

            Item = new Item();
            Item.Name = "Аниме и Манга";
            Item.Link = TrackerServerNNM + "/forum/portal.php?c=1;PAGENNM";
            Item.ImageLink = ICO_Folder;
            Item.Description = "<html><font face=\"Arial\" size=\"5\"><b>" + Item.Name + "</font></b><p><img src=\"http://assets.nnm-club.ws/forum/images/logos/10let8.png\" />";
            items.Add(Item);
            PlayList.IsIptv = "false";
            return PlayListPlugPar(items, context);
        }

        public PluginApi.Plugins.Playlist GetPAGENNM(IPluginContext context, string URL)
        {

            System.Collections.Generic.List<Item> items = new System.Collections.Generic.List<Item>();

            try
            {
                System.Net.HttpWebRequest RequestGet = System.Net.HttpWebRequest.CreateHttp(URL);
                if (ProxyEnablerNNM == true)
                {
                    RequestGet.Proxy = new System.Net.WebProxy(ProxyServr, ProxyPort);
                }
                RequestGet.Method = "GET";
                RequestGet.ContentType = "text/html; charset=windows-1251";
                RequestGet.Headers.Add("Cookie", CookiesNNM);

                System.Net.WebResponse Response2 = RequestGet.GetResponse();
                System.IO.Stream dataStream = Response2.GetResponseStream();
                System.IO.StreamReader reader = new System.IO.StreamReader(dataStream, System.Text.Encoding.GetEncoding(1251));
                string responseFromServer = reader.ReadToEnd();

                System.Text.RegularExpressions.Regex Regex = new System.Text.RegularExpressions.Regex("(<td class=\"pcatHead\"><img class=\"picon\").*?(\" /></span>)");


                foreach (System.Text.RegularExpressions.Match MAtch in Regex.Matches(responseFromServer.Replace("\n", "   ")))
                {
                    Regex = new System.Text.RegularExpressions.Regex("(?<=title=\").*?(?=\">)");
                    Item Item = new Item();
                    Item.Name = Regex.Matches(MAtch.Value)[1].Value;

                    Regex = new System.Text.RegularExpressions.Regex("(?<=<var class=\"portalImg\" title=\").*?(?=\">)");
                    Item.ImageLink = Regex.Matches(MAtch.Value)[0].Value;

                    Item.ImageLink = "http://" + IPAdress + ":8027/proxym3u8B" + Base64Encode(Item.ImageLink + "OPT:ContentType--image/jpegOPEND:/") + "/";

                    Regex = new System.Text.RegularExpressions.Regex("(?<=<a class=\"pgenmed\" href=\").*?(?=&)");
                    Item.Link = TrackerServerNNM + "/forum/" + Regex.Matches(MAtch.Value)[0].Value + ";PAGEFILMNNM";

                    Regex = new System.Text.RegularExpressions.Regex("(?<=<a class=\"pgenmed\" href=\").*?(?=&)");
                    Item.Description = FormatDescriptionNNM(MAtch.Value, Item.ImageLink);


                    items.Add(Item);
                }

                Regex = new System.Text.RegularExpressions.Regex("(?<=&nbsp;&nbsp;<a href=\").*?(?=sid=)");
                System.Text.RegularExpressions.MatchCollection Rzult = Regex.Matches(responseFromServer);

                next_page_url = TrackerServerNNM + "/forum/" + Rzult[Rzult.Count - 1].Value.Replace("amp;", "") + ";PAGENNM";

            }
            catch (Exception ex)
            {
                Item Item = new Item();
                Item.Name = "ERROR";
                Item.ImageLink = ICO_Error;
                Item.Description = ex.Message;
                Item.Link = "";
                items.Add(Item);
            }

            PlayList.IsIptv = "false";
            return PlayListPlugPar(items, context, next_page_url);

        }

        public string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public PluginApi.Plugins.Playlist GetTorrentPAGENNM(IPluginContext context, string URL)
        {
            System.Net.HttpWebRequest RequestGet = System.Net.HttpWebRequest.CreateHttp(URL);
            if (ProxyEnablerNNM == true)
            {
                RequestGet.Proxy = new System.Net.WebProxy(ProxyServr, ProxyPort);
            }
            RequestGet.Method = "GET";
            RequestGet.Headers.Add("Cookie", CookiesNNM);

            System.Net.WebResponse Response = RequestGet.GetResponse();
            System.IO.Stream DataStream = Response.GetResponseStream();
            System.IO.StreamReader Reader = new System.IO.StreamReader(DataStream, System.Text.Encoding.GetEncoding(1251));
            string ResponseFromServer = Reader.ReadToEnd();
            Reader.Close();
            DataStream.Close();
            Response.Close();


            string TorrentPath = null;
            try
            {
                System.Text.RegularExpressions.Regex Regex = new System.Text.RegularExpressions.Regex("(?<=<span class=\"genmed\"><b><a href=\").*?(?=&amp;)");
                TorrentPath = TrackerServerNNM + "/forum/" + Regex.Matches(ResponseFromServer)[0].Value;
            }
            catch (Exception ex)
            {
                TorrentPath = ex.Message;
            }



            string Title = null;
            try
            {
                System.Text.RegularExpressions.Regex Regex = new System.Text.RegularExpressions.Regex("(?<=<span style=\"font-weight: bold\">).*?(?=</span>)");
                Title = Regex.Matches(ResponseFromServer)[0].Value;
            }
            catch (Exception ex)
            {
                Title = ex.Message;
            }

            System.Net.HttpWebRequest RequestTorrent = System.Net.HttpWebRequest.CreateHttp(TorrentPath);
            if (ProxyEnablerNNM == true)
            {
                RequestTorrent.Proxy = new System.Net.WebProxy(ProxyServr, ProxyPort);
            }
            RequestTorrent.Method = "GET";
            RequestTorrent.Headers.Add("Cookie", CookiesNNM);

            Response = RequestTorrent.GetResponse();
            DataStream = Response.GetResponseStream();
            Reader = new System.IO.StreamReader(DataStream, System.Text.Encoding.GetEncoding(1251));
            string FileTorrent = Reader.ReadToEnd();
            System.IO.File.WriteAllText(System.IO.Path.GetTempPath() + "TorrentTemp.torrent", FileTorrent, System.Text.Encoding.GetEncoding(1251));
            Reader.Close();
            DataStream.Close();
            Response.Close();


            System.Collections.Generic.List<Item> items = new System.Collections.Generic.List<Item>();



            try
            {
                string Description = FormatDescriptionFileNNM(ResponseFromServer);
                TorrentPlayList[] PlayListtoTorrent = GetFileList(System.IO.Path.GetTempPath() + "TorrentTemp.torrent");

                foreach (TorrentPlayList PlayListItem in PlayListtoTorrent)
                {
                    Item Item = new Item();
                    Item.Name = PlayListItem.Name;
                    Item.ImageLink = PlayListItem.ImageLink;
                    Item.Link = PlayListItem.Link;
                    Item.Type = ItemType.FILE;
                    Item.Description = Description;
                    items.Add(Item);
                }

            }
            catch (Exception ex)
            {
                Item Item = new Item();
                Item.Name = "ERROR";
                Item.Link = "";
                Item.Type = ItemType.FILE;
                Item.Description = ex.Message;
                Item.ImageLink = ICO_Error;
                items.Add(Item);
            }
            PlayList.IsIptv = "false";
            return PlayListPlugPar(items, context);
        }

        public string FormatDescriptionFileNNM(string HTML)
        {
            HTML = HTML.Replace("\n", "   ");

            string Title = null;
            System.Text.RegularExpressions.Regex Regex = new System.Text.RegularExpressions.Regex("(<span style=\"text-align:).*?(</span>)");
            try
            {
                Title = Regex.Matches(HTML)[0].Value;
            }
            catch (Exception ex)
            {
                Title = ex.Message;
            }


            string SidsPirs = null;
            try
            {
                Regex = new System.Text.RegularExpressions.Regex("(<table cellspacing=\"0\").*?(</table>)");
                SidsPirs = Regex.Matches(HTML)[0].Value;
            }
            catch (Exception ex)
            {
                SidsPirs = ex.Message;
            }


            string ImagePath = null;
            try
            {
                Regex = new System.Text.RegularExpressions.Regex("(?<=<var class=\"postImg postImgAligned img-right\" title=\").*?(?=\">)");
                ImagePath = Regex.Matches(HTML)[0].Value;
                ImagePath = "http://" + IPAdress + ":8027/proxym3u8B" + Base64Encode(ImagePath + "OPT:ContentType--image/jpegOPEND:/") + "/";

            }
            catch (Exception ex)
            {

            }


            string InfoFile = null;
            try
            {
                Regex = new System.Text.RegularExpressions.Regex("(<div class=\"kpi\">).*(?=<div class=\"spoiler-wrap\">)");
                InfoFile = Regex.Matches(HTML)[0].Value;
            }
            catch (Exception e)
            {
                try
                {
                    Regex = new System.Text.RegularExpressions.Regex("(<br /><br /><span style=\"font-weight: bold\">).*?(<br />)");

                    System.Text.RegularExpressions.MatchCollection Match = Regex.Matches(HTML);
                    for (int I = 1; I < Match.Count; ++I)
                    {
                        InfoFile = InfoFile + Match[I].Value;
                    }

                }
                catch (Exception ex)
                {
                    InfoFile = ex.Message;
                }

            }
            string Opisanie = null;

            try
            {
                Regex = new System.Text.RegularExpressions.Regex("(<span style=\"font-weight: bold\">Описание:</span><br />).*?(?=<div)");
                Opisanie = Regex.Matches(HTML)[0].Value;
            }
            catch (Exception ex)
            {
                Opisanie = ex.Message;
            }




            SidsPirs = replacetags(SidsPirs);
            InfoFile = replacetags(InfoFile);
            Title = replacetags(Title);
            Opisanie = replacetags(Opisanie);

            return "<div id=\"poster\" style=\"float:left;padding:4px;        background-color:#EEEEEE;margin:0px 13px 1px 0px;\">" + "<img src=\"" + ImagePath + "\" style=\"width:180px;float:left;\" /></div><span style=\"color:#3090F0\">" + Title + "</span><br>" + SidsPirs + "<br>" + Opisanie + "<span style=\"color:#3090F0\">Информация</span><br>" + InfoFile;
        }

        public string replacetags(string s)
        {
            try
            {
                System.Text.RegularExpressions.Regex rgx = new System.Text.RegularExpressions.Regex("<[^b].*?>");
                s = rgx.Replace(s, "").Replace("<b>", "");
                return s;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public string FormatDescriptionNNM(string HTML, string ImagePath)
        {
            HTML = HTML.Replace("\n", "   ");

            string Title = null;
            try
            {
                System.Text.RegularExpressions.Regex Regex = new System.Text.RegularExpressions.Regex("(?<=title=\").*?(?=\")");
                Title = Regex.Matches(HTML)[1].Value;
            }
            catch (Exception ex)
            {
                Title = ex.Message;
            }


            string InfoFile = null;
            try
            {
                System.Text.RegularExpressions.Regex Regex = new System.Text.RegularExpressions.Regex("(<img class=\"tit-b pims\").*(?=<span id=)");
                InfoFile = Regex.Matches(HTML)[0].Value;
            }
            catch (Exception ex)
            {
                InfoFile = ex.Message;
            }


            string InfoFilms = null;
            try
            {
                System.Text.RegularExpressions.Regex Regex = new System.Text.RegularExpressions.Regex("(</var></a>).*?(<br />)");
                InfoFilms = Regex.Matches(HTML)[0].Value;
            }
            catch (Exception ex)
            {
                InfoFilms = ex.Message;
            }

            string InfoPro = null;
            try
            {
                System.Text.RegularExpressions.Regex Regex = new System.Text.RegularExpressions.Regex("(<br /><b>).*(</span></td> )");
                InfoPro = Regex.Matches(HTML)[0].Value;
            }
            catch (Exception ex)
            {
                InfoPro = ex.Message;
            }
            return "<div id=\"poster\" style=\"float:left;padding:4px;        background-color:#EEEEEE;margin:0px 13px 1px 0px;\">" + "<img src=\"" + ImagePath + "\" style=\"width:180px;float:left;\" /></div><span style=\"color:#3090F0\">" + Title + "</span><br>" + InfoFile + InfoPro + "<br><span style=\"color:#3090F0\">Описание: </span>" + InfoFilms;
        }

        #endregion

        #region TorrentTV

        public PluginApi.Plugins.Playlist GetTorrentTV(IPluginContext context)
        {
            var items = new System.Collections.Generic.List<Item>();
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

            Item = new Item();
            Item.Type = ItemType.DIRECTORY;
            Item.Name = "ЭРОТИКА 18+";
            Item.Link = "porn";
            Item.ImageLink = "http://torrent-tv.ru/images/all_channels.png";
            items.Add(Item);

            //Item = New Item
            //With Item
            //    .Type = ItemType.DIRECTORY
            //    .Name = "ВСЕ КАНАЛЫ"
            //    .Link = "all"
            //    .ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            //End With
            //items.Add(Item)
            PlayList.IsIptv = "false";
            return PlayListPlugPar(items, context);
        }

        public PluginApi.Plugins.Playlist LastModifiedPlayList(string NamePlayList, IPluginContext context)
        {
            PlayList.IsIptv = "true";
            string PathFileUpdateTime = System.IO.Path.GetTempPath() + NamePlayList + ".UpdateTime.tmp";
            string PathFilePlayList = System.IO.Path.GetTempPath() + NamePlayList + ".PlayList.m3u8";

            System.Net.HttpWebRequest request = System.Net.HttpWebRequest.CreateHttp("http://super-pomoyka.us.to/trash/ttv-list/ttv." + NamePlayList + ".iproxy.m3u?ip=" + IPAdress + ":" + PortAce);
            request.Method = "HEAD";
            request.ContentType = "text/html";
            request.KeepAlive = true;
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36";
            request.Host = "super-pomoyka.us.to";
            System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)(request.GetResponse());
            var responHeader = response.GetResponseHeader("Last-Modified");
            response.Close();

            System.Net.WebClient WC = new System.Net.WebClient();
            WC.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36");
            WC.Encoding = System.Text.Encoding.UTF8;

            System.Collections.Generic.List<Item> items = new System.Collections.Generic.List<Item>();
            Item Item = new Item();

            if ((System.IO.File.Exists(PathFileUpdateTime) && System.IO.File.Exists(PathFilePlayList)) == false)
            {
                UpdatePlayList(NamePlayList, PathFilePlayList, PathFileUpdateTime, responHeader);
                Item.Type = ItemType.DIRECTORY;
                Item.GetInfo = "http://" + IPAdress + ":" + PortRemoteFork + "/treeview?pluginacetorrentplaycs%5c.xml&host=" + IPAdress + "%3a8027&pluginPath=getinfo&ID=" + WC.DownloadString(PathFilePlayList);
                // Item.GetInfo = WC.DownloadString(PathFilePlayList)
                items.Add(Item);
                return PlayListPlugPar(items, context);
            }

            if (responHeader != System.IO.File.ReadAllText(PathFileUpdateTime))
            {
                UpdatePlayList(NamePlayList, PathFilePlayList, PathFileUpdateTime, responHeader);
                Item.Type = ItemType.DIRECTORY;
                Item.GetInfo = "http://" + IPAdress + ":" + PortRemoteFork + "/treeview?pluginacetorrentplaycs%5c.xml&host=" + IPAdress + "%3a8027&pluginPath=getinfo&ID=" + WC.DownloadString(PathFilePlayList);
                // Item.GetInfo = WC.DownloadString(PathFilePlayList)
                items.Add(Item);
                return PlayListPlugPar(items, context);
            }

            Item.Type = ItemType.DIRECTORY;
            Item.GetInfo = "http://" + IPAdress + ":" + PortRemoteFork + "/treeview?pluginacetorrentplaycs%5c.xml&host=" + IPAdress + "%3a8027&pluginPath=getinfo&ID=" + WC.DownloadString(PathFilePlayList);
            // Item.GetInfo = WC.DownloadString(PathFilePlayList)
            items.Add(Item);

            return PlayListPlugPar(items, context);

        }

        public void UpdatePlayList(string NamePlayList, string PathFilePlayList, string PathFileUpdateTime, string LastModified)
        {
            System.IO.File.WriteAllText(PathFileUpdateTime, LastModified);
            System.Net.WebClient WC = new System.Net.WebClient();
            WC.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36");
            WC.Encoding = System.Text.Encoding.UTF8;

            string PlayList = WC.DownloadString("http://super-pomoyka.us.to/trash/ttv-list/ttv." + NamePlayList + ".iproxy.m3u?ip=" + IPAdress + ":" + PortAce);
            System.IO.File.WriteAllText(PathFilePlayList, PlayList.Replace("(Эротика)", "(Эротика 18+)"));
            WC.DownloadFile("http://super-pomoyka.us.to/trash/ttv-list/MyTraf.php", System.IO.Path.GetTempPath() + "MyTraf.tmp");
            WC.Dispose();
        }

        #endregion

        #region AceTorrent
        private string PortAce = "6878";
        private bool AceProxEnabl;
        public struct TorrentPlayList
        {
            public string IDX;
            public string Name;
            public string Link;
            public string Description;
            public string ImageLink;
        }


        public string GetID(string PathTorrent)
        {
            System.Net.WebClient WC = new System.Net.WebClient();
            WC.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0");
            WC.Encoding = System.Text.Encoding.UTF8;
            byte[] FileTorrent = WC.DownloadData(PathTorrent);

            string FileTorrentString = System.Convert.ToBase64String(FileTorrent);
            FileTorrent = System.Text.Encoding.Default.GetBytes(FileTorrentString);

            System.Net.HttpWebRequest request = System.Net.HttpWebRequest.CreateHttp("http://api.torrentstream.net/upload/raw");
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
            string ID = responseSplit[3];
            return ID;
        }

        public TorrentPlayList[] GetFileList(string PathTorrent)
        {

            System.Net.WebClient WC = new System.Net.WebClient();
            WC.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36");
            WC.Encoding = System.Text.Encoding.UTF8;

            string ID = GetID(PathTorrent);
            TorrentPlayList[] PlayListTorrent = null;
            string AceMadiaInfo = null;

            switch (FunctionsGetTorrentPlayList)
            {
                case "GetFileListJSON":
                    {
                      
                        string[] CodeZnaki = { "\\U0430", "\\U0431", "\\U0432", "\\U0433", "\\U0434", "\\U0435", "\\U0451", "\\U0436", "\\U0437", "\\U0438", "\\U0439", "\\U043A", "\\U043B", "\\U043C", "\\U043D", "\\U043E", "\\U043F", "\\U0440", "\\U0441", "\\U0442", "\\U0443", "\\U0444", "\\U0445", "\\U0446", "\\U0447", "\\U0448", "\\U0449", "\\U044A", "\\U044B", "\\U044C", "\\U044D", "\\U044E", "\\U044F", "\\U0410", "\\U0411", "\\U0412", "\\U0413", "\\U0414", "\\U0415", "\\U0401", "\\U0416", "\\U0417", "\\U0418", "\\U0419", "\\U041A", "\\U041B", "\\U041C", "\\U041D", "\\U041E", "\\U041F", "\\U0420", "\\U0421", "\\U0422", "\\U0423", "\\U0424", "\\U0425", "\\U0426", "\\U0427", "\\U0428", "\\U0429", "\\U042A", "\\U042B", "\\U042C", "\\U042D", "\\U042E", "\\U042F", "\\U00AB", "\\U00BB", "U2116" };
                        string[] DecodeZnaki = { "а", "б", "в", "г", "д", "е", "ё", "ж", "з", "и", "й", "к", "л", "м", "н", "о", "п", "р", "с", "т", "у", "ф", "х", "ц", "ч", "ш", "щ", "ъ", "ы", "ь", "э", "ю", "я", "А", "Б", "В", "Г", "Д", "Е", "Ё", "Ж", "З", "И", "Й", "К", "Л", "М", "Н", "О", "П", "Р", "С", "Т", "У", "Ф", "Х", "Ц", "Ч", "Ш", "Щ", "Ъ", "Ы", "Ь", "Э", "Ю", "Я", "«", "»", "№" };

                        AceMadiaInfo = WC.DownloadString("http://" + IPAdress + ":" + PortAce + "/server/api?method=get_media_files&content_id=" + ID);
                        for (int I = 0; I <= 68; ++I)
                        {
                            AceMadiaInfo = AceMadiaInfo.Replace(CodeZnaki[I].ToLower(), DecodeZnaki[I]);
                        }
                        WC.Dispose();

                        string PlayListJson = AceMadiaInfo;
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

                        PlayListTorrent = new TorrentPlayList[(ListSplit.Length / 2) - 2 + 1];

                        int N = 0;
                        for (int I = 1; I <= ListSplit.Length - 2; ++I)
                        {
                            PlayListTorrent[N].IDX = ListSplit[I];
                            PlayListTorrent[N].Name = ListSplit[I + 1];
                            PlayListTorrent[N].Link = "http://" + IPAdress + ":" + PortAce + "/ace/getstream?id=" + ID + "&_idx=" + PlayListTorrent[N].IDX;
                            PlayListTorrent[N].ImageLink = ICO_VideoFile;
                            ++I;
                            ++N;
                        }


                        break;
                    }
                case "GetFileListM3U":
                    {
                        AceMadiaInfo = WC.DownloadString("http://" + IPAdress + ":" + PortAce + "/ace/manifest.m3u8?id=" + ID + "&format=json&use_api_events=1&use_stop_notifications=1");
                        if (AceMadiaInfo.StartsWith("{\"response\": {\"event_url\": \"") == true)
                        {
                            goto case "GetFileListJSON";
                        }
                        if (AceMadiaInfo.StartsWith("{\"response\": null, \"error\": \"") == true)
                        {
                            PlayListTorrent = new TorrentPlayList[1];
                            PlayListTorrent[0].Name = "ОШИБКА: " + new System.Text.RegularExpressions.Regex("(?<={\"response\": null, \"error\": \").*?(?=\")").Matches(AceMadiaInfo)[0].Value;
                            PlayListTorrent[0].ImageLink = ICO_Error;
                            return PlayListTorrent;
                        }

                        //"Получение потока в формате HLS
                        AceMadiaInfo = WC.DownloadString("http://" + IPAdress + ":" + PortAce + "/ace/manifest.m3u8?id=" + ID);
                        System.Text.RegularExpressions.Regex Regex = new System.Text.RegularExpressions.Regex("(?<=EXTINF:-1,).*(.*)");
                        System.Text.RegularExpressions.MatchCollection Itog = Regex.Matches(AceMadiaInfo);

                        PlayListTorrent = new TorrentPlayList[Itog.Count];
                        int N = 0;
                        foreach (System.Text.RegularExpressions.Match Match in Itog)
                        {
                            PlayListTorrent[N].Name = Match.Value;
                            PlayListTorrent[N].ImageLink = ICO_VideoFile;
                            ++N;
                        }

                        N = 0;
                        Regex = new System.Text.RegularExpressions.Regex("(http:).*(?=.*?)");
                        Itog = Regex.Matches(AceMadiaInfo);
                        foreach (System.Text.RegularExpressions.Match Match in Itog)
                        {
                            PlayListTorrent[N].Link = Match.Value;
                            ++N;
                        }
                        break;
                    }
            }


            return PlayListTorrent;
        }


        #endregion

    }

}
