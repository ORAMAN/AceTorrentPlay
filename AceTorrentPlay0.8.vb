Option Explicit On
Imports System.Linq
Imports PluginApi.Plugins
Imports RemoteFork.Plugins
Imports System.Collections.Generic
Imports System.Collections.Specialized
Imports System.Drawing
Imports Microsoft.VisualBasic
Imports System


Namespace RemoteFork.Plugins
    <PluginAttribute(Id:="acetorrentplay", Version:="0.84", Author:="ORAMAN", Name:="AceTorrentPlay", Description:="Воспроизведение файлов TORRENT через меда-сервер Ace Stream", ImageLink:="http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597291utorrent2.png")>
    Public Class AceTorrentPlay
        Implements IPlugin

        Dim IPAdress As String
        Dim PortRemoteFork As String = "8027"
        Dim PLUGIN_PATH As String = "pluginPath"
        Dim PlayList As New PluginApi.Plugins.Playlist
        Dim next_page_url As String
        Dim IDPlagin As String = "acetorrentplay"


#Region "Настройки"

#Region "Иконки"
        Dim ICO_FolderVideo As String = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597246foldervideos.png"
        Dim ICO_FolderVideo2 As String = "http://s1.iconbird.com/ico/1112/Concave/w256h2561352644144Videos.png"
        Dim ICO_Folder As String = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597246folder.png"
        Dim ICO_Folder2 As String = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597240explorer.png"
        Dim ICO_Settings As String = "http://s1.iconbird.com/ico/2013/11/504/w128h1281385326483gear.png"
        Dim ICO_SettingsFolder As String = "http://s1.iconbird.com/ico/2013/11/504/w128h1281385326516tools.png"
        Dim ICO_SettingsParam As String = "http://s1.iconbird.com/ico/2013/11/504/w128h1281385326449check.png"
        Dim ICO_SettingsReset As String = "http://s1.iconbird.com/ico/2013/11/504/w128h1281385326539check.png"
        Dim ICO_OtherFile As String = "http://s1.iconbird.com/ico/2013/6/364/w256h2561372348486helpfile256.png"
        Dim ICO_VideoFile As String = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597291videofile.png"
        Dim ICO_MusicFile As String = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597283musicfile.png"
        Dim ICO_TorrentFile As String = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597291utorrent2.png"
        Dim ICO_TorrentFile2 As String = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597291utorrent.png"
        Dim ICO_ImageFile As String = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597278imagefile.png"
        Dim ICO_M3UFile As String = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597278librarymusic.png"
        Dim ICO_NNMClub As String = "http://s1.iconbird.com/ico/0912/MorphoButterfly/w128h1281348669898RhetenorMorpho.png"
        Dim ICO_Search As String = "http://s1.iconbird.com/ico/0612/MustHave/w256h2561339195991Search256x256.png"
        Dim ICO_Search2 As String = "http://s1.iconbird.com/ico/0912/MetroUIDock/w512h5121347464996Search.png"
        Dim ICO_Error As String = "http://s1.iconbird.com/ico/0912/ToolbarIcons/w256h2561346685474SymbolError.png"
        Dim ICO_Error2 As String = "http://errorfix48.ru/uploads/posts/2014-09/1409846068_400px-warning_icon.png"
        Dim ICO_Save As String = "http://s1.iconbird.com/ico/2013/6/355/w128h1281372334742check.png"
        Dim ICO_Delete As String = "http://s1.iconbird.com/ico/2013/6/355/w128h1281372334742delete.png"
        Dim ICO_Pusto As String = "https://avatanplus.com/files/resources/mid/5788db3ecaa49155ee986d6e.png"
        Dim ICO_Login As String = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597246portal2.png"
        Dim ICO_Password As String = "http://s1.iconbird.com/ico/0612/GooglePlusInterfaceIcons/w128h1281338911371password.png"
        Dim ICO_LoginKey As String = "http://s1.iconbird.com/ico/0912/ToolbarIcons/w256h2561346685464Login.png"

        Dim LOGO_RuTr As String = "https://rutrk.org/logo/logo.png"
        Dim LOGO_TrackerRutor As String = "http://mega-tor.org/s/logo.jpg"
        Dim LOGO_NoNaMeClub As String = "http://assets.nnm-club.ws/forum/images/logos/10let8.png"
#End Region

#Region "Параметры"
        Dim ProxyServr As String = "proxy.antizapret.prostovpn.org"
        Dim ProxyPort As Integer = 3128

        Dim FunctionsGetTorrentPlayList As String
        Dim ProxyEnablerNNM As Boolean
        Dim TrackerServerNNM As String
#End Region


        Sub Load_Settings()

            Dim TempStr As String = CStr(Microsoft.Win32.Registry.GetValue("HKEY_CURRENT_USER\Software\RemoteFork\Plugins\AceTorrentPlay\", "FunctionsGetTorrentPlayList", ""))
            If TempStr = "" Then
                FunctionsGetTorrentPlayList = "GetFileListM3U"
                Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\Software\RemoteFork\Plugins\AceTorrentPlay\", "FunctionsGetTorrentPlayList", FunctionsGetTorrentPlayList)
            Else
                FunctionsGetTorrentPlayList = TempStr
            End If

            TempStr = CStr(Microsoft.Win32.Registry.GetValue("HKEY_CURRENT_USER\Software\RemoteFork\Plugins\AceTorrentPlay\", "ProxyEnablerNNM", ""))
            If TempStr = "" Then
                ProxyEnablerNNM = True
                Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\Software\RemoteFork\Plugins\AceTorrentPlay\", "ProxyEnablerNNM", ProxyEnablerNNM)
            Else
                ProxyEnablerNNM = CBool(TempStr)
            End If

            TempStr = CStr(Microsoft.Win32.Registry.GetValue("HKEY_CURRENT_USER\Software\RemoteFork\Plugins\AceTorrentPlay\", "TrackerServerNNM", ""))
            If TempStr = "" Then
                TrackerServerNNM = "http://nnmclub.to"
                Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\Software\RemoteFork\Plugins\AceTorrentPlay\", "TrackerServerNNM", TrackerServerNNM)
            Else
                TrackerServerNNM = TempStr
            End If

        End Sub
        Sub Save_Settings()
            Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\Software\RemoteFork\Plugins\AceTorrentPlay\", "FunctionsGetTorrentPlayList", FunctionsGetTorrentPlayList)
            Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\Software\RemoteFork\Plugins\AceTorrentPlay\", "ProxyEnablerNNM", ProxyEnablerNNM)
            Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\Software\RemoteFork\Plugins\AceTorrentPlay\", "TrackerServerNNM", TrackerServerNNM)
        End Sub

        Function GetListSettingsNNM(context As IPluginContext, Optional ByVal ParametrSettings As String = "") As PluginApi.Plugins.Playlist


            Select Case ParametrSettings
                Case "ProxyNNM"
                    ProxyEnablerNNM = Not ProxyEnablerNNM
                Case "TrackerServerNNM"
                    Select Case TrackerServerNNM
                        Case "http://nnmclub.to"
                            TrackerServerNNM = "https://nnm-club.name"
                        Case "https://nnm-club.name"
                            TrackerServerNNM = "http://nnm-club.me"
                        Case "http://nnm-club.me"
                            TrackerServerNNM = "http://nnmclub.to"
                    End Select
                    '   If TrackerServerNNM = "http://nnmclub.to" Then TrackerServerNNM = "https://nnm-club.name" Else TrackerServerNNM = "http://nnmclub.to"
            End Select
            Save_Settings()

            Dim Items As New System.Collections.Generic.List(Of Item)
            Dim Item_Top, Item_ProxEnabl, Item_TrackerServerNNM As New Item

            With Item_Top
                .Name = " - N N M - C l u b -"
                .ImageLink = ICO_Pusto
                .Description = "Доступ к ресурсу " & TrackerServerNNM & "<p><b> Прокси: " & ProxyEnablerNNM
                .Type = ItemType.FILE
                Items.Add(Item_Top)
            End With

            With Item_ProxEnabl
                .Name = "Вкл/Выкл proxy"
                .Link = "ProxyNNM;SETTINGS_NNM"
                .ImageLink = ICO_SettingsParam
                If ProxyEnablerNNM = True Then .Description = "Доступ к ресурсу " & TrackerServerNNM & " через прокси сервер включён." Else .Description = "Доступ к ресурсу " & TrackerServerNNM & " через прокси отключён."
                Items.Add(Item_ProxEnabl)
            End With

            With Item_TrackerServerNNM
                .Name = "Адрес сервера NNM-Club"
                .Link = "TrackerServerNNM;SETTINGS_NNM"
                .ImageLink = ICO_SettingsParam
                .Description = "Доступ к ресурсу осуществляется через " & TrackerServerNNM
                Items.Add(Item_TrackerServerNNM)
            End With

            PlayList.IsIptv = "false"
            Return PlayListPlugPar(Items, context)
        End Function

        Function GetListSettings(context As IPluginContext, Optional ByVal ParametrSettings As String = "") As PluginApi.Plugins.Playlist

            Select Case ParametrSettings

                Case "FunctionsGetTorrentPlayList"
                    Select Case FunctionsGetTorrentPlayList
                        Case "GetFileListJSON"
                            FunctionsGetTorrentPlayList = "GetFileListM3U"
                        Case "GetFileListM3U"
                            FunctionsGetTorrentPlayList = "GetFileListJSON"
                    End Select
                    ParametrSettings = ""
                Case "NNM-Club_Settings"
                Case "DeleteSettings"
                    Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree("Software\RemoteFork\Plugins\AceTorrentPlay\", False)
                    Load_Settings()
                    ParametrSettings = ""
            End Select
            Save_Settings()

            Dim Items As New System.Collections.Generic.List(Of Item)
            Dim Item_Top, Item_FGPL, Item_NNM, Item_DelSettings As New Item
            With Item_Top
                .Name = "- Н А С Т Р О Й К И -"
                .Link = ""
                .ImageLink = ICO_Pusto
                .Type = ItemType.FILE
                Items.Add(Item_Top)
            End With


            With Item_NNM
                .Name = "Настройка доступа к NNM-Club"
                .Link = ";SETTINGS_NNM"
                .ImageLink = ICO_SettingsFolder
                Items.Add(Item_NNM)
            End With

            With Item_FGPL
                .Name = "Обработка содержимого torrent файла"
                .Link = "FunctionsGetTorrentPlayList;SETTINGS"
                .ImageLink = ICO_SettingsParam
                .Description = "<html>Выбор метода для запроса содержимого торрент файла <p> Текущий метод: <b>" & FunctionsGetTorrentPlayList & "</b></p><p>Измените параметр если при открытии торрентов содержащих более одного файла происходит ошибка.</html>"
                Items.Add(Item_FGPL)
            End With


            With Item_DelSettings
                .Name = "Настройки по умолчанию"
                .Link = "DeleteSettings;SETTINGS"
                .ImageLink = ICO_SettingsReset
                Items.Add(Item_DelSettings)
            End With


            PlayList.IsIptv = "false"
            Return PlayListPlugPar(Items, context)
        End Function
#End Region


        Public Function GetList(context As IPluginContext) As PluginApi.Plugins.Playlist Implements IPlugin.GetList
            Load_Settings()
            IPAdress = context.GetRequestParams.Get("host").Split(":")(0)

            PlayList.source = Nothing
            Dim path = context.GetRequestParams().Get(PLUGIN_PATH)
            path = (If((path Is Nothing), "plugin", "plugin;" & path))

            If context.GetRequestParams.Get("search") <> Nothing Then
                Select Case path
                    Case "plugin;searchtvtoace"
                        Return GetPageSearchStreamTV(context, context.GetRequestParams("search"))
                    Case "plugin;Search_NNM"
                        Return SearchListNNM(context, context.GetRequestParams()("search"))
                    Case "plugin;Search_RuTor"
                        Return GetPAGERUTOR(context, TrackerServerRuTor & "/search/0/0/100/2/" & context.GetRequestParams()("search"))
                    Case "plugin;Search_rutracker"
                        Return SearchListRuTr(context, context.GetRequestParams()("search"))
                    Case "plugin;RuTr_Login"
                        Login = context.GetRequestParams("search")
                        Return SetPassword(context)
                    Case "plugin;RuTr_Password"
                        Password = context.GetRequestParams("search")
                        Return AuthorizationRuTr(context)
                    Case "plugin;RuTr_Capcha_Key"
                        Capcha = context.GetRequestParams("search")
                        Return AuthorizationRuTr(context)
                End Select
            End If


            Select Case path
                Case "plugin"
                    Return GetTopList(context)
                Case "plugin;tv"
                    Return GetTV(context)
                Case "plugin;nnmclub"
                    Return GetTopNNMClubList(context)
                Case "plugin;rutor"
                    Return GetTopListRuTor(context)
                Case "plugin;rutr"
                    Return GetTopListRuTr(context)
            End Select



            Dim PathSpliter() As String = path.Split(";")

            Select Case PathSpliter(PathSpliter.Length - 1)
                 'Трекер NNM
                Case "PAGENNM"
                    Return GetPAGENNM(context, PathSpliter(PathSpliter.Length - 2))
                Case "PAGEFILMNNM"
                    Return GetTorrentPAGENNM(context, PathSpliter(PathSpliter.Length - 2))
                    'Трекер РуТор
                Case "PAGERUTOR"
                    Return GetPAGERUTOR(context, PathSpliter(PathSpliter.Length - 2))
                Case "PAGEFILMRUTOR"
                    Return GetTorrentPageRuTor(context, PathSpliter(PathSpliter.Length - 2))
                    'Трекер RuTracker
                Case "RuTrSubGroop"
                    Return GetListRuTrCategory(context, PathSpliter(PathSpliter.Length - 3), PathSpliter(PathSpliter.Length - 2))
                Case "RuTrGroop"
                    Return GetListRuTrCategory(context, PathSpliter(PathSpliter.Length - 2))
                Case "Search_Groop_RuTr"
                    Return SearchListRuTr(context, context.GetRequestParams("search"), PathSpliter(PathSpliter.Length - 2))
                Case "PAGERUTR"
                    Return GetPageRuTr(context, PathSpliter(PathSpliter.Length - 2))
                Case "PAGEFILMRUTR"
                    Return GetTorrentPageRuTr(context, PathSpliter(PathSpliter.Length - 2))
                Case "RuTrNonAuthorization"
                    Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree("Software\RemoteFork\Plugins\RuTracker\", False)
                    Return GetTopListRuTr(context)

                  'Тв
                Case "torrenttv"
                    Return GetTorrentTV(context)
                Case "acestreamnettv"
                    Return GetAceStreamNetTV(context)
                Case "SEARCHTV"
                    Return GetPageSearchStreamTV(context, PathSpliter(PathSpliter.Length - 2), PathSpliter(PathSpliter.Length - 3))


                    'Настройки
                Case "SETTINGS"
                    Return GetListSettings(context, PathSpliter(PathSpliter.Length - 2))
                Case "SETTINGS_NNM"
                    Return GetListSettingsNNM(context, PathSpliter(PathSpliter.Length - 2))
            End Select

            'тв
            Select Case Right(PathSpliter(PathSpliter.Length - 1), 7)
                Case ".iproxy"
                    Return LastModifiedPlayList(PathSpliter(PathSpliter.Length - 1), context)
            End Select

            Dim PathFiles As String = Microsoft.VisualBasic.Strings.Replace(PathSpliter(PathSpliter.Length - 1), "|", "\")
            Dim items As New System.Collections.Generic.List(Of Item)

            Select Case System.IO.Path.GetExtension(PathFiles)

                Case ".torrent"
                    Dim Description As String = SearchDescriptions(System.IO.Path.GetFileNameWithoutExtension(PathFiles.Split("(", ".", "[", "|")(0)))

                    Dim PlayListtoTorrent() As TorrentPlayList = GetFileList(PathFiles)

                    For Each PlayListItem As TorrentPlayList In PlayListtoTorrent
                        Dim Item As New Item
                        With Item
                            .Name = PlayListItem.Name
                            .ImageLink = PlayListItem.ImageLink
                            .Link = PlayListItem.Link
                            .Type = ItemType.FILE
                            .Description = Description
                        End With
                        items.Add(Item)
                    Next


                    Return PlayListPlugPar(items, context)

                Case ".m3u8", ".m3u"
                    Dim WC As New System.Net.WebClient
                    Return toSource(WC.DownloadString(PathFiles), context)
            End Select

            Dim ListFolders() As String = System.IO.Directory.GetDirectories(PathFiles)
            For Each Fold As String In ListFolders
                Dim Item As New Item
                With Item
                    .Name = System.IO.Path.GetFileName(Fold)
                    .Link = Microsoft.VisualBasic.Strings.Replace(Fold, "\", "|")
                    .ImageLink = ICO_Folder
                    .Type = ItemType.DIRECTORY
                End With
                items.Add(Item)
            Next

            If AceProxEnabl = True Then
                For Each File As String In System.IO.Directory.EnumerateFiles(PathFiles, "*.*", System.IO.SearchOption.TopDirectoryOnly).Where(Function(s) s.EndsWith(".torrent"))
                    Dim Item As New Item
                    With Item
                        .ImageLink = ICO_TorrentFile
                        .Name = System.IO.Path.GetFileNameWithoutExtension(File)
                        .Link = Microsoft.VisualBasic.Strings.Replace(File, "\", "|")
                        .Description = .Name
                        .Type = ItemType.DIRECTORY
                    End With
                    items.Add(Item)
                Next
            End If

            For Each File As String In System.IO.Directory.EnumerateFiles(PathFiles, "*.*", System.IO.SearchOption.TopDirectoryOnly).Where(Function(s) s.EndsWith(".mkv") OrElse s.EndsWith(".avi") OrElse s.EndsWith(".mp4"))
                Dim Item As New Item
                With Item
                    .ImageLink = IconFile(File)
                    .Name = System.IO.Path.GetFileNameWithoutExtension(File)
                    .Link = Microsoft.VisualBasic.Strings.Replace("http://" & IPAdress & ":" & PortRemoteFork & "/?file:/" & File, "\", "/")
                    .Description = .Link
                    .Type = ItemType.FILE
                End With
                items.Add(Item)
            Next

            For Each File As String In System.IO.Directory.EnumerateFiles(PathFiles, "*.*", System.IO.SearchOption.TopDirectoryOnly).Where(Function(s) s.EndsWith(".mp3") OrElse s.EndsWith(".flac") OrElse s.EndsWith(".wma"))
                Dim Item As New Item
                With Item
                    .ImageLink = IconFile(File)
                    .Name = System.IO.Path.GetFileNameWithoutExtension(File)
                    .Link = Microsoft.VisualBasic.Strings.Replace("http://" & IPAdress & ":" & PortRemoteFork & "/?file:/" & File, "\", "/")
                    .Description = .Link
                    .Type = ItemType.FILE
                End With
                items.Add(Item)
            Next

            For Each File As String In System.IO.Directory.EnumerateFiles(PathFiles, "*.*", System.IO.SearchOption.TopDirectoryOnly).Where(Function(s) s.EndsWith(".jpg") OrElse s.EndsWith(".png") OrElse s.EndsWith(".gif") OrElse s.EndsWith(".bmp"))
                Dim Item As New Item
                With Item
                    .ImageLink = IconFile(File)
                    .Name = System.IO.Path.GetFileNameWithoutExtension(File)
                    .Link = Strings.Replace("http://" & IPAdress & ":" & PortRemoteFork & "/?file:/" & File, "\", "/")
                    .Description = .Link & "<html><p><div align=""center""> <img src=""" & .Link & """ width=""80%"" /></div><p><p></html>"

                    .Type = ItemType.FILE
                End With
                items.Add(Item)
            Next

            For Each File As String In System.IO.Directory.EnumerateFiles(PathFiles, "*.*", System.IO.SearchOption.TopDirectoryOnly).Where(Function(s) s.EndsWith(".m3u") OrElse s.EndsWith(".m3u8"))
                Dim Item As New Item
                With Item
                    .ImageLink = ICO_M3UFile
                    .Name = System.IO.Path.GetFileNameWithoutExtension(File)
                    .Link = Microsoft.VisualBasic.Strings.Replace("http://" & IPAdress & ":" & PortRemoteFork & "/?file:/" & File, "\", "/")
                    .Description = .Link
                    .Type = ItemType.DIRECTORY
                End With
                items.Add(Item)
            Next

            PlayList.IsIptv = "false"
            Return PlayListPlugPar(items, context)

        End Function

        Public Function GetInfo(ByVal context As IPluginContext) As Playlist
            Dim playlist = New PluginApi.Plugins.Playlist()
            Dim items As New List(Of Item)()
            Dim Item As New Item()
            Item.Name = "information"
            Item.Link = "2"
            Item.Type = ItemType.FILE
            Item.Description = "peers:2<br>"
            items.Add(Item)
            playlist.Items = items.ToArray()
            Return playlist
        End Function

        Public Function toSource(ByVal source As String, ByVal context As IPluginContext) As Playlist 'Отдает текст source напрямую в forkplayer игнорируя остальные поля Playlist
            PlayList.source = source
            Return PlayList
        End Function

        Function PlayListPlugPar(ByVal items As System.Collections.Generic.List(Of Item), ByVal context As IPluginContext, Optional ByVal next_page_url As String = "") As PluginApi.Plugins.Playlist
            If next_page_url <> "" Then
                Dim pluginParams = New NameValueCollection()
                pluginParams(PLUGIN_PATH) = next_page_url
                PlayList.NextPageUrl = context.CreatePluginUrl(pluginParams)
            Else
                PlayList.NextPageUrl = Nothing
            End If
            PlayList.Timeout = "60" 'sec

            PlayList.Items = items.ToArray()
            For Each Item As Item In PlayList.Items
                If ItemType.DIRECTORY = Item.Type Then
                    Dim pluginParams = New NameValueCollection()
                    pluginParams(PLUGIN_PATH) = Item.Link
                    Item.Link = context.CreatePluginUrl(pluginParams)
                End If
            Next
            Return PlayList
        End Function

        Public Function GetTopList(ByVal context As IPluginContext) As PluginApi.Plugins.Playlist
            Load_Settings()
            Dim items As New System.Collections.Generic.List(Of Item)

            Dim WC As New System.Net.WebClient
            WC.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0")
            WC.Encoding = System.Text.Encoding.UTF8

            Dim ItemTop, ItemTorrentTV, ItemNNMClub, ItemRuTor, ItemRuTracker, ItemTV As New Item
            Try
                AceProxEnabl = True
                Dim AceMadiaGet As String
                AceMadiaGet = WC.DownloadString("http://" & IPAdress & ":" & PortAce & "/webui/api/service?method=get_version&format=jsonp&callback=mycallback")
                AceMadiaGet = "<html> Ответ от движка Ace Media получен: " & "<div>" & AceMadiaGet & "</div></html>"


                With ItemTop
                    .ImageLink = "http://cs5-2.4pda.to/8001667.png"
                    .Name = "<span style=""color:#9DB1CC""> - AceTorrentPlay - </span>"
                    .Link = ""
                    .Type = ItemType.FILE
                    .Description = AceMadiaGet & "<html><p><p><img src="" http://static.acestream.net/sites/acestream/img/ACE-logo.png""></html>"

                End With

                With ItemTV
                    .Name = "Телевиденье"
                    .Type = ItemType.DIRECTORY
                    .Link = "tv"
                    .ImageLink = "http://s1.iconbird.com/ico/1112/Television/w256h25613523820647.png"
                    Try
                        If System.IO.File.Exists(System.IO.Path.GetTempPath & "MyTraf.tmp") = False Then
                            WC.DownloadFile("http://pomoyka.lib.emergate.net/trash/ttv-list/MyTraf.php", System.IO.Path.GetTempPath & "MyTraf.tmp")
                        End If
                        .Description = "<html><font face="" Arial"" size="" 5""><b>" & UCase(.Name) & "</font></b><p><img width=""100%"" src=""https://retailradio.biz/wp-content/uploads/2016/10/video-wall.png""></html><p>" & WC.DownloadString(System.IO.Path.GetTempPath & "MyTraf.tmp")
                    Catch ex As Exception
                        .Description = "<html><font face="" Arial"" size="" 5""><b>" & UCase(.Name) & "</font></b><p></html><p>" & ex.Message

                    End Try
                End With

                With ItemNNMClub
                    .ImageLink = ICO_NNMClub
                    .Name = "Tracker NoNaMe-Club"
                    .Link = "nnmclub"
                    .Type = ItemType.DIRECTORY

                    Dim Description_NNMC As String = "Добро пожаловать на интеллигентный битторрент. Наш торрент-трекер - это место, где можно не только скачать фильмы, музыку и программы, но и найти друзей или просто пообщаться на интересующие Вас темы. Для того, чтобы скачать с помощью торрента не нужно платить. Главное правило торрент-трекера: скачал сам, останься на раздаче. Для этого просто не надо удалять торрент из клиента."
                    .Description = "<html><font face="" Arial"" size="" 5""><b>Трекер " & .Name & "</font></b><p><img src=""" & LOGO_NoNaMeClub & """ /> <p>" & Description_NNMC & "</html>"
                End With

                With ItemRuTor
                    .ImageLink = "http://s1.iconbird.com/ico/2013/12/566/w128h1281387223970serpmolot128x128.png"
                    .Name = "Tracker Rutor"
                    .Link = "rutor"
                    .Type = ItemType.DIRECTORY
                    Dim Description_RuTor As String = "Файлы для обмена предоставлены пользователями сайта. Администрация не несёт ответственности за их содержание. На сервере хранятся только торрент-файлы. Это значит, что мы не храним никаких нелегальных материалов."
                    .Description = "<html><font face=""Arial"" size=""5""><b>" & .Name & "</font></b><p><img src=""" & LOGO_TrackerRutor & """ /><p>" & Description_RuTor & "</html>"
                End With

                With ItemRuTracker
                    .ImageLink = "http://s1.iconbird.com/ico/0612/Inside/w256h2561339745864icontextoinsidefavorites.png"
                    .Name = "Tracker RuTracker"
                    .Link = "rutr"
                    .Type = ItemType.DIRECTORY
                    Dim Description_RuTr As String = "RuTracker.org (ранее — Torrents.ru) — крупнейший русскоязычный BitTorrent-трекер, насчитывающий более 15,3 миллиона зарегистрированных учётных записей. На трекере зарегистрировано 1,728 миллиона раздач (из которых более 1,60 миллиона — «живых»), суммарный размер которых составляет 3.20 петабайта"
                    .Description = "<html><font face=""Arial"" size=""5""><b>" & .Name & "</font></b><p><img src=""" & LOGO_RuTr & """ /><p>" & Description_RuTr & "</html>"
                End With

                items.Add(ItemTop)
                'items.Add(ItemTorrentTV)
                items.Add(ItemTV)
                items.Add(ItemRuTor)
                items.Add(ItemRuTracker)
                items.Add(ItemNNMClub)

            Catch ex As Exception
                AceProxEnabl = False
                With ItemTop
                    .ImageLink = ICO_Error2
                    .Name = "<span style=""color:#FF380A""> - AceTorrentPlay - </span>"
                    .Link = ""
                    .Type = ItemType.FILE
                    .Description = "Ответ от движка Ace Media не получен!" & "<p>" & ex.Message & "</p>"
                End With
                items.Add(ItemTop)
            End Try

            Dim ListDisk() As System.IO.DriveInfo = System.IO.DriveInfo.GetDrives
            For Each Disk As System.IO.DriveInfo In ListDisk
                If Disk.DriveType = System.IO.DriveType.Fixed Then
                    Dim Item As New Item
                    With Item
                        .Name = Disk.Name & "  " & "(" & Math.Round(Disk.TotalFreeSpace / 1024 / 1024 / 1024, 2) & "ГБ свободно из " & Math.Round(Disk.TotalSize / 1024 / 1024 / 1024, 2) & "ГБ)"
                        .ImageLink = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597268hddwin.png"
                        .Link = Microsoft.VisualBasic.Strings.Replace(Disk.Name, "\", "|")
                        .Type = ItemType.DIRECTORY
                        .Description = .Name & Chr(10) & Chr(13) & " <html><p> Метка диска: " & Disk.VolumeLabel & "</html>"
                    End With

                    items.Add(Item)
                End If
            Next

            Dim ItemSettings As New Item
            With ItemSettings
                .Name = "Настройки"
                .Link = ";SETTINGS"
                .Type = ItemType.DIRECTORY
                .ImageLink = ICO_Settings
            End With
            items.Add(ItemSettings)

            PlayList.IsIptv = "false"
            Return PlayListPlugPar(items, context)
        End Function



        Function SearchDescriptions(ByVal Name As String) As String
            Dim HtmlFile As String

            Try
                Dim WC As New System.Net.WebClient
                WC.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0")
                WC.Encoding = System.Text.Encoding.UTF8
                Dim Str As String = WC.DownloadString("http://www.kinomania.ru/search/?q=" & System.IO.Path.GetFileName(Name))

                Dim Regex As New System.Text.RegularExpressions.Regex("<header><h3>По вашему запросу ничего не найдено</h3></header>", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                Dim Bool As Boolean = Regex.IsMatch(Str)


                If Bool = True Then
                    HtmlFile = "<html><div>Описание не найдено.</div><div>Попробуйте переименовать торрент файл</div></html>"
                Else
                    Regex = New System.Text.RegularExpressions.Regex("(?<=fid="").*(?="">)", System.Text.RegularExpressions.RegexOptions.IgnoreCase)

                    Dim FidStr As String = Regex.Matches(Str)(0).Value
                    Str = WC.DownloadString("http://www.kinomania.ru/film/" & FidStr)

                    Dim Title As String = Nothing
                    Try
                        Regex = New System.Text.RegularExpressions.Regex("(?<=<title>).*(?=</title>)")
                        Title = Regex.Matches(Str)(0).Value.Replace("| KINOMANIA.RU", "")
                    Catch ex As Exception
                    End Try

                    Dim ImagePath As String = Nothing
                    Try
                        Regex = New System.Text.RegularExpressions.Regex("(?<=src="").*?(.jpg)")
                        ImagePath = Regex.Matches(Str)(0).Value
                        ImagePath = "http://" & IPAdress & ":8027/proxym3u8B" & Base64Encode(ImagePath & "OPT:ContentType--image/jpegOPEND:/") & "/"
                    Catch ex As Exception
                    End Try

                    Dim Opisanie As String = Nothing
                    Try
                        Regex = New System.Text.RegularExpressions.Regex("(<div class=""l-col l-col-2"">)(\n|.)*?(</div>)")
                        Opisanie = Regex.Matches(Str)(0).Value
                    Catch ex As Exception
                    End Try

                    Dim InfoFile As String = Nothing
                    Try
                        Regex = New System.Text.RegularExpressions.Regex("(<h2 class=""b-switcher"">)(\n|.)*?(</div>)")
                        InfoFile = Regex.Matches(Str)(0).Value
                    Catch ex As Exception

                    End Try


                    HtmlFile = "<div id=""poster"" style=""float:left;padding:4px;        background-color:#EEEEEE;margin:0px 13px 1px 0px;"">" & "<img src=""" & ImagePath & """ style=""width:180px;float:left;"" /></div><span style=""color:#3090F0"">" & Title & "</span>" & Opisanie & "<span style=""color:#3090F0"">Информация</span><br>" & InfoFile

                End If

            Catch ex As Exception
                HtmlFile = ex.Message
            End Try
            Return HtmlFile

        End Function

        'Function GetRequest(ByVal link As String, Optional ByVal Cookies As String = Nothing, Optional ByVal ProxySwitch As Boolean = False, Optional ByVal Method As String = "GET", Optional ByVal Data As String = Nothing) As String ''''''''""""""""""""""""""""""""""""""
        '    Dim Request As System.Net.HttpWebRequest = System.Net.HttpWebRequest.CreateHttp(link)
        '    If ProxySwitch = True Then Request.Proxy = New System.Net.WebProxy(ProxyServr, ProxyPort)
        '    Request.Method = Method
        '    Request.ContentType = "text/html; charset=windows-1251"
        '    If Cookies <> Nothing Then Request.Headers.Add("Cookie", Cookies)

        '    Request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36"
        '    Request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8"
        '    Request.Headers.Add(Net.HttpRequestHeader.AcceptLanguage, "ru-ru,ru;q=0.8,en-us;q=0.5,en;q=0.3")
        '    Request.Headers.Add(Net.HttpRequestHeader.AcceptEncoding, "gzip,deflate")
        '    Request.Headers.Add(Net.HttpRequestHeader.AcceptCharset, "windows-1251,utf-8;q=0.7,*;q=0.7")
        '    Request.KeepAlive = True
        '    Request.ContentType = "application/x-www-form-urlencoded"
        '    Request.AllowAutoRedirect = False
        '    Request.AutomaticDecompression = Net.DecompressionMethods.GZip

        '    If Data <> Nothing Then
        '        Request.ContentType = "application/x-www-form-urlencoded"
        '        Dim myStream As System.IO.Stream = Request.GetRequestStream
        '        Dim DataByte() As Byte = System.Text.Encoding.GetEncoding(1251).GetBytes(Data)
        '        myStream.Write(DataByte, 0, DataByte.Length)
        '        myStream.Close()
        '    End If

        '    Dim Response As System.Net.HttpWebResponse = Request.GetResponse()
        '    Dim dataStream As System.IO.Stream = Response.GetResponseStream()
        '    Dim reader As New System.IO.StreamReader(dataStream, System.Text.Encoding.GetEncoding(1251))
        '    Return reader.ReadToEnd()
        'End Function

#Region "Rutracker"
        Dim ProxyEnablerRuTr As Boolean = True
        Dim TrackerServer As String = "https://rutracker.org"
        ' Dim TrackerServer As String = "http://рутрекер.org"
#Region "Авторизация"
        Dim Login, Password, Cap_Sid, Cap_Code, Capcha, CookiesRuTr As String

        Dim UserAuthorization As String
        Function AuthorizationTest() As Boolean
            CookiesRuTr = Microsoft.Win32.Registry.GetValue("HKEY_CURRENT_USER\Software\RemoteFork\Plugins\RuTracker\", "Cookies", "")
            If CookiesRuTr = "" Then CookiesRuTr = "bb_ssl=1"
            Dim Request As System.Net.HttpWebRequest = System.Net.HttpWebRequest.CreateHttp(TrackerServer & "/forum/index.php")
            Request.Method = "GET"
            Request.Headers.Add("Cookie", CookiesRuTr)
            Request.Host = New Uri(TrackerServer).Host
            Request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36"
            Request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8"
            Request.Headers.Add(Net.HttpRequestHeader.AcceptLanguage, "ru-ru,ru;q=0.8,en-us;q=0.5,en;q=0.3")
            Request.Headers.Add(Net.HttpRequestHeader.AcceptEncoding, "gzip,deflate")
            Request.Headers.Add(Net.HttpRequestHeader.AcceptCharset, "windows-1251,utf-8;q=0.7,*;q=0.7")
            Request.KeepAlive = True
            Request.Referer = TrackerServer & "/forum/index.php"

            Request.ContentType = "application/x-www-form-urlencoded"
            Request.AllowAutoRedirect = False
            Request.AutomaticDecompression = Net.DecompressionMethods.GZip
            If ProxyEnablerRuTr = True Then Request.Proxy = New System.Net.WebProxy(ProxyServr, ProxyPort)


            Dim Response As Net.HttpWebResponse = Request.GetResponse
            Dim Stream As IO.Stream = Response.GetResponseStream
            Stream = Response.GetResponseStream
            Dim Reader As New System.IO.StreamReader(Stream, System.Text.Encoding.GetEncoding(1251))
            Dim OtvetServera As String = Reader.ReadToEnd.Replace(vbLf, " ")
            Reader.Close()
            Stream.Close()

            Dim Reg As New System.Text.RegularExpressions.Regex("(>Вход</span>).*?(</span>)")
            Dim Matchs As System.Text.RegularExpressions.MatchCollection = Reg.Matches(OtvetServera)

            If Matchs.Count > 0 Then
                Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\Software\RemoteFork\Plugins\RuTracker\", "Cookies", "bb_ssl=1")
                Return False
            Else
                Reg = New System.Text.RegularExpressions.Regex("(<span class=""logged-in-as-cap"">).*?(</div>)")
                Matchs = Reg.Matches(OtvetServera)
                If Matchs.Count > 0 Then
                    Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\Software\RemoteFork\Plugins\RuTracker\", "Cookies", CookiesRuTr)
                    UserAuthorization = Matchs(0).Value
                    Return True
                End If
            End If
            Return Nothing
        End Function

        Function AuthorizationRuTr(context As IPluginContext) As PluginApi.Plugins.Playlist
            Dim items As New System.Collections.Generic.List(Of Item)
            CookiesRuTr = "bb_ssl=1"
            Dim Request As System.Net.HttpWebRequest = System.Net.HttpWebRequest.CreateHttp(TrackerServer & "/forum/login.php?redirect=tracker.php")
            Request.Method = "POST"
            Request.Headers.Add("Cookie", CookiesRuTr)
            Request.Host = New Uri(TrackerServer).Host
            Request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36"
            Request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8"
            Request.Headers.Add(Net.HttpRequestHeader.AcceptLanguage, "ru-ru,ru;q=0.8,en-us;q=0.5,en;q=0.3")
            Request.Headers.Add(Net.HttpRequestHeader.AcceptEncoding, "gzip,deflate")
            Request.Headers.Add(Net.HttpRequestHeader.AcceptCharset, "windows-1251,utf-8;q=0.7,*;q=0.7")
            Request.KeepAlive = True
            Request.Referer = TrackerServer & "/forum/index.php"
            Request.ContentType = "application/x-www-form-urlencoded"
            Request.AllowAutoRedirect = False
            Request.AutomaticDecompression = Net.DecompressionMethods.GZip
            If ProxyEnablerRuTr = True Then Request.Proxy = New System.Net.WebProxy(ProxyServr, ProxyPort)

            Dim StringData As String
            If Capcha = "" Then
                StringData = "redirect=tracker.php&login_username=" & Login & "&login_password=" & Password & "&login=Вход"
            Else
                StringData = "redirect=tracker.php&login_username=" & Login & "&login_password=" & Password & "&cap_sid=" & Cap_Sid & "&" & Cap_Code & "=" & Capcha & "&login=%C2%F5%EE%E4"
            End If
            Capcha = ""
            Dim Stream As IO.Stream = Request.GetRequestStream
            Dim ByteData() As Byte = System.Text.Encoding.GetEncoding(1251).GetBytes(StringData)
            Stream.Write(ByteData, 0, ByteData.Length)
            Stream.Close()


            Dim Response As Net.HttpWebResponse = Request.GetResponse
            Stream = Response.GetResponseStream
            Dim Reader As New System.IO.StreamReader(Stream, System.Text.Encoding.GetEncoding(1251))
            Dim OtvetServera As String = Reader.ReadToEnd.Replace(vbLf, " ")

            If Not String.IsNullOrEmpty(Response.Headers("Set-Cookie")) Then

                CookiesRuTr = Response.Headers("Set-Cookie")
                Request = System.Net.HttpWebRequest.CreateHttp(TrackerServer & "/forum/index.php")
                Request.Method = "GET"
                Request.Headers.Add("Cookie", CookiesRuTr)
                Request.Host = New Uri(TrackerServer).Host
                Request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36"
                Request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8"
                Request.Headers.Add(Net.HttpRequestHeader.AcceptLanguage, "ru-ru,ru;q=0.8,en-us;q=0.5,en;q=0.3")
                Request.Headers.Add(Net.HttpRequestHeader.AcceptEncoding, "gzip,deflate")
                Request.Headers.Add(Net.HttpRequestHeader.AcceptCharset, "windows-1251,utf-8;q=0.7,*;q=0.7")
                Request.KeepAlive = True
                Request.Referer = TrackerServer & "/forum/login.php?redirect=tracker.php"
                Request.Headers.Add(Net.HttpRequestHeader.Cookie, "spylog_test=1")
                Request.ContentType = "application/x-www-form-urlencoded"
                Request.AllowAutoRedirect = False
                Request.AutomaticDecompression = Net.DecompressionMethods.GZip
                If ProxyEnablerRuTr = True Then Request.Proxy = New System.Net.WebProxy(ProxyServr, ProxyPort)

                Response = Request.GetResponse
                Stream = Response.GetResponseStream
                Reader = New System.IO.StreamReader(Stream, System.Text.Encoding.GetEncoding(1251))
                OtvetServera = Reader.ReadToEnd.Replace(vbLf, " ")
                Reader.Close()
                Stream.Close()

                Dim Reg As New System.Text.RegularExpressions.Regex("(<a href=""profile.php?mode=register""><b>).*?(</span>)")
                Dim Matchs As System.Text.RegularExpressions.MatchCollection = Reg.Matches(OtvetServera)

                If Matchs.Count > 0 Then
                    Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\Software\RemoteFork\Plugins\RuTracker\", "Cookies", "bb_ssl=1")
                    Return SetLogin(context)
                Else
                    Reg = New System.Text.RegularExpressions.Regex("(<span class=""logged-in-as-cap"">).*?(</div>)")
                    Matchs = Reg.Matches(OtvetServera)
                    If Matchs.Count > 0 Then
                        Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\Software\RemoteFork\Plugins\RuTracker\", "Cookies", CookiesRuTr)
                        Return GetTopListRuTr(context)
                    End If
                End If
            End If

            Dim AdressCapha As String
            Dim Regex As New System.Text.RegularExpressions.Regex("(?<=<td class=""tRight nowrap"">Код:</td> 					<td> 			<div><img src=""//).*?(.jpg)")
            Dim Matches As System.Text.RegularExpressions.MatchCollection = Regex.Matches(OtvetServera)
            If Matches.Count > 0 Then
                AdressCapha = "http://" & Regex.Matches(OtvetServera)(0).Value
                Regex = New System.Text.RegularExpressions.Regex("(?<=name=""cap_sid"" value="").*?(?="">)")
                Cap_Sid = Regex.Matches(OtvetServera)(0).Value
                Regex = New System.Text.RegularExpressions.Regex("(cap_code_).*?(?="")")
                Cap_Code = Regex.Matches(OtvetServera)(0).Value


                Regex = New System.Text.RegularExpressions.Regex("(<h4 class=""warnColor1 tCenter mrg_16"">).*?(</h4>)")
                Matches = Regex.Matches(OtvetServera)

                Dim ItemCap As New Item
                With ItemCap
                    .Name = "Capcha"
                    .SearchOn = "Введите код"
                    .Link = "RuTr_Capcha_Key"
                    .Description = Matches(0).Value & "<img src=""" & AdressCapha & """ width=""120"" height=""72"">"
                    .ImageLink = AdressCapha
                End With
                items.Add(ItemCap)

            End If




            PlayList.IsIptv = "false"
            Return PlayListPlugPar(items, context)
        End Function

        Function SetLogin(context As IPluginContext) As PluginApi.Plugins.Playlist
            Dim items As New System.Collections.Generic.List(Of Item)
            Dim Item As New Item

            With Item
                .Name = "Login"
                .Link = "RuTr_Login"
                .Type = ItemType.DIRECTORY
                .SearchOn = "Login"
                .ImageLink = ICO_Login
            End With
            items.Add(Item)

            PlayList.IsIptv = "false"
            Return PlayListPlugPar(items, context)
        End Function


        Function SetPassword(context As IPluginContext) As PluginApi.Plugins.Playlist
            Dim items As New System.Collections.Generic.List(Of Item)
            Dim Item As New Item

            Item = New Item
            With Item
                .Name = "Password"
                .Link = "RuTr_Password"
                .Type = ItemType.DIRECTORY
                .SearchOn = "Password"
                .ImageLink = ICO_Password
            End With
            items.Add(Item)

            PlayList.IsIptv = "false"
            Return PlayListPlugPar(items, context)
        End Function
#End Region

        Dim KategoriRuTracker As String
        Public Function GetListRuTrCategory(context As IPluginContext, ByVal Groop As String, Optional ByVal SubGroop As String = Nothing) As PluginApi.Plugins.Playlist

            Dim items As New List(Of Item)


            Dim Regex As New System.Text.RegularExpressions.Regex("(<optgroup label=""&nbsp;" & Groop & ").*?(optgroup>)")
            Dim GroopText As String = Regex.Matches(KategoriRuTracker)(0).Value.Replace(" |- ", "::").Replace("(", " ").Replace(")", " ")



            Select Case SubGroop
                Case Nothing

                    Regex = New System.Text.RegularExpressions.Regex("(<option).*?(option>)")
                    Dim Matches As System.Text.RegularExpressions.MatchCollection = Regex.Matches(GroopText)
                    Dim RegexOptionsID As New System.Text.RegularExpressions.Regex("(?<=value="").*?(?="")")
                    Dim IDSubGroops As String = Nothing
                    For Each SSGroop As System.Text.RegularExpressions.Match In Matches
                        IDSubGroops = IDSubGroops & RegexOptionsID.Match(SSGroop.Value).Value & ","
                    Next
                    Dim ItemSearch As New Item
                    With ItemSearch
                        .Name = "Поиск"
                        .Link = IDSubGroops.Remove(IDSubGroops.Count - 1) & ";Search_Groop_RuTr"
                        .Type = ItemType.DIRECTORY
                        .SearchOn = "Поик в категории"
                        .ImageLink = ICO_Search
                        .Description = "<html><font face=""Arial"" size=""5""><b>" & .Name & "</font></b><p><img src=""" & LOGO_RuTr & """ /> <p>" & UserAuthorization
                    End With
                    items.Add(ItemSearch)


                    Regex = New System.Text.RegularExpressions.Regex("(?<=class=""root_forum has_sf"">).*?(?=&nbsp;)")
                    For Each SGroop As System.Text.RegularExpressions.Match In Regex.Matches(GroopText)
                        Dim Item As New Item
                        Item.Name = SGroop.Value
                        Select Case Item.Name
                            Case "F.A.Q."
                            Case Else
                                With Item
                                    .Type = ItemType.DIRECTORY
                                    .ImageLink = ICO_Folder
                                    .Description = "<html><font face=""Arial"" size=""5""><b>" & .Name & "</font></b><p><img src=""" & LOGO_RuTr & """ /> <p>" & UserAuthorization
                                    .Link = Groop & ";" & .Name & ";RuTrSubGroop"
                                    items.Add(Item)
                                End With
                        End Select
                    Next
                Case Else




                    Regex = New System.Text.RegularExpressions.Regex("(<option).*?(class=""root_forum has_sf"">" & SubGroop & ")")
                    Dim SubSubGroopStart As String = Regex.Match(GroopText.Replace("^", vbLf)).Value


                    Regex = New System.Text.RegularExpressions.Regex("(" & SubSubGroopStart & ").*?(?=class=""root_forum has_sf"">|optgroup>)")
                    Dim Options As String = Regex.Match(GroopText).Value

                    Regex = New System.Text.RegularExpressions.Regex("(<option).*?(option>)")
                    Dim RegexOptionsName As New System.Text.RegularExpressions.Regex("(?<=""root_forum has_sf"">|""root_forum"">|::).*?(?=&nbsp;)")
                    Dim RegexOptionsID As New System.Text.RegularExpressions.Regex("(?<=value="").*?(?="")")


                    Dim Matches As System.Text.RegularExpressions.MatchCollection = Regex.Matches(Options)

                    Dim IDSubGroops As String = Nothing
                    For Each SSGroop As System.Text.RegularExpressions.Match In Matches
                        IDSubGroops = IDSubGroops & RegexOptionsID.Match(SSGroop.Value).Value & ","
                    Next
                    Dim ItemSearch As New Item
                    With ItemSearch
                        .Name = "Поиск"
                        .Link = IDSubGroops.Remove(IDSubGroops.Count - 1) & ";Search_Groop_RuTr"
                        .Type = ItemType.DIRECTORY
                        .SearchOn = "Поик в подкатегории"
                        .ImageLink = ICO_Search
                        .Description = "<html><font face=""Arial"" size=""5""><b>" & .Name & "</font></b><p><img src=""" & LOGO_RuTr & """ /> <p>" & UserAuthorization
                    End With
                    items.Add(ItemSearch)

                    For Each SSGroop As System.Text.RegularExpressions.Match In Matches
                        Dim Item As New Item
                        With Item
                            .Name = RegexOptionsName.Match(SSGroop.Value).Value
                            .Type = ItemType.DIRECTORY
                            .ImageLink = ICO_FolderVideo
                            .Description = "<html><font face=""Arial"" size=""5""><b>" & .Name & "</font></b><p><img src=""" & LOGO_RuTr & """ /> <p>" & UserAuthorization
                            .Link = TrackerServer & "/forum/tracker.php?f=" & RegexOptionsID.Match(SSGroop.Value).Value & ";PAGERUTR"
                            items.Add(Item)
                        End With
                    Next



            End Select


            PlayList.IsIptv = "False"
            Return PlayListPlugPar(items, context)
        End Function

        Public Function GetPageRuTr(context As IPluginContext, ByVal URL As String) As PluginApi.Plugins.Playlist
            Dim RequestPost As System.Net.WebRequest = System.Net.WebRequest.CreateHttp(URL)

            If ProxyEnablerRuTr = True Then RequestPost.Proxy = New System.Net.WebProxy(ProxyServr, ProxyPort)
            RequestPost.Method = "POST"
            RequestPost.ContentType = "text/html; charset=windows-1251"
            RequestPost.Headers.Add("Cookie", CookiesRuTr)
            RequestPost.ContentType = "application/x-www-form-urlencoded"
            Dim myStream As System.IO.Stream = RequestPost.GetRequestStream
            Dim DataByte() As Byte = System.Text.Encoding.GetEncoding(1251).GetBytes("prev_new=0&prev_oop=0&o=1&s=2&tm=-1&pn=&nm=")
            myStream.Write(DataByte, 0, DataByte.Length)
            myStream.Close()

            Dim Response As System.Net.HttpWebResponse = RequestPost.GetResponse
            Dim dataStream As System.IO.Stream = Response.GetResponseStream
            Dim reader As New System.IO.StreamReader(dataStream, System.Text.Encoding.GetEncoding(1251))
            Dim ResponseFromServer As String = reader.ReadToEnd()

            Dim items As New System.Collections.Generic.List(Of Item)

            Dim Regex As New System.Text.RegularExpressions.Regex("start|f-1")
            If Regex.IsMatch(URL) = False Then
                Regex = New System.Text.RegularExpressions.Regex("(?<=f=).*?(?=&|$)")
                Dim ItemSearch As New Item
                With ItemSearch
                    .Name = "Поиск"
                    .Link = Regex.Match(URL).Value & ";Search_Groop_RuTr"
                    .Type = ItemType.DIRECTORY
                    .SearchOn = "Поик в категории"
                    .ImageLink = ICO_Search
                    .Description = "<html><font face=""Arial"" size=""5""><b>" & .Name & "</font></b><p><img src=""" & LOGO_RuTr & """ /> <p>" & UserAuthorization
                    items.Add(ItemSearch)
                End With
            End If

            Regex = New System.Text.RegularExpressions.Regex("(<tr class=""tCenter hl-tr"">).*?(</tr>)")
            Dim Result As System.Text.RegularExpressions.MatchCollection = Regex.Matches(ResponseFromServer.Replace(Microsoft.VisualBasic.vbLf, " "))
            If Result.Count > 0 Then
                For Each Match As System.Text.RegularExpressions.Match In Result
                    Dim Item As New Item
                    Regex = New System.Text.RegularExpressions.Regex("(?<=<a data-topic_id="").*?(?="")")
                    Dim LinkID As String = Regex.Matches(Match.Value)(0).Value
                    Item.Link = TrackerServer & "/forum/viewtopic.php?t=" & LinkID & ";PAGEFILMRUTR"
                    Regex = New System.Text.RegularExpressions.Regex("(?<=" & LinkID & """>).*?(?=</a>)")
                    Item.Name = Regex.Matches(Match.Value)(0).Value
                    Item.ImageLink = ICO_TorrentFile
                    Item.Description = GetDescriptionRuTr(Match.Value)
                    items.Add(Item)
                Next
            Else
                Return NonSearch(context, True)
            End If


            next_page_url = Nothing
            Regex = New System.Text.RegularExpressions.Regex("(?<=&amp;start=).*?(?="")")
            Result = Regex.Matches(ResponseFromServer)
            If Result.Count > 0 Then
                Regex = New System.Text.RegularExpressions.Regex("(.*).*(?=&start)")
                Dim Matchs As System.Text.RegularExpressions.MatchCollection = Regex.Matches(URL)
                If Matchs.Count > 0 Then
                    next_page_url = Matchs(0).Value & "&start=" & Result(Result.Count - 1).Value & ";PAGERUTR"
                Else
                    next_page_url = URL & "&start=" & Result(Result.Count - 1).Value & ";PAGERUTR"
                End If
            End If


            PlayList.IsIptv = "false"
            Return PlayListPlugPar(items, context, next_page_url)
        End Function

        Function GetDescriptionRuTr(ByVal HTML As String) As String


            Dim Title As String = Nothing
            Dim RegexTop As New System.Text.RegularExpressions.Regex("(?<=href=""viewtopic.php).*?(?=</a>)")
            Dim RegexSub As New System.Text.RegularExpressions.Regex("(?<=>).*(.*)")
            Try
                Title = RegexSub.Matches(RegexTop.Matches(HTML)(0).Value)(0).Value
            Catch ex As Exception
            End Try




            RegexTop = New System.Text.RegularExpressions.Regex("(?<=<a class=""small tr-dl dl-stub"").*?(?=;</a>)")
            RegexSub = New System.Text.RegularExpressions.Regex("(?<="">).*(?=&)")
            Dim SizeFile As String = Nothing
            Try
                SizeFile = "<br>Размер: " & RegexSub.Matches(RegexTop.Matches(HTML)(0).Value)(0).Value
            Catch ex As Exception
            End Try

            RegexTop = New System.Text.RegularExpressions.Regex("(?<=title=""Личи""><b>).*?(</b>)")
            RegexSub = New System.Text.RegularExpressions.Regex("(?<=<b class=""seedmed"">).*?(?=</b>)")
            Dim SidsPirs As String = Nothing
            Try
                SidsPirs = "<br><br>Seeders: " & RegexSub.Matches(HTML)(0).Value & "<br>Leechers: " & RegexTop.Matches(HTML)(0).Value
            Catch ex As Exception
            End Try

            RegexTop = New System.Text.RegularExpressions.Regex("(?<=<a class=""gen f"").*?(?=</a>)")
            RegexSub = New System.Text.RegularExpressions.Regex("(?<="">).*(.*)")
            Dim Razdel As String = Nothing
            Try
                Razdel = "<br><br>Раздел: " & RegexSub.Matches(RegexTop.Matches(HTML)(0).Value)(0).Value
            Catch ex As Exception
            End Try

            RegexTop = New System.Text.RegularExpressions.Regex("(?<=<td class=""row4 small nowrap"").*?(?=</td>)")
            RegexSub = New System.Text.RegularExpressions.Regex("(?<=<p>).*(?=</p>)")
            Dim DataCreate As String = Nothing
            Try

                DataCreate = "<br><br>Создан: " & RegexSub.Matches(RegexTop.Matches(HTML)(0).Value)(0).Value
            Catch ex As Exception
            End Try




            Return "<span style=""color:#3090F0"">" & Title & "</span><br>" & SizeFile & SidsPirs & Razdel & DataCreate
        End Function

        Sub LoadSaveGroupeRuTr()
            Dim RequestPost As System.Net.WebRequest = System.Net.WebRequest.CreateHttp(TrackerServer & "/forum/tracker.php")
            If ProxyEnablerRuTr = True Then RequestPost.Proxy = New System.Net.WebProxy(ProxyServr, ProxyPort)
            RequestPost.Method = "POST"
            RequestPost.ContentType = "text/html; charset=windows-1251"
            RequestPost.Headers.Add("Cookie", CookiesRuTr)
            RequestPost.ContentType = "application/x-www-form-urlencoded"
            Dim myStream As System.IO.Stream = RequestPost.GetRequestStream
            Dim DataByte() As Byte = System.Text.Encoding.GetEncoding(1251).GetBytes("prev_new=0&prev_oop=0&o=1&s=2&tm=-1&pn=&nm=")
            myStream.Write(DataByte, 0, DataByte.Length)
            myStream.Close()

            Dim Response As System.Net.HttpWebResponse = RequestPost.GetResponse
            Dim dataStream As System.IO.Stream = Response.GetResponseStream
            Dim reader As New System.IO.StreamReader(dataStream, System.Text.Encoding.GetEncoding(1251))
            Dim ResponseFromServer As String = reader.ReadToEnd.Replace(vbLf, "^")

            System.IO.File.WriteAllText(System.IO.Path.GetTempPath & "GroopRuTr", ResponseFromServer)
            System.IO.File.WriteAllText(System.IO.Path.GetTempPath & "UpdateGroopRuTr", Date.Now.Date)
        End Sub

        Public Function GetTopListRuTr(context As IPluginContext) As PluginApi.Plugins.Playlist
            If AuthorizationTest() = False Then Return SetLogin(context)

            Dim items As New System.Collections.Generic.List(Of Item)
            Dim Item As New Item

            With Item
                .Name = "Поиск"
                .Link = "Search_rutracker"
                .Type = ItemType.DIRECTORY
                .SearchOn = "Поик на RuTracker"
                .ImageLink = ICO_Search
                .Description = "<html><font face=""Arial"" size=""5""><b>" & .Name & "</font></b><p><img src=""" & LOGO_RuTr & """ /> <p>" & UserAuthorization
                items.Add(Item)
            End With

            Item = New Item
            With Item
                .Name = "Торренты за сегодня"
                .Link = TrackerServer & "/forum/tracker.php?f-1;PAGERUTR"
                .Type = ItemType.DIRECTORY
                .ImageLink = ICO_FolderVideo
                .Description = "<html><font face=""Arial"" size=""5""><b>" & .Name & "</font></b><p><img src=""" & LOGO_RuTr & """ /> <p>" & UserAuthorization
                items.Add(Item)
            End With

            If (System.IO.File.Exists(System.IO.Path.GetTempPath & "GroopRuTr") AndAlso System.IO.File.Exists(System.IO.Path.GetTempPath & "UpdateGroopRuTr")) = False Then
                LoadSaveGroupeRuTr()
            End If
            If Date.Now.Date <> System.IO.File.ReadAllText(System.IO.Path.GetTempPath & "UpdateGroopRuTr") Then
                LoadSaveGroupeRuTr()
            End If

            Dim RuTrHTML As String = IO.File.ReadAllText(System.IO.Path.GetTempPath & "GroopRuTr")
            Dim Regex As New System.Text.RegularExpressions.Regex("(?<=<img class=""site-logo"" src="").*?("")")
            LOGO_RuTr = "https:" & Regex.Match(RuTrHTML).Value

            Regex = New System.Text.RegularExpressions.Regex("(<p class=""select"">).*?(</select>)")
            KategoriRuTracker = Regex.Matches(RuTrHTML)(0).Value


            Regex = New System.Text.RegularExpressions.Regex("(<optgroup).*?(</optgroup>)")
            For Each Groop As System.Text.RegularExpressions.Match In Regex.Matches(KategoriRuTracker)

                Regex = New System.Text.RegularExpressions.Regex("(?<=label=""&nbsp;).*?(?="")")

                Select Case Regex.Matches(Groop.Value)(0).Value
                    Case "Новости", "Книги и журналы", "Игры", "Программы и Дизайн", "Обсуждения, встречи, общение"
                    Case Else
                        Item = New Item
                        With Item
                            .Name = Regex.Matches(Groop.Value)(0).Value
                            .Link = .Name & ";RuTrGroop"
                            .Type = ItemType.DIRECTORY
                            .ImageLink = ICO_Folder
                            .Description = "<html><font face=""Arial"" size=""5""><b>" & .Name & "</font></b><p><img src=""" & LOGO_RuTr & """ /> <p>" & UserAuthorization
                            items.Add(Item)
                        End With
                End Select
            Next


            Item = New Item
            With Item
                .Name = ""
                .Link = ""
                .Type = ItemType.FILE
                .ImageLink = ICO_Pusto
                items.Add(Item)
            End With


            Item = New Item
            With Item
                .Name = "Выйти с RuTracker"
                .Link = "RuTrNonAuthorization"
                .Type = ItemType.DIRECTORY
                .ImageLink = ICO_Delete
            End With
            items.Add(Item)

            PlayList.IsIptv = "False"
            Return PlayListPlugPar(items, context)
        End Function

        Public Function GetTorrentPageRuTr(context As IPluginContext, ByVal URL As String) As PluginApi.Plugins.Playlist

            Dim RequestGet As System.Net.WebRequest = Net.WebRequest.CreateHttp(URL)
            If ProxyEnablerRuTr = True Then RequestGet.Proxy = New System.Net.WebProxy(ProxyServr, ProxyPort)
            RequestGet.Method = "Get"
            RequestGet.Headers.Add("Cookie", CookiesRuTr)

            Dim Response As Net.HttpWebResponse = RequestGet.GetResponse
            Dim dataStream As System.IO.Stream = Response.GetResponseStream()
            Dim reader As New System.IO.StreamReader(dataStream, Text.Encoding.GetEncoding(1251))
            Dim responseFromServer As String = reader.ReadToEnd

            reader.Close()
            dataStream.Close()
            Response.Close()



            Dim Regex As New System.Text.RegularExpressions.Regex("(?<=<p><a href="").*?(?="")")
            Dim TorrentPath As String = TrackerServer & "/forum/" & Regex.Matches(responseFromServer)(0).Value


            Dim RequestTorrent As System.Net.WebRequest = Net.WebRequest.CreateHttp(TorrentPath)
            If ProxyEnablerRuTr = True Then RequestTorrent.Proxy = New System.Net.WebProxy(ProxyServr, ProxyPort)
            RequestTorrent.Method = "Get"
            RequestTorrent.Headers.Add("Cookie", CookiesRuTr)

            Response = RequestTorrent.GetResponse
            dataStream = Response.GetResponseStream()
            reader = New System.IO.StreamReader(dataStream, System.Text.Encoding.GetEncoding(1251))
            Dim FileTorrent As String = reader.ReadToEnd

            System.IO.File.WriteAllText(System.IO.Path.GetTempPath & "TorrentTemp.torrent", FileTorrent, System.Text.Encoding.GetEncoding(1251))

            reader.Close()
            dataStream.Close()
            Response.Close()


            Dim PlayListtoTorrent() As TorrentPlayList = GetFileList(System.IO.Path.GetTempPath & "TorrentTemp.torrent")

            Dim items As New System.Collections.Generic.List(Of Item)

            Dim Description As String = FormatDescriptionFileRuTr(responseFromServer)
            For Each PlayListItem As TorrentPlayList In PlayListtoTorrent

                Dim Item As New Item
                With Item
                    .Name = PlayListItem.Name
                    .ImageLink = PlayListItem.ImageLink
                    .Link = PlayListItem.Link
                    .Type = ItemType.FILE
                    .Description = Description
                End With
                items.Add(Item)
            Next



            PlayList.IsIptv = "false"
            Return PlayListPlugPar(items, context)
        End Function

        Function FormatDescriptionFileRuTr(ByVal HTML As String) As String

            HTML = HTML.Replace(Microsoft.VisualBasic.vbLf, "")

            Dim Title As String = Nothing
            Try
                Dim Regex As New System.Text.RegularExpressions.Regex("(?<=<title>).*?(</title>)")
                Title = Regex.Matches(HTML)(0).Value
            Catch ex As Exception
                Title = ex.Message
            End Try

            Dim SidsPirs As String = Nothing
            Try
                Dim Regex As New System.Text.RegularExpressions.Regex("(<td class=""catTitle"">).*?(?=<td class=""row3 pad_4"">)")
                SidsPirs = Regex.Matches(HTML)(0).Value
            Catch ex As Exception
                SidsPirs = ex.Message
            End Try


            Dim ImagePath As String = Nothing
            Try
                Dim Regex As New System.Text.RegularExpressions.Regex("(?<=<var class=""postImg postImgAligned img-right"" title="").*?(?="">)")
                ImagePath = Regex.Matches(HTML)(0).Value
            Catch ex As Exception
            End Try


            Dim InfoFile As String = Nothing
            Try
                Dim Regex As New System.Text.RegularExpressions.Regex("(<span class=""post-b"">).*(?=<div class=""sp-wrap"">)")
                InfoFile = Regex.Matches(HTML)(0).Value
            Catch ex As Exception
                InfoFile = ex.Message
            End Try

            Return "<div id=""poster"" style=""float:left;padding:4px;        background-color:#EEEEEE;margin:0px 13px 1px 0px;"">" & "<img src=""" & ImagePath & """ style=""width:180px;float:left;"" /></div><span style=""color:#3090F0"">" & Title & "</span><br><font face=""Arial Narrow"" size=""4""><span style=""color:#70A4A3"">" & SidsPirs & "</font></span>" & InfoFile

        End Function
        Public Function SearchListRuTr(context As IPluginContext, ByVal search As String, Optional ByVal Category As String = Nothing) As PluginApi.Plugins.Playlist
            Dim RequestPost As System.Net.WebRequest
            If Category = Nothing Then
                RequestPost = System.Net.WebRequest.CreateHttp(TrackerServer & "/forum/tracker.php?nm=" & search)
            Else
                RequestPost = System.Net.WebRequest.CreateHttp(TrackerServer & "/forum/tracker.php?f=" & Category & "&nm=" & search)
            End If



            If ProxyEnablerRuTr = True Then RequestPost.Proxy = New System.Net.WebProxy(ProxyServr, ProxyPort)
            RequestPost.Method = "POST"
            RequestPost.ContentType = "text/html; charset=windows-1251"
            RequestPost.Headers.Add("Cookie", CookiesRuTr)
            RequestPost.ContentType = "application/x-www-form-urlencoded"
            Dim myStream As System.IO.Stream = RequestPost.GetRequestStream
            Dim DataStr As String = "prev_new=0&prev_oop=0&o=10&s=2&pn=&nm=" & search
            Dim DataByte() As Byte = System.Text.Encoding.GetEncoding(1251).GetBytes(DataStr)
            myStream.Write(DataByte, 0, DataByte.Length)
            myStream.Close()

            Dim Response As System.Net.HttpWebResponse = RequestPost.GetResponse
            Dim dataStream As System.IO.Stream = Response.GetResponseStream
            Dim reader As New System.IO.StreamReader(dataStream, System.Text.Encoding.GetEncoding(1251))
            Dim ResponseFromServer As String = reader.ReadToEnd()


            Dim items As New System.Collections.Generic.List(Of Item)
            Dim Regex As New System.Text.RegularExpressions.Regex("(<tr class=""tCenter hl-tr"">).*?(</tr>)")
            Dim Result As System.Text.RegularExpressions.MatchCollection = Regex.Matches(ResponseFromServer.Replace(Microsoft.VisualBasic.vbLf, " "))

            If Result.Count > 0 Then

                For Each Match As System.Text.RegularExpressions.Match In Result
                    Dim Item As New Item
                    Regex = New System.Text.RegularExpressions.Regex("(?<=<a data-topic_id="").*?(?="")")
                    Dim LinkID As String = Regex.Matches(Match.Value)(0).Value
                    Item.Link = TrackerServer & "/forum/viewtopic.php?t=" & LinkID & ";PAGEFILMRUTR"
                    Regex = New System.Text.RegularExpressions.Regex("(?<=" & LinkID & """>).*?(?=</a>)")
                    Item.Name = Regex.Matches(Match.Value)(0).Value
                    Item.ImageLink = ICO_TorrentFile
                    Item.Description = GetDescriptionRuTr(Match.Value)
                    items.Add(Item)
                Next
            Else
                Return NonSearch(context)
            End If

            next_page_url = Nothing

            PlayList.IsIptv = "false"
            Return PlayListPlugPar(items, context)
        End Function

#End Region

#Region "RuTor"
        Dim TrackerServerRuTor As String = "http://mega-tor.org"

        Public Function GetTorrentPageRuTor(context As IPluginContext, ByVal URL As String) As PluginApi.Plugins.Playlist
            Dim items As New System.Collections.Generic.List(Of Item)()
            Dim WC As New System.Net.WebClient
            WC.Encoding = System.Text.Encoding.UTF8
            WC.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36")
            Dim ResponseFromServer As String = WC.DownloadString(URL).Replace(vbLf, " ")


            Dim Regex As New System.Text.RegularExpressions.Regex("(/download/).*?(?="">)")
            Dim TorrentPath As String = TrackerServerRuTor & Regex.Matches(ResponseFromServer)(0).Value
            Dim PlayListtoTorrent() As TorrentPlayList = GetFileList(TorrentPath)

            Dim Description As String = FormatDescriptionFileRuTor(ResponseFromServer)
            For Each PlayListItem As TorrentPlayList In PlayListtoTorrent

                Dim Item As New Item
                With Item
                    .Name = PlayListItem.Name
                    .ImageLink = PlayListItem.ImageLink
                    .Link = PlayListItem.Link
                    .Type = ItemType.FILE
                    .Description = Description
                End With
                items.Add(Item)
            Next

            Regex = New System.Text.RegularExpressions.Regex("(Связанные раздачи).*?(Файлы)")
            Dim Matches As System.Text.RegularExpressions.MatchCollection = Regex.Matches(ResponseFromServer)

            If Matches.Count > 0 Then

                Dim Item As New Item
                With Item
                    .Name = "<span style=""color:#C0DAE3"">" & "- СВЯЗАННЫЕ РАЗДАЧИ -" & "</span>"
                    .ImageLink = ICO_Pusto
                    .Type = ItemType.FILE
                End With
                items.Add(Item)

                Regex = New System.Text.RegularExpressions.Regex("(?<=<a href="").*?(?="")")
                Dim MatchesSearchNext As System.Text.RegularExpressions.MatchCollection = Regex.Matches(Matches(0).Value)
                Dim ItemSearchNext As New Item
                With ItemSearchNext
                    .ImageLink = ICO_Search2
                    .Name = "<span style=""color:#C0E3D3"">" & "Искать ещё похожие раздачи" & "</span>"
                    .Link = TrackerServerRuTor & MatchesSearchNext(MatchesSearchNext.Count - 1).Value & ";PAGERUTOR"
                End With



                Regex = New System.Text.RegularExpressions.Regex("(<a href=""magnet:).*?(</span></td></tr>)")

                Matches = Regex.Matches(Matches(0).Value)

                For Each Macth As System.Text.RegularExpressions.Match In Matches
                    Item = New Item

                    With Item
                        .ImageLink = ICO_TorrentFile2
                        Regex = New System.Text.RegularExpressions.Regex("(?<=<a href="").*?(?="">)")
                        .Link = TrackerServerRuTor & Regex.Matches(Macth.Value)(1).Value & ";PAGEFILMRUTOR"

                        Regex = New System.Text.RegularExpressions.Regex("(?<="">).*?(?=</a>)")
                        .Name = "<span style=""color:#E3D6C0"">" & Regex.Matches(Macth.Value)(1).Value & "</span>"

                        Regex = New System.Text.RegularExpressions.Regex("(<td align=""right"">).*?(</td>)")
                        Dim MatchSize As System.Text.RegularExpressions.MatchCollection = Regex.Matches(Macth.Value)
                        Dim SizeFile As String = MatchSize(MatchSize.Count - 1).Value
                        SizeFile = "Размер: " & SizeFile

                        Regex = New System.Text.RegularExpressions.Regex("(?<=alt=""S"" />&nbsp;).*?(?=<)")
                        Dim Seeders As String = "Seeders: " & Regex.Matches(Macth.Value)(0).Value


                        Regex = New System.Text.RegularExpressions.Regex("(?<=alt=""L"" /><span class=""red"">&nbsp;).*?(?=</span>)")
                        Dim Leechers As String = "Leechers: " & Regex.Matches(Macth.Value)(0).Value

                        .Description = "</div><span style=""color:#3090F0"">" & .Name & "</span><p><br>" & SizeFile & "<br><p>" & Seeders & "<br>" & Leechers

                    End With
                    items.Add(Item)
                Next
                items.Add(ItemSearchNext)

            End If





            PlayList.IsIptv = "false"
            Return PlayListPlugPar(items, context)
        End Function

        Function FormatDescriptionFileRuTor(ByVal HTML As String) As String


            Dim Title As String = Nothing
            Try
                Dim Regex As New System.Text.RegularExpressions.Regex("(?<=<h1>).*?(?=<h1>)")
                Title = Regex.Matches(HTML)(0).Value
            Catch ex As Exception
                Title = ex.Message
            End Try




            Try
                Dim Regex As New System.Text.RegularExpressions.Regex("(<table id=""details"">).*?(</table>)")
                HTML = Regex.Matches(HTML)(0).Value
            Catch ex As Exception

            End Try


            Dim SidsPirs As String = Nothing
            Try
                Dim Regex As New System.Text.RegularExpressions.Regex("(<td class=""header"">Раздают).*?(?=<td class=""header"" nowrap=""nowrap"">Добавить)")
                SidsPirs = Regex.Matches(HTML)(0).Value.Replace("</td><td>", ":(</td><td>").Replace("</td></tr>", "</td></tr>) ")
            Catch ex As Exception
                SidsPirs = ex.Message
            End Try

            Dim ImagePath As String = Nothing
            Try
                Dim Regex As New System.Text.RegularExpressions.Regex("(?<=<img src="").*?(?="")")
                ImagePath = "http://" & IPAdress & ":8027/proxym3u8B" & Base64Encode(Regex.Matches(HTML)(1).Value & "OPT:ContentType--image/jpegOPEND:/") & "/"
            Catch ex As Exception
            End Try


            Dim InfoFile As String = HTML

            InfoFile = replacetags(InfoFile).Replace("<br /><br />", "")
            Title = replacetags(Title)



            Return "<div id=""poster"" style=""float:left;padding:4px;        background-color:#EEEEEE;margin:0px 13px 1px 0px;"">" & "<img src=""" & ImagePath & """ style=""width:180px;float:left;"" /></div><span style=""color:#3090F0"">" & Title & "</span><br><font face=""Arial Narrow"" size=""4""><span style=""color:#70A4A3"">" & SidsPirs & "</font></span>" & InfoFile

        End Function


        Public Function GetPAGERUTOR(context As IPluginContext, ByVal URL As String) As PluginApi.Plugins.Playlist

            Dim items As New System.Collections.Generic.List(Of Item)()
            Dim WC As New System.Net.WebClient
            WC.Encoding = System.Text.Encoding.UTF8
            WC.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36")
            Dim ResponseFromServer As String = WC.DownloadString(URL).Replace(vbLf, " ")

            Dim Regex As New System.Text.RegularExpressions.Regex("(<a href=""magnet).*?(</span></td></tr>)")
            Dim Matches As System.Text.RegularExpressions.MatchCollection = Regex.Matches(ResponseFromServer)

            If Matches.Count = 0 Then
                Return NonSearch(context)
            End If


            For Each Macth As System.Text.RegularExpressions.Match In Matches
                Dim Item As New Item
                With Item
                    .ImageLink = ICO_TorrentFile

                    Regex = New System.Text.RegularExpressions.Regex("(?<=<a href="").*?(?="">)")
                    .Link = TrackerServerRuTor & Regex.Matches(Macth.Value)(1).Value & ";PAGEFILMRUTOR"

                    Regex = New System.Text.RegularExpressions.Regex("(?<="">).*?(?=</a>)")
                    .Name = Regex.Matches(Macth.Value)(1).Value

                    Regex = New System.Text.RegularExpressions.Regex("(<td align=""right"">).*?(</td>)")
                    Dim MatchSize As System.Text.RegularExpressions.MatchCollection = Regex.Matches(Macth.Value)
                    Dim SizeFile As String = MatchSize(MatchSize.Count - 1).Value
                    SizeFile = "Размер: " & SizeFile

                    Regex = New System.Text.RegularExpressions.Regex("(?<=alt=""S"" />&nbsp;).*?(?=<)")
                    Dim Seeders As String = "Seeders: " & Regex.Matches(Macth.Value)(0).Value


                    Regex = New System.Text.RegularExpressions.Regex("(?<=alt=""L"" /><span class=""red"">&nbsp;).*?(?=</span>)")
                    Dim Leechers As String = "Leechers: " & Regex.Matches(Macth.Value)(0).Value

                    .Description = "</div><span style=""color:#3090F0"">" & .Name & "</span><p><br>" & SizeFile & "<br><p>" & Seeders & "<br>" & Leechers

                End With
                items.Add(Item)
            Next

            Regex = New System.Text.RegularExpressions.Regex("(?<=&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="").*?(?=""><b>След)")
            Dim MatchNext As System.Text.RegularExpressions.MatchCollection = Regex.Matches(ResponseFromServer)
            If MatchNext.Count > 0 Then next_page_url = TrackerServerRuTor & MatchNext(0).Value & ";PAGERUTOR" Else next_page_url = Nothing

            PlayList.IsIptv = "false"
            Return PlayListPlugPar(items, context, next_page_url)
        End Function

        Public Function GetTopListRuTor(context As IPluginContext) As PluginApi.Plugins.Playlist
            Dim items As New System.Collections.Generic.List(Of Item)
            Dim Item As New Item


            With Item
                .Name = "Поиск"
                .Link = "Search_RuTor"
                .Type = ItemType.DIRECTORY
                .SearchOn = "Поик на RuTor"
                .ImageLink = ICO_Search
                .Description = "<html><font face=""Arial"" size=""5""><b>" & .Name & "</font></b><p><img src=""" & LOGO_TrackerRutor & """ />"
                items.Add(Item)
            End With

            Item = New Item
            With Item
                .Name = "Торренты за последние 24 часа"
                .Link = TrackerServerRuTor & "/;PAGERUTOR"
                .ImageLink = ICO_Folder
                .Description = "<html><font face=""Arial"" size=""5""><b>" & .Name & "</font></b><p><img src=""" & LOGO_TrackerRutor & """ />"
                items.Add(Item)
            End With

            Item = New Item
            With Item
                .Name = "Зарубежные фильмы"
                .Link = TrackerServerRuTor & "/browse/0/1/0/0/;PAGERUTOR"
                .ImageLink = ICO_Folder
                .Description = "<html><font face=""Arial"" size=""5""><b>" & .Name & "</font></b><p><img src=""" & LOGO_TrackerRutor & """ />"
                items.Add(Item)
            End With

            Item = New Item
            With Item
                .Name = "Наши фильмы"
                .Link = TrackerServerRuTor & "/browse/0/5/0/0/;PAGERUTOR"
                .ImageLink = ICO_Folder
                .Description = "<html><font face=""Arial"" size=""5""><b>" & .Name & "</font></b><p><img src=""" & LOGO_TrackerRutor & """ />"
                items.Add(Item)
            End With


            Item = New Item
            With Item
                .Name = "Научно-популярные фильмы"
                .Link = TrackerServerRuTor & "/browse/0/12/0/0/;PAGERUTOR"
                .ImageLink = ICO_Folder
                .Description = "<html><font face=""Arial"" size=""5""><b>" & .Name & "</font></b><p><img src=""" & LOGO_TrackerRutor & """ />"
                items.Add(Item)
            End With


            Item = New Item
            With Item
                .Name = "Сериалы"
                .Link = TrackerServerRuTor & "/browse/0/4/0/0/;PAGERUTOR"
                .ImageLink = ICO_Folder
                .Description = "<html><font face=""Arial"" size=""5""><b>" & .Name & "</font></b><p><img src=""" & LOGO_TrackerRutor & """ />"
                items.Add(Item)
            End With

            Item = New Item
            With Item
                .Name = "Телевизор"
                .Link = TrackerServerRuTor & "/browse/0/6/0/0/;PAGERUTOR"
                .ImageLink = ICO_Folder
                .Description = "<html><font face=""Arial"" size=""5""><b>" & .Name & "</font></b><p><img src=""" & LOGO_TrackerRutor & """ />"
                items.Add(Item)
            End With


            Item = New Item
            With Item
                .Name = "Мультипликация"
                .Link = TrackerServerRuTor & "/browse/0/7/0/0/;PAGERUTOR"
                .ImageLink = ICO_Folder
                .Description = "<html><font face=""Arial"" size=""5""><b>" & .Name & "</font></b><p><img src=""" & LOGO_TrackerRutor & """ />"
                items.Add(Item)
            End With

            Item = New Item
            With Item
                .Name = "Аниме"
                .Link = TrackerServerRuTor & "/browse/0/10/0/0/;PAGERUTOR"
                .ImageLink = ICO_Folder
                .Description = "<html><font face=""Arial"" size=""5""><b>" & .Name & "</font></b><p><img src=""" & LOGO_TrackerRutor & """ />"
                items.Add(Item)
            End With

            Item = New Item
            With Item
                .Name = "Музыка"
                .Link = TrackerServerRuTor & "/browse/0/2/0/0/;PAGERUTOR"
                .ImageLink = ICO_Folder
                .Description = "<html><font face=""Arial"" size=""5""><b>" & .Name & "</font></b><p><img src=""" & LOGO_TrackerRutor & """ />"
                items.Add(Item)
            End With

            Item = New Item
            With Item
                .Name = "Юмор"
                .Link = TrackerServerRuTor & "/browse/0/15/0/0/;PAGERUTOR"
                .ImageLink = ICO_Folder
                .Description = "<html><font face=""Arial"" size=""5""><b>" & .Name & "</font></b><p><img src=""" & LOGO_TrackerRutor & """ />"
                items.Add(Item)
            End With

            Item = New Item
            With Item
                .Name = "Спорт и Здоровье"
                .Link = TrackerServerRuTor & "/browse/0/13/0/0/;PAGERUTOR"
                .ImageLink = ICO_Folder
                .Description = "<html><font face=""Arial"" size=""5""><b>" & .Name & "</font></b><p><img src=""" & LOGO_TrackerRutor & """ />"
                items.Add(Item)
            End With


            PlayList.IsIptv = "false"
            Return PlayListPlugPar(items, context)
        End Function

#End Region

#Region "NNM Club"

        Dim CookiesNNM As String = "phpbb2mysql_4_data=a%3A2%3A%7Bs%3A11%3A%22autologinid%22%3Bs%3A32%3A%2296229c9a3405ae99cce1f3bc0cefce2e%22%3Bs%3A6%3A%22userid%22%3Bs%3A8%3A%2213287549%22%3B%7D"
        Public Function SearchListNNM(context As IPluginContext, ByVal search As String) As PluginApi.Plugins.Playlist

            Dim RequestPost As System.Net.HttpWebRequest = System.Net.HttpWebRequest.CreateHttp(TrackerServerNNM & "/forum/tracker.php")
            If ProxyEnablerNNM = True Then RequestPost.Proxy = New System.Net.WebProxy(ProxyServr, ProxyPort)
            RequestPost.Method = "POST"
            RequestPost.ContentType = "text/html; charset=windows-1251"
            RequestPost.Headers.Add("Cookie", CookiesNNM)
            RequestPost.ContentType = "application/x-www-form-urlencoded"
            Dim myStream As System.IO.Stream = RequestPost.GetRequestStream
            Dim DataStr As String = "prev_sd=1&prev_a=1&prev_my=0&prev_n=0&prev_shc=0&prev_shf=0&prev_sha=0&prev_shs=0&prev_shr=0&prev_sht=0&f%5B%5D=724&f%5B%5D=725&f%5B%5D=729&f%5B%5D=731&f%5B%5D=733&f%5B%5D=730&f%5B%5D=732&f%5B%5D=230&f%5B%5D=659&f%5B%5D=658&f%5B%5D=231&f%5B%5D=660&f%5B%5D=661&f%5B%5D=890&f%5B%5D=232&f%5B%5D=734&f%5B%5D=742&f%5B%5D=735&f%5B%5D=738&f%5B%5D=967&f%5B%5D=907&f%5B%5D=739&f%5B%5D=1109&f%5B%5D=736&f%5B%5D=737&f%5B%5D=898&f%5B%5D=935&f%5B%5D=871&f%5B%5D=973&f%5B%5D=960&f%5B%5D=1239&f%5B%5D=740&f%5B%5D=741&f%5B%5D=216&f%5B%5D=270&f%5B%5D=218&f%5B%5D=219&f%5B%5D=954&f%5B%5D=888&f%5B%5D=217&f%5B%5D=266&f%5B%5D=318&f%5B%5D=320&f%5B%5D=677&f%5B%5D=1177&f%5B%5D=319&f%5B%5D=678&f%5B%5D=885&f%5B%5D=908&f%5B%5D=909&f%5B%5D=910&f%5B%5D=911&f%5B%5D=912&f%5B%5D=220&f%5B%5D=221&f%5B%5D=222&f%5B%5D=882&f%5B%5D=889&f%5B%5D=224&f%5B%5D=225&f%5B%5D=226&f%5B%5D=227&f%5B%5D=891&f%5B%5D=682&f%5B%5D=694&f%5B%5D=884&f%5B%5D=1211&f%5B%5D=693&f%5B%5D=913&f%5B%5D=228&f%5B%5D=1150&f%5B%5D=254&f%5B%5D=321&f%5B%5D=255&f%5B%5D=906&f%5B%5D=256&f%5B%5D=257&f%5B%5D=258&f%5B%5D=883&f%5B%5D=955&f%5B%5D=905&f%5B%5D=271&f%5B%5D=1210&f%5B%5D=264&f%5B%5D=265&f%5B%5D=272&f%5B%5D=1262&f%5B%5D=1219&f%5B%5D=1221&f%5B%5D=1220&f%5B%5D=768&f%5B%5D=779&f%5B%5D=778&f%5B%5D=788&f%5B%5D=1288&f%5B%5D=787&f%5B%5D=1196&f%5B%5D=1141&f%5B%5D=777&f%5B%5D=786&f%5B%5D=803&f%5B%5D=776&f%5B%5D=785&f%5B%5D=1265&f%5B%5D=1289&f%5B%5D=774&f%5B%5D=775&f%5B%5D=1242&f%5B%5D=1140&f%5B%5D=782&f%5B%5D=773&f%5B%5D=1142&f%5B%5D=784&f%5B%5D=1195&f%5B%5D=772&f%5B%5D=771&f%5B%5D=783&f%5B%5D=1144&f%5B%5D=804&f%5B%5D=1290&f%5B%5D=770&f%5B%5D=922&f%5B%5D=780&f%5B%5D=781&f%5B%5D=769&f%5B%5D=799&f%5B%5D=800&f%5B%5D=791&f%5B%5D=798&f%5B%5D=797&f%5B%5D=790&f%5B%5D=793&f%5B%5D=794&f%5B%5D=789&f%5B%5D=796&f%5B%5D=792&f%5B%5D=795&f%5B%5D=713&f%5B%5D=706&f%5B%5D=577&f%5B%5D=894&f%5B%5D=578&f%5B%5D=580&f%5B%5D=579&f%5B%5D=953&f%5B%5D=581&f%5B%5D=806&f%5B%5D=714&f%5B%5D=761&f%5B%5D=809&f%5B%5D=924&f%5B%5D=812&f%5B%5D=576&f%5B%5D=590&f%5B%5D=591&f%5B%5D=588&f%5B%5D=823&f%5B%5D=589&f%5B%5D=598&f%5B%5D=652&f%5B%5D=596&f%5B%5D=600&f%5B%5D=819&f%5B%5D=599&f%5B%5D=956&f%5B%5D=959&f%5B%5D=597&f%5B%5D=594&f%5B%5D=593&f%5B%5D=595&f%5B%5D=582&f%5B%5D=587&f%5B%5D=583&f%5B%5D=584&f%5B%5D=586&f%5B%5D=585&f%5B%5D=614&f%5B%5D=603&f%5B%5D=1287&f%5B%5D=1282&f%5B%5D=1206&f%5B%5D=1200&f%5B%5D=1194&f%5B%5D=1062&f%5B%5D=974&f%5B%5D=609&f%5B%5D=1263&f%5B%5D=951&f%5B%5D=975&f%5B%5D=608&f%5B%5D=607&f%5B%5D=606&f%5B%5D=750&f%5B%5D=605&f%5B%5D=604&f%5B%5D=950&f%5B%5D=610&f%5B%5D=613&f%5B%5D=612&f%5B%5D=655&f%5B%5D=653&f%5B%5D=654&f%5B%5D=611&f%5B%5D=656&f%5B%5D=615&f%5B%5D=616&f%5B%5D=617&f%5B%5D=619&f%5B%5D=620&f%5B%5D=623&f%5B%5D=622&f%5B%5D=635&f%5B%5D=621&f%5B%5D=632&f%5B%5D=643&f%5B%5D=624&f%5B%5D=627&f%5B%5D=626&f%5B%5D=636&f%5B%5D=625&f%5B%5D=633&f%5B%5D=644&f%5B%5D=628&f%5B%5D=631&f%5B%5D=630&f%5B%5D=637&f%5B%5D=629&f%5B%5D=634&f%5B%5D=642&f%5B%5D=645&f%5B%5D=639&f%5B%5D=640&f%5B%5D=648&f%5B%5D=638&f%5B%5D=646&f%5B%5D=695&o=10&s=2&tm=-1&a=1&sd=1&ta=-1&sns=-1&sds=-1&nm=" & search & "&pn=&submit=Поиск"
            Dim DataByte() As Byte = System.Text.Encoding.GetEncoding(1251).GetBytes(DataStr)
            myStream.Write(DataByte, 0, DataByte.Length)
            myStream.Close()

            Dim Response As System.Net.HttpWebResponse = RequestPost.GetResponse
            Dim dataStream As System.IO.Stream = Response.GetResponseStream
            Dim reader As New System.IO.StreamReader(dataStream, System.Text.Encoding.GetEncoding(1251))
            Dim ResponseFromServer As String = reader.ReadToEnd()
            ' Dim ResponseFromServer As String = GetRequest(TrackerServerNNM & "/forum/tracker.php", CookiesNNM, ProxyEnablerNNM, "POST", DataStr)

            Dim items As New System.Collections.Generic.List(Of Item)
            Dim Regex As New System.Text.RegularExpressions.Regex("(<tr class=""prow).*?(</tr>)")
            Dim Result As System.Text.RegularExpressions.MatchCollection = Regex.Matches(ResponseFromServer.Replace(Microsoft.VisualBasic.vbLf, "   "))


            If Result.Count > 0 Then

                For Each Match As System.Text.RegularExpressions.Match In Result
                    Regex = New System.Text.RegularExpressions.Regex("(?<=href="").*?(?=&amp;)")
                    Dim Item As New Item
                    Item.Link = TrackerServerNNM & "/forum/" & Regex.Matches(Match.Value)(0).Value & ";PAGEFILMNNM"
                    Regex = New System.Text.RegularExpressions.Regex("(?<=""><b>).*?(?=</b>)")
                    Item.Name = Regex.Matches(Match.Value)(0).Value
                    Item.ImageLink = ICO_TorrentFile
                    Item.Description = GetDescriptionSearhNNM(Match.Value)
                    items.Add(Item)
                Next
            Else
                Return NonSearch(context)
            End If

            PlayList.IsIptv = "false"
            Return PlayListPlugPar(items, context)
        End Function

        Function NonSearch(context As IPluginContext, Optional ByVal Categor As Boolean = False) As PluginApi.Plugins.Playlist
            Dim items As New System.Collections.Generic.List(Of Item)
            Dim Item As New Item
            Item.Link = ""
            Item.ImageLink = ICO_Pusto
            If Categor = True Then
                Item.Name = "<span style=""color#F68648"">" & " - Здесь ничего нет - " & "</span>"
                Item.Description = "Нет информации для отображения"
            Else
                Item.Name = "<span style=""color#F68648"">" & " - Ничего не найдено - " & "</span>"
                Item.Description = "Поиск не дал результатов"
            End If


            items.Add(Item)

            PlayList.IsIptv = "false"
            Return PlayListPlugPar(items, context)
        End Function

        Function GetDescriptionSearhNNM(ByVal HTML As String) As String

            Dim NameFilm As String = Nothing
            Try
                Dim Regex As New System.Text.RegularExpressions.Regex("(?<=""><b>).*?(?=</b>)")
                NameFilm = Regex.Matches(HTML)(0).Value
            Catch ex As Exception
            End Try

            Dim SizeFile As String = Nothing
            Dim DobavlenFile As String = Nothing
            Try
                Dim Regex As New System.Text.RegularExpressions.Regex("(?<=</u>).*?(?=</td>)")
                SizeFile = "<p> Размер: <b>" & Regex.Matches(HTML)(0).Value & "</b>"
                DobavlenFile = "<p> Добавлен: <b>" & Regex.Matches(HTML)(1).Value.Replace("<br>", " ") & "</b>"
            Catch ex As Exception
            End Try

            Dim Seeders As String = Nothing
            Try
                Dim Regex As New System.Text.RegularExpressions.Regex("(?<=class=""seedmed"">).*?(?=</td>)")
                Seeders = "<p> Seeders: <b> " & Regex.Matches(HTML)(0).Value & "</b>"
            Catch ex As Exception
            End Try
            Dim Leechers As String = Nothing

            Try
                Dim Regex As New System.Text.RegularExpressions.Regex("(?<=ass=""leechmed"">).*?(?=</td>)")
                Leechers = "<p> Leechers: <b> " & Regex.Matches(HTML)(0).Value & "</b>"
            Catch ex As Exception
            End Try

            Return "<html><font face=""Arial"" size=""5""><b>" & NameFilm & "</font></b><p><font face=""Arial Narrow"" size=""4"">" & SizeFile & DobavlenFile & Seeders & Leechers & "</font></html>"
        End Function

        Public Function GetTopNNMClubList(context As IPluginContext) As PluginApi.Plugins.Playlist

            Dim items As New System.Collections.Generic.List(Of Item)
            Dim Item As New Item

            With Item
                .Name = "Поиск"
                .Link = "Search_NNM"
                .Type = ItemType.DIRECTORY
                .SearchOn = "Поиск видео на NNM-Club"
                .ImageLink = ICO_Search

                .Description = "<html><font face=""Arial"" size=""5""><b>" & .Name & "</font></b><p><img src=""" & LOGO_NoNaMeClub & """ />"


            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Name = "Новинки кино"
                .Link = TrackerServerNNM & "/forum/portal.php?c=10;PAGENNM"
                .ImageLink = ICO_Folder
                .Description = "<html><font face=""Arial"" size=""5""><b>" & .Name & "</font></b><p><img src=""" & LOGO_NoNaMeClub & """ />"

            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Name = "Наше кино"
                .Link = TrackerServerNNM & "/forum/portal.php?c=13;PAGENNM"
                .ImageLink = ICO_Folder
                .Description = "<html><font face=""Arial"" size=""5""><b>" & .Name & "</font></b><p><img src=""" & LOGO_NoNaMeClub & """ />"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Name = "Зарубежное кино"
                .Link = TrackerServerNNM & "/forum/portal.php?c=6;PAGENNM"
                .ImageLink = ICO_Folder
                .Description = "<html><font face=""Arial"" size=""5""><b>" & .Name & "</font></b><p><img src=""" & LOGO_NoNaMeClub & """ />"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Name = "HD (3D) Кино"
                .Link = TrackerServerNNM & "/forum/portal.php?c=11;PAGENNM"
                .ImageLink = ICO_Folder
                .Description = "<html><font face=""Arial"" size=""5""><b>" & .Name & "</font></b><p><img src=""" & LOGO_NoNaMeClub & """ />"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Name = "Артхаус"
                .Link = TrackerServerNNM & "/forum/portal.php?c=17;PAGENNM"
                .ImageLink = ICO_Folder
                .Description = "<html><font face=""Arial"" size=""5""><b>" & .Name & "</font></b><p><img src=""" & LOGO_NoNaMeClub & """ />"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Name = "Наши сериалы"
                .Link = TrackerServerNNM & "/forum/portal.php?c=4;PAGENNM"
                .ImageLink = ICO_Folder
                .Description = "<html><font face=""Arial"" size=""5""><b>" & .Name & "</font></b><p><img src=""" & LOGO_NoNaMeClub & """ />"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Name = "Зарубежные сериалы"
                .Link = TrackerServerNNM & "/forum/portal.php?c=3;PAGENNM"
                .ImageLink = ICO_Folder
                .Description = "<html><font face=""Arial"" size=""5""><b>" & .Name & "</font></b><p><img src=""" & LOGO_NoNaMeClub & """ />"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Name = "Театр, МузВидео, Разное"
                .Link = TrackerServerNNM & "/forum/portal.php?c=21;PAGENNM"
                .ImageLink = ICO_Folder
                .Description = "<html><font face=""Arial"" size=""5""><b>" & .Name & "</font></b><p><img src=""" & LOGO_NoNaMeClub & """ />"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Name = "Док. TV-бренды"
                .Link = TrackerServerNNM & "/forum/portal.php?c=22;PAGENNM"
                .ImageLink = ICO_Folder
                .Description = "<html><font face=""Arial"" size=""5""><b>" & .Name & "</font></b><p><img src=""" & LOGO_NoNaMeClub & """ />"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Name = "Док. и телепередачи"
                .Link = TrackerServerNNM & "/forum/portal.php?c=23;PAGENNM"
                .ImageLink = ICO_Folder
                .Description = "<html><font face=""Arial"" size=""5""><b>" & .Name & "</font></b><p><img src=""" & LOGO_NoNaMeClub & """ />"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Name = "Спорт и Юмор"
                .Link = TrackerServerNNM & "/forum/portal.php?c=24;PAGENNM"
                .ImageLink = ICO_Folder
                .Description = "<html><font face=""Arial"" size=""5""><b>" & .Name & "</font></b><p><img src=""" & LOGO_NoNaMeClub & """ />"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Name = "Аниме и Манга"
                .Link = TrackerServerNNM & "/forum/portal.php?c=1;PAGENNM"
                .ImageLink = ICO_Folder
                .Description = "<html><font face=""Arial"" size=""5""><b>" & .Name & "</font></b><p><img src=""" & LOGO_NoNaMeClub & """ />"
            End With
            items.Add(Item)
            PlayList.IsIptv = "false"
            Return PlayListPlugPar(items, context)
        End Function

        Function GetPAGENNM(ByVal context As IPluginContext, ByVal URL As String) As PluginApi.Plugins.Playlist

            Dim items As New System.Collections.Generic.List(Of Item)()

            Try
                Dim RequestGet As System.Net.HttpWebRequest = System.Net.HttpWebRequest.CreateHttp(URL)
                If ProxyEnablerNNM = True Then
                    RequestGet.Proxy = New System.Net.WebProxy(ProxyServr, ProxyPort)
                End If
                RequestGet.Method = "GET"
                RequestGet.ContentType = "text/html; charset=windows-1251"
                RequestGet.Headers.Add("Cookie", CookiesNNM)

                Dim Response2 As System.Net.HttpWebResponse = RequestGet.GetResponse()
                Dim dataStream As System.IO.Stream = Response2.GetResponseStream()
                Dim reader As New System.IO.StreamReader(dataStream, System.Text.Encoding.GetEncoding(1251))
                Dim responseFromServer As String = reader.ReadToEnd()
                '  Dim responseFromServer As String = GetRequest(URL, CookiesNNM, ProxyEnablerNNM)

                Dim Regex As New System.Text.RegularExpressions.Regex("(<td class=""pcatHead""><img class=""picon"").*?("" /></span>)")


                For Each MAtch As System.Text.RegularExpressions.Match In Regex.Matches(responseFromServer.Replace(Constants.vbLf, "   "))
                    Regex = New System.Text.RegularExpressions.Regex("(?<=title="").*?(?="">)")
                    Dim Item As New Item()
                    Item.Name = Regex.Matches(MAtch.Value)(1).Value

                    Regex = New System.Text.RegularExpressions.Regex("(?<=<var class=""portalImg"" title="").*?(?="">)")
                    Item.ImageLink = Regex.Matches(MAtch.Value)(0).Value

                    Item.ImageLink = "http://" & IPAdress & ":8027/proxym3u8B" & Base64Encode(Item.ImageLink & "OPT:ContentType--image/jpegOPEND:/") & "/"

                    Regex = New System.Text.RegularExpressions.Regex("(?<=<a class=""pgenmed"" href="").*?(?=&)")
                    Item.Link = TrackerServerNNM & "/forum/" & Regex.Matches(MAtch.Value)(0).Value & ";PAGEFILMNNM"

                    Regex = New System.Text.RegularExpressions.Regex("(?<=<a class=""pgenmed"" href="").*?(?=&)")
                    Item.Description = FormatDescriptionNNM(MAtch.Value, Item.ImageLink)


                    items.Add(Item)
                Next MAtch

                Regex = New System.Text.RegularExpressions.Regex("(?<=&nbsp;&nbsp;<a href="").*?(?=sid=)")
                Dim Rzult As System.Text.RegularExpressions.MatchCollection = Regex.Matches(responseFromServer)

                next_page_url = TrackerServerNNM & "/forum/" & Rzult(Rzult.Count - 1).Value.Replace("amp;", "") & ";PAGENNM"

            Catch ex As Exception
                Dim Item As New Item()
                With Item
                    .Name = "ERROR"
                    .ImageLink = ICO_Error
                    .Description = ex.Message
                    .Link = ""
                    items.Add(Item)
                End With
            End Try

            PlayList.IsIptv = "false"
            Return PlayListPlugPar(items, context, next_page_url)

        End Function

        Function Base64Encode(ByVal plainText As String) As String
            Dim plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText)
            Return System.Convert.ToBase64String(plainTextBytes)
        End Function

        Public Function GetTorrentPAGENNM(ByVal context As IPluginContext, ByVal URL As String) As PluginApi.Plugins.Playlist
            Dim RequestGet As System.Net.HttpWebRequest = Net.HttpWebRequest.CreateHttp(URL)
            If ProxyEnablerNNM = True Then RequestGet.Proxy = New System.Net.WebProxy(ProxyServr, ProxyPort)
            RequestGet.Method = "GET"
            RequestGet.Headers.Add("Cookie", CookiesNNM)

            Dim Response As Net.HttpWebResponse = RequestGet.GetResponse
            Dim DataStream As System.IO.Stream = Response.GetResponseStream()
            Dim Reader As New System.IO.StreamReader(DataStream, Text.Encoding.GetEncoding(1251))
            Dim ResponseFromServer As String = Reader.ReadToEnd
            Reader.Close()
            DataStream.Close()
            Response.Close()
            ' Dim ResponseFromServer As String = GetRequest(URL, CookiesNNM, ProxyEnablerNNM)

            Dim TorrentPath As String = Nothing
            Try
                Dim Regex As New System.Text.RegularExpressions.Regex("(?<=<span class=""genmed""><b><a href="").*?(?=&amp;)")
                TorrentPath = TrackerServerNNM & "/forum/" & Regex.Matches(ResponseFromServer)(0).Value
            Catch ex As Exception
                TorrentPath = ex.Message
            End Try



            Dim Title As String
            Try
                Dim Regex As New System.Text.RegularExpressions.Regex("(?<=<span style=""font-weight: bold"">).*?(?=</span>)")
                Title = Regex.Matches(ResponseFromServer)(0).Value
            Catch ex As Exception
                Title = ex.Message
            End Try

            Dim RequestTorrent As System.Net.HttpWebRequest = Net.HttpWebRequest.CreateHttp(TorrentPath)
            If ProxyEnablerNNM = True Then RequestTorrent.Proxy = New System.Net.WebProxy(ProxyServr, ProxyPort)
            RequestTorrent.Method = "GET"
            RequestTorrent.Headers.Add("Cookie", CookiesNNM)

            Response = RequestTorrent.GetResponse
            DataStream = Response.GetResponseStream()
            Reader = New System.IO.StreamReader(DataStream, System.Text.Encoding.GetEncoding(1251))
            Dim FileTorrent As String = Reader.ReadToEnd
            ' Dim FileTorrent As String = GetRequest(TorrentPath, CookiesNNM, ProxyEnablerNNM)
            System.IO.File.WriteAllText(System.IO.Path.GetTempPath & "TorrentTemp.torrent", FileTorrent, System.Text.Encoding.GetEncoding(1251))
            Reader.Close()
            DataStream.Close()
            Response.Close()


            Dim items As New System.Collections.Generic.List(Of Item)



            Try
                Dim Description As String = FormatDescriptionFileNNM(ResponseFromServer)
                Dim PlayListtoTorrent() As TorrentPlayList = GetFileList(System.IO.Path.GetTempPath & "TorrentTemp.torrent")

                For Each PlayListItem As TorrentPlayList In PlayListtoTorrent
                    Dim Item As New Item
                    With Item
                        .Name = PlayListItem.Name
                        .ImageLink = PlayListItem.ImageLink
                        .Link = PlayListItem.Link
                        .Type = ItemType.FILE
                        .Description = Description
                    End With
                    items.Add(Item)
                Next

            Catch ex As Exception
                Dim Item As New Item
                With Item
                    .Name = "ERROR"
                    .Link = ""
                    .Type = ItemType.FILE
                    .Description = ex.Message
                    .ImageLink = ICO_Error
                End With
                items.Add(Item)
            End Try
            PlayList.IsIptv = "false"
            Return PlayListPlugPar(items, context)
        End Function

        Function FormatDescriptionFileNNM(ByVal HTML As String) As String
            HTML = HTML.Replace(Constants.vbLf, "   ")

            Dim Title As String = Nothing
            Dim Regex As New System.Text.RegularExpressions.Regex("(<span style=""text-align:).*?(</span>)")
            Try
                Title = Regex.Matches(HTML)(0).Value
            Catch ex As Exception
                Title = ex.Message
            End Try


            Dim SidsPirs As String = Nothing
            Try
                Regex = New System.Text.RegularExpressions.Regex("(<table cellspacing=""0"").*?(</table>)")
                SidsPirs = Regex.Matches(HTML)(0).Value
            Catch ex As Exception
                SidsPirs = ex.Message
            End Try


            Dim ImagePath As String = Nothing
            Try
                Regex = New System.Text.RegularExpressions.Regex("(?<=<var class=""postImg postImgAligned img-right"" title="").*?(?="">)")
                ImagePath = Regex.Matches(HTML)(0).Value
                ImagePath = "http://" & IPAdress & ":8027/proxym3u8B" & Base64Encode(ImagePath & "OPT:ContentType--image/jpegOPEND:/") & "/"

            Catch ex As Exception

            End Try


            Dim InfoFile As String = Nothing
            Try
                Regex = New System.Text.RegularExpressions.Regex("(<div class=""kpi"">).*(?=<div class=""spoiler-wrap"">)")
                InfoFile = Regex.Matches(HTML)(0).Value
            Catch e As Exception
                Try
                    Regex = New System.Text.RegularExpressions.Regex("(<br /><br /><span style=""font-weight: bold"">).*?(<br />)")

                    Dim Match As System.Text.RegularExpressions.MatchCollection = Regex.Matches(HTML)
                    For I As Integer = 1 To Match.Count - 1
                        InfoFile = InfoFile & Match(I).Value
                    Next I

                Catch ex As Exception
                    InfoFile = ex.Message
                End Try

            End Try
            Dim Opisanie As String = Nothing

            Try
                Regex = New System.Text.RegularExpressions.Regex("(<span style=""font-weight: bold"">Описание:</span><br />).*?(?=<div)")
                Opisanie = Regex.Matches(HTML)(0).Value
            Catch ex As Exception
                Opisanie = ex.Message
            End Try




            SidsPirs = replacetags(SidsPirs)
            InfoFile = replacetags(InfoFile)
            Title = replacetags(Title)
            Opisanie = replacetags(Opisanie)

            Return "<div id=""poster"" style=""float:left;padding:4px;        background-color:#EEEEEE;margin:0px 13px 1px 0px;"">" & "<img src=""" & ImagePath & """ style=""width:180px;float:left;"" /></div><span style=""color:#3090F0"">" & Title & "</span><br>" & SidsPirs & "<br>" & Opisanie & "<span style=""color:#3090F0"">Информация</span><br>" & InfoFile
        End Function

        Public Function replacetags(ByVal s As String) As String
            Try
                Dim rgx As New System.Text.RegularExpressions.Regex("<[^b].*?>")
                s = rgx.Replace(s, "").Replace("<b>", "")
                Return s
            Catch ex As Exception

            End Try
            Return Nothing
        End Function

        Function FormatDescriptionNNM(ByVal HTML As String, ByVal ImagePath As String) As String
            HTML = HTML.Replace(Constants.vbLf, "   ")

            Dim Title As String = Nothing
            Try
                Dim Regex As New System.Text.RegularExpressions.Regex("(?<=title="").*?(?="")")
                Title = Regex.Matches(HTML)(1).Value
            Catch ex As Exception
                Title = ex.Message
            End Try


            Dim InfoFile As String = Nothing
            Try
                Dim Regex As New System.Text.RegularExpressions.Regex("(<img class=""tit-b pims"").*(?=<span id=)")
                InfoFile = Regex.Matches(HTML)(0).Value
            Catch ex As Exception
                InfoFile = ex.Message
            End Try


            Dim InfoFilms As String = Nothing
            Try
                Dim Regex As New System.Text.RegularExpressions.Regex("(</var></a>).*?(<br />)")
                InfoFilms = Regex.Matches(HTML)(0).Value
            Catch ex As Exception
                InfoFilms = ex.Message
            End Try

            Dim InfoPro As String = Nothing
            Try
                Dim Regex As New System.Text.RegularExpressions.Regex("(<br /><b>).*(</span></td> )")
                InfoPro = Regex.Matches(HTML)(0).Value
            Catch ex As Exception
                InfoPro = ex.Message
            End Try
            Return "<div id=""poster"" style=""float:left;padding:4px;        background-color:#EEEEEE;margin:0px 13px 1px 0px;"">" & "<img src=""" & ImagePath & """ style=""width:180px;float:left;"" /></div><span style=""color:#3090F0"">" & Title & "</span><br>" & InfoFile & InfoPro & "<br><span style=""color:#3090F0"">Описание: </span>" & InfoFilms
        End Function

#End Region

#Region "TorrentTV"

        'Поиск ТВ
        Public Function GetPageSearchStreamTV(context As IPluginContext, ByVal URL As String, Optional ByVal Page As Integer = 0) As PluginApi.Plugins.Playlist
            Dim Items As New System.Collections.Generic.List(Of Item)
            Dim ReGexInfioHash As New System.Text.RegularExpressions.Regex("(?<=""infohash"":"").*?(?="")")
            Dim ReGexName As New System.Text.RegularExpressions.Regex("(?<=""name"":"").*?(?="")")
            Dim Str As String = ReCoder(Requesters("https://search.acestream.net/?method=search&api_version=1.0&api_key=test_api_key&query=" & URL & "&page_size=50&page=" & Page))

            If ReGexInfioHash.IsMatch(Str) = True Then
                Dim InfoHashs, Names As System.Text.RegularExpressions.MatchCollection
                InfoHashs = ReGexInfioHash.Matches(Str)
                Names = ReGexName.Matches(Str)
                For I As Integer = 0 To InfoHashs.Count - 1
                    Dim Item As New Item
                    With Item
                        .Name = Names(I).Value
                        .Link = ("http://" & IPAdress & ":" & PortAce & "/ace/manifest.m3u8?&infohash=" & InfoHashs(I).Value)
                        .Type = ItemType.FILE
                        .ImageLink = ICO_VideoFile
                        .Description = "<html><font face=""Arial"" size=""5""><b>" & .Name & "</font></b>"
                    End With
                    Items.Add(Item)
                Next
            End If


            next_page_url = Page + 1 & ";" & URL & ";SEARCHTV"
            PlayList.IsIptv = "true"
            Return PlayListPlugPar(Items, context, next_page_url)
        End Function
        Function Requesters(ByVal Path As String) As String
            Dim WC As New Net.WebClient
            ' WC.Proxy = New System.Net.WebProxy(ProxyServr, ProxyPort)
            WC.Headers.Item(Net.HttpRequestHeader.UserAgent) = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3071.115 Safari/537.36"
            WC.Encoding = System.Text.Encoding.UTF8

            Dim STR As String = WC.DownloadString(Path)
            Return STR
        End Function
        'ТВ


        Public Function GetTV(ByVal context As IPluginContext) As PluginApi.Plugins.Playlist
            Dim items = New Collections.Generic.List(Of Item)
            Dim Item As New Item
            With Item
                .Name = "Поиск"
                .Link = "searchtvtoace"
                .Type = ItemType.DIRECTORY
                .SearchOn = "Поиск"
                .ImageLink = ICO_Search
                .Description = "<html><font face=""Arial"" size=""5""><b>ПОИСК ТВ ТРАНСЛЯЦИЙ...</font></b><p><img width=""100%""  src=""https://tvfeed.in/img/acestream-main.jpg"" />"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Name = "Torrent TV"
                .Type = ItemType.DIRECTORY
                .Link = "torrenttv"
                .ImageLink = "https://cs5-2.4pda.to/7342878.png"
                .Description = "<html><img src="" http://torrent-tv.ru/images/logo.png""></html><p>"

            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Name = "AceStream.Net TV"
                .Type = ItemType.DIRECTORY
                .Link = "acestreamnettv"
                .ImageLink = "http://lh3.googleusercontent.com/Vh58wclC2o-4lMfibmBqiuhY2j9vBZbxO4bTJCZtjZ1jeLNe0fgoYpy1888fgGa9EL0=w300"
                .Description = "<html><img src=""http://static.acestream.net/sites/acestream/img/ACE-logo.png""></html><p>"

            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Type = ItemType.DIRECTORY
                .Name = "ALLFON-TV"
                .Link = "allfon.all.iproxy"
                .ImageLink = "http://allfon-tv.pro/css/images/favicon.png"
                .Description = "<html><img src=""http://static.acestream.net/sites/acestream/img/ACE-logo.png""></html><p>"
            End With
            items.Add(Item)

            PlayList.IsIptv = "False"
            Return PlayListPlugPar(items, context)
        End Function
        Public Function GetAceStreamNetTV(ByVal context As IPluginContext) As PluginApi.Plugins.Playlist
            Dim items = New Collections.Generic.List(Of Item)
            Dim Item As New Item

            Item = New Item
            With Item
                .Type = ItemType.DIRECTORY
                .Name = "РАЗВЛЕКАТЕЛЬНЫЕ"
                .Link = "ace.entertaining.iproxy"
                .ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Type = ItemType.DIRECTORY
                .Name = "ДЕТСКИЕ"
                .Link = "ace.kids.iproxy"
                .ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Type = ItemType.DIRECTORY
                .Name = "ОБРАЗОВАТЕЛЬНЫЕ"
                .Link = "ace.educational.iproxy"
                .ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Type = ItemType.DIRECTORY
                .Name = "ФИЛЬМЫ"
                .Link = "ace.movies.iproxy"
                .ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Type = ItemType.DIRECTORY
                .Name = "СЕРИАЛЫ"
                .Link = "ace.series.iproxy"
                .ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Type = ItemType.DIRECTORY
                .Name = "ИНФОРМАЦИОННЫЕ"
                .Link = "ace.informational.iproxy"
                .ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Type = ItemType.DIRECTORY
                .Name = "СПОРТИВНЫЕ"
                .Link = "ace.sport.iproxy"
                .ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Type = ItemType.DIRECTORY
                .Name = "МУЗЫКАЛЬНЫЕ"
                .Link = "ace.music.iproxy"
                .ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Type = ItemType.DIRECTORY
                .Name = "РЕГИОНАЛЬНЫЕ"
                .Link = "ace.regional.iproxy"
                .ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Type = ItemType.DIRECTORY
                .Name = "ЭРОТИКА 18+"
                .Link = "ace.erotic_18_plus.iproxy"
                .ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Type = ItemType.DIRECTORY
                .Name = "ВСЕ КАНАЛЫ"
                .Link = "ace.all.iproxy"
                .ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            End With
            items.Add(Item)


            PlayList.IsIptv = "false"
            Return PlayListPlugPar(items, context)
        End Function

        Public Function GetTorrentTV(ByVal context As IPluginContext) As PluginApi.Plugins.Playlist
            Dim items = New Collections.Generic.List(Of Item)
            Dim Item As New Item

            With Item
                .Type = ItemType.DIRECTORY
                .Name = "РАЗВЛЕКАТЕЛЬНЫЕ"
                .Link = "ttv.ent.iproxy"
                .ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Type = ItemType.DIRECTORY
                .Name = "ДЕТСКИЕ"
                .Link = "ttv.child.iproxy"
                .ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Type = ItemType.DIRECTORY
                .Name = "ПОЗНАВАТЕЛЬНЫЕ"
                .Link = "ttv.discover.iproxy"
                .ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Type = ItemType.DIRECTORY
                .Name = "HD"
                .Link = "ttv.HD.iproxy"
                .ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Type = ItemType.DIRECTORY
                .Name = "ОБЩИЕ"
                .Link = "ttv.common.iproxy"
                .ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Type = ItemType.DIRECTORY
                .Name = "ФИЛЬМЫ"
                .Link = "ttv.film.iproxy"
                .ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Type = ItemType.DIRECTORY
                .Name = "МУЖСКИЕ"
                .Link = "ttv.man.iproxy"
                .ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Type = ItemType.DIRECTORY
                .Name = "МУЗЫКАЛЬНЫЕ"
                .Link = "ttv.music.iproxy"
                .ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Type = ItemType.DIRECTORY
                .Name = "ИНФОРМАЦИОННЫЕ"
                .Link = "ttv.news.iproxy"
                .ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Type = ItemType.DIRECTORY
                .Name = "РЕГИОНАЛЬНЫЕ"
                .Link = "ttv.region.iproxy"
                .ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Type = ItemType.DIRECTORY
                .Name = "РЕЛИГИОЗНЫЕ"
                .Link = "ttv.relig.iproxy"
                .ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Type = ItemType.DIRECTORY
                .Name = "СПОРТИВНЫЕ"
                .Link = "ttv.sport.iproxy"
                .ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Type = ItemType.DIRECTORY
                .Name = "ЭРОТИКА 18+"
                .Link = "ttv.porn.iproxy"
                .ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Type = ItemType.DIRECTORY
                .Name = "ВСЕ КАНАЛЫ"
                .Link = "ttv.all.iproxy"
                .ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            End With
            items.Add(Item)
            PlayList.IsIptv = "false"
            Return PlayListPlugPar(items, context)
        End Function

        Public Function LastModifiedPlayList(ByVal NamePlayList As String, ByVal context As IPluginContext) As PluginApi.Plugins.Playlist
            PlayList.IsIptv = "true"
            Dim PathFileUpdateTime As String = System.IO.Path.GetTempPath() & NamePlayList & ".UpdateTime.tmp"
            Dim PathFilePlayList As String = System.IO.Path.GetTempPath() & NamePlayList & ".PlayList.m3u8"

            Dim request As System.Net.HttpWebRequest = System.Net.HttpWebRequest.CreateHttp("http://pomoyka.lib.emergate.net/trash/ttv-list/" & NamePlayList & ".m3u?ip=" & IPAdress & ":" & PortAce)
            request.Method = "HEAD"
            request.ContentType = "text/html"
            request.KeepAlive = True
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8"
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36"
            request.Host = "pomoyka.lib.emergate.net"
            Dim response As System.Net.HttpWebResponse = CType(request.GetResponse(), System.Net.HttpWebResponse)
            Dim responHeader = response.GetResponseHeader("Last-Modified")
            response.Close()

            Dim WC As New System.Net.WebClient()
            WC.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36")
            WC.Encoding = System.Text.Encoding.UTF8

            Dim items As New System.Collections.Generic.List(Of Item)()
            Dim Item As New Item()

            If (System.IO.File.Exists(PathFileUpdateTime) AndAlso System.IO.File.Exists(PathFilePlayList)) = False Then
                UpdatePlayList(NamePlayList, PathFilePlayList, PathFileUpdateTime, responHeader)
                Return toSource(WC.DownloadString(PathFilePlayList), context)
            End If

            If responHeader IsNot System.IO.File.ReadAllText(PathFileUpdateTime) Then
                UpdatePlayList(NamePlayList, PathFilePlayList, PathFileUpdateTime, responHeader)

                Return toSource(WC.DownloadString(PathFilePlayList), context)
            End If
            Return toSource(WC.DownloadString(PathFilePlayList), context)

        End Function

        Sub UpdatePlayList(ByVal NamePlayList As String, ByVal PathFilePlayList As String, ByVal PathFileUpdateTime As String, ByVal LastModified As String)
            Try
                System.IO.File.WriteAllText(PathFileUpdateTime, LastModified)
                Dim WC As New System.Net.WebClient
                WC.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36")
                WC.Encoding = System.Text.Encoding.UTF8

                Dim PlayList As String = WC.DownloadString("http://pomoyka.lib.emergate.net/trash/ttv-list/" & NamePlayList & ".m3u?ip=" & IPAdress & ":" & PortAce)
                System.IO.File.WriteAllText(PathFilePlayList, PlayList.Replace("(Эротика)", "(Эротика 18+)"))
                WC.DownloadFile("http://pomoyka.lib.emergate.net/trash/ttv-list/MyTraf.php", System.IO.Path.GetTempPath & "MyTraf.tmp")
                WC.Dispose()
            Catch ex As Exception
            End Try
        End Sub

#End Region

#Region "AceTorrent"
        Dim PortAce As String = "6878"
        Dim AceProxEnabl As Boolean
        Structure TorrentPlayList
            Dim IDX As String
            Dim Name As String
            Dim Link As String
            Dim Description As String
            Dim ImageLink As String
        End Structure


        Function GetID(ByVal PathTorrent As String) As String
            Dim WC As New System.Net.WebClient
            WC.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/57.0.2987.133 Safari/537.36")
            WC.Encoding = System.Text.Encoding.UTF8
            Dim FileTorrent() As Byte = WC.DownloadData(PathTorrent)

            Dim FileTorrentString As String = System.Convert.ToBase64String(FileTorrent)
            FileTorrent = System.Text.Encoding.Default.GetBytes(FileTorrentString)

            Dim request As System.Net.HttpWebRequest = System.Net.HttpWebRequest.Create("http://api.torrentstream.net/upload/raw")
            request.Method = "POST"
            request.ContentType = "application/octet-stream\r\n"
            request.ContentLength = FileTorrent.Length
            Dim dataStream As System.IO.Stream = request.GetRequestStream
            dataStream.Write(FileTorrent, 0, FileTorrent.Length)
            dataStream.Close()

            Dim response As System.Net.WebResponse = request.GetResponse()
            dataStream = response.GetResponseStream()
            Dim reader As New System.IO.StreamReader(dataStream)
            Dim responseFromServer As String = reader.ReadToEnd()

            'MsgBox(responseFromServer)
            Dim responseSplit() As String = responseFromServer.Split("""")
            Dim ID As String = responseSplit(3)
            Return ID
        End Function

        Function ReCoder(ByVal Str As String)
            Dim CodeZnaki() As String = {"\U0430", "\U0431", "\U0432", "\U0433", "\U0434", "\U0435", "\U0451", "\U0436", "\U0437", "\U0438", "\U0439", "\U043A", "\U043B", "\U043C", "\U043D", "\U043E", "\U043F", "\U0440", "\U0441", "\U0442", "\U0443",
                              "\U0444", "\U0445", "\U0446", "\U0447", "\U0448", "\U0449", "\U044A", "\U044B", "\U044C", "\U044D", "\U044E", "\U044F", "\U0410", "\U0411", "\U0412", "\U0413", "\U0414", "\U0415", "\U0401", "\U0416", "\U0417", "\U0418", "\U0419", "\U041A",
                              "\U041B", "\U041C", "\U041D", "\U041E", "\U041F", "\U0420", "\U0421", "\U0422", "\U0423", "\U0424", "\U0425", "\U0426", "\U0427", "\U0428", "\U0429", "\U042A", "\U042B", "\U042C", "\U042D", "\U042E", "\U042F", "\U00AB", "\U00BB", "U2116"}
            Dim DecodeZnaki() As String = {"а", "б", "в", "г", "д", "е", "ё", "ж", "з", "и", "й", "к", "л", "м", "н", "о", "п", "р", "с", "т", "у", "ф", "х", "ц", "ч", "ш", "щ", "ъ", "ы", "ь", "э", "ю", "я",
                    "А", "Б", "В", "Г", "Д", "Е", "Ё", "Ж", "З", "И", "Й", "К", "Л", "М", "Н", "О", "П", "Р", "С", "Т", "У", "Ф", "Х", "Ц", "Ч", "Ш", "Щ", "Ъ", "Ы", "Ь", "Э", "Ю", "Я", "«", "»", "№"}

            ' MsgBox(AceMadiaInfo)
            For I As Integer = 0 To 68
                Str = Microsoft.VisualBasic.Strings.Replace(Str, Microsoft.VisualBasic.Strings.LCase(CodeZnaki(I)), DecodeZnaki(I))
            Next
            Return Str
        End Function

        Function GetFileList(ByVal PathTorrent As String) As TorrentPlayList()

            Dim WC As New System.Net.WebClient
            WC.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/57.0.2987.133 Safari/537.36")
            WC.Encoding = System.Text.UTF8Encoding.UTF8

            Dim ID As String = GetID(PathTorrent)
            Dim PlayListTorrent() As TorrentPlayList = Nothing
            Dim AceMadiaInfo As String

            Select Case FunctionsGetTorrentPlayList
                Case "GetFileListJSON"
GetFileListJSON:
                    AceMadiaInfo = ReCoder(WC.DownloadString("http://" & IPAdress & ":" & PortAce & "/server/api?method=get_media_files&content_id=" & ID))
                    WC.Dispose()


                    Dim PlayListJson As String = AceMadiaInfo
                    PlayListJson = Microsoft.VisualBasic.Strings.Replace(PlayListJson, ",", Nothing)
                    PlayListJson = Microsoft.VisualBasic.Strings.Replace(PlayListJson, ":", Nothing)
                    PlayListJson = Microsoft.VisualBasic.Strings.Replace(PlayListJson, "}", Nothing)
                    PlayListJson = Microsoft.VisualBasic.Strings.Replace(PlayListJson, "{", Nothing)
                    PlayListJson = Microsoft.VisualBasic.Strings.Replace(PlayListJson, "result", Nothing)
                    PlayListJson = Microsoft.VisualBasic.Strings.Replace(PlayListJson, "error", Nothing)
                    PlayListJson = Microsoft.VisualBasic.Strings.Replace(PlayListJson, "null", Nothing)
                    PlayListJson = Microsoft.VisualBasic.Strings.Replace(PlayListJson, """""", """")
                    PlayListJson = Microsoft.VisualBasic.Strings.Replace(PlayListJson, """ """, """")

                    Dim ListSplit() As String = PlayListJson.Split("""")
                    ReDim PlayListTorrent((ListSplit.Length / 2) - 2)
                    Dim N As Integer
                    For I As Integer = 1 To ListSplit.Length - 2
                        PlayListTorrent(N).IDX = ListSplit(I)
                        PlayListTorrent(N).Name = ListSplit(I + 1)
                        PlayListTorrent(N).Link = "http://" & IPAdress & ":" & PortAce & "/ace/getstream?id=" & ID & "&_idx=" & PlayListTorrent(N).IDX
                        PlayListTorrent(N).ImageLink = IconFile(PlayListTorrent(N).Name)
                        I += 1
                        N += 1
                    Next


                Case "GetFileListM3U"
                    AceMadiaInfo = WC.DownloadString("http://" & IPAdress & ":" & PortAce & "/ace/manifest.m3u8?id=" & ID & "&format=json&use_api_events=1&use_stop_notifications=1")
                    'MsgBox(AceMadiaInfo)
                    If AceMadiaInfo.StartsWith("{""response"": {""event_url"": """) = True Then
                        GoTo GetFileListJSON
                    End If
                    If AceMadiaInfo.StartsWith("{""response"": null, ""error"": """) = True Then
                        ReDim PlayListTorrent(0)
                        PlayListTorrent(0).Name = "ОШИБКА: " & New System.Text.RegularExpressions.Regex("(?<={""response"": null, ""error"": "").*?(?="")").Matches(AceMadiaInfo)(0).Value
                        PlayListTorrent(0).ImageLink = ICO_Error
                        Return PlayListTorrent
                    End If

                    '"Получение потока в формате HLS
                    AceMadiaInfo = WC.DownloadString("http://" & IPAdress & ":" & PortAce & "/ace/manifest.m3u8?id=" & ID)

                    Dim Regex As New System.Text.RegularExpressions.Regex("(?<=EXTINF:-1,).*(.*)")
                    Dim RegexLink As New System.Text.RegularExpressions.Regex("(http:).*(?=.*?)")
                    Dim Itog As System.Text.RegularExpressions.MatchCollection = Regex.Matches(AceMadiaInfo)
                    Dim ItogLink As System.Text.RegularExpressions.MatchCollection = RegexLink.Matches(AceMadiaInfo)

                    ReDim PlayListTorrent(Itog.Count - 1)
                    Dim N As Integer
                    For Each Match As System.Text.RegularExpressions.Match In Itog
                        PlayListTorrent(N).Name = Match.Value
                        PlayListTorrent(N).ImageLink = IconFile(Match.Value)
                        PlayListTorrent(N).Link = ItogLink(N).Value
                        N += 1
                    Next

            End Select


            Return PlayListTorrent
        End Function

        Function IconFile(ByVal Name As String) As String
            Select Case IO.Path.GetExtension(UCase(Name))

                Case ".MP3"
                    Return "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597278mp3.png"
                Case ".WMA"
                    Return "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597291wma.png"
                Case ".FLAC", ".WAV", ".AAC"
                    Return ICO_MusicFile

                Case ".AVI"
                    Return "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597263avi.png"
                Case ".MP4", ".MPG"
                    Return "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597283mpg.png"
                Case ".MKV", ".TS"
                    Return ICO_VideoFile

                Case ".BMP"
                    Return "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597263bmpfile.png"
                Case ".GIF"
                    Return "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597268giffile.png"
                Case ".JPG"
                    Return "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597278jpgfile.png"
                Case ".PNG"
                    Return "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597283pngfile.png"


                Case Else
                    Return ICO_OtherFile
            End Select
        End Function

#End Region

    End Class

End Namespace
