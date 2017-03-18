Option Explicit On
Imports System.Linq
Imports PluginApi.Plugins
Imports RemoteFork.Plugins
Imports System.Collections.Generic
Imports System.Collections.Specialized
Imports Microsoft.VisualBasic
Imports System

Namespace RemoteFork.Plugins
    <PluginAttribute(Id:="acetorrentplayb", Version:="0.36.b", Author:="ORAMAN", Name:="AceTorrentPlay (beta)", Description:="Воспроизведение файлов TORRENT через меда-сервер Ace Stream", ImageLink:="http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597291utorrent2.png")>
    Public Class AceTorrentPlay
        Implements IPlugin

        Dim IPAdress As String
        Dim PortRemoteFork As String = "8027"
        Dim PLUGIN_PATH As String = "pluginPath"
        Dim PlayList As New PluginApi.Plugins.Playlist
        Dim next_page_url As String



#Region "Настройки"

#Region "Иконки"
        Dim ICO_Folder As String = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597246folder.png"
        Dim ICO_Settings As String = "http://s1.iconbird.com/ico/1112/DiagramPreview/w128h1281354120955diagram45.png"
        Dim ICO_Settings2 As String = "http://s1.iconbird.com/ico/2013/7/395/w128h1281374340707Settings.PNG"
        Dim ICO_SettingsFolder As String = "http://s1.iconbird.com/ico/2013/6/304/w128h1281371731205supermono3dpart267.png"
        Dim ICO_SettingsParam As String = "http://s1.iconbird.com/ico/1212/Smilebyjordanfc/w90h901355053543setting.png"
        Dim ICO_VideoFile As String = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597291videofile.png"
        Dim ICO_MusicFile As String = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597240aimp.png"
        Dim ICO_TorrentFile As String = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597291utorrent2.png"
        Dim ICO_ImageFile As String = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597278imagefile.png"
        Dim ICO_M3UFile As String = "http://s1.iconbird.com/ico/0912/VannillACreamIconSet/w128h1281348320736M3U.png"
        Dim ICO_NNMClub As String = "http://s1.iconbird.com/ico/0912/MorphoButterfly/w128h1281348669898RhetenorMorpho.png"
        Dim ICO_Search As String = "http://s1.iconbird.com/ico/0612/MustHave/w256h2561339195991Search256x256.png"
#End Region

#Region "Параметры"
        Dim ProxyServr As String = "proxy.antizapret.prostovpn.org"
        Dim ProxyPort As Integer = 3128

        Dim ProxyEnablerNNM As Boolean
        Dim TrackerServerNNM As String '= "http://nnmclub.to"   '"http://nnm-club.me" 
#End Region


        Sub Load_Settings()

            Dim TempStr As String = CStr(Microsoft.Win32.Registry.GetValue("HKEY_CURRENT_USER\Software\RemoteFork\Plugins\AceTorrentPlay\", "FunctionsGetTorrentPlayList", ""))
            If TempStr = "" Then
                FunctionsGetTorrentPlayList = "GetFileListJSON"
                Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\Software\RemoteFork\Plugins\AceTorrentPlay\", "FunctionsGetTorrentPlayList", "GetFileListJSON")
            Else
                FunctionsGetTorrentPlayList = TempStr
            End If

            TempStr = CStr(Microsoft.Win32.Registry.GetValue("HKEY_CURRENT_USER\Software\RemoteFork\Plugins\AceTorrentPlay\", "ProxyEnablerNNM", ""))
            If TempStr = "" Then
                Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\Software\RemoteFork\Plugins\AceTorrentPlay\", "ProxyEnablerNNM", False)
            Else
                ProxyEnablerNNM = CBool(TempStr)
            End If

            TempStr = CStr(Microsoft.Win32.Registry.GetValue("HKEY_CURRENT_USER\Software\RemoteFork\Plugins\AceTorrentPlay\", "TrackerServerNNM", ""))
            If TempStr = "" Then
                Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\Software\RemoteFork\Plugins\AceTorrentPlay\", "TrackerServerNNM", "http://nnmclub.to")
            Else
                TrackerServerNNM = TempStr
            End If

        End Sub
        Sub Save_Settings()
            Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\Software\RemoteFork\Plugins\AceTorrentPlay\", "FunctionsGetTorrentPlayList", FunctionsGetTorrentPlayList)
        End Sub
        Dim FunctionsGetTorrentPlayList As String


        Function GetListSettings(context As IPluginContext, Optional ByVal ParametrSettings As String = "") As PluginApi.Plugins.Playlist
            PlayList.IsIptv = "false"
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
                .ImageLink = ICO_Settings
                .Type = ItemType.FILE
                Items.Add(Item_Top)
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
                .ImageLink = ICO_Settings2
                Items.Add(Item_DelSettings)
            End With




            Return PlayListPlugPar(Items, context)
        End Function
#End Region


        Public Function GetList(context As IPluginContext) As PluginApi.Plugins.Playlist Implements IPlugin.GetList

            IPAdress = context.GetRequestParams.Get("host").Split(":")(0)


            Dim path = context.GetRequestParams().Get(PLUGIN_PATH)
            path = (If((path Is Nothing), "plugin", "plugin;" & path))

            If context.GetRequestParams.Get("search") <> Nothing Then
                Select Case path
                    Case "plugin;Search_NNM"
                        Return SearchListNNM(context, context.GetRequestParams()("search"))
                    Case "plugin;Search_rutracker"
                End Select
            End If


            Select Case path
                Case "plugin"
                    Return GetTopList(context)
                Case "plugin;torrenttv"
                    Return GetTorrentTV(context)
                Case "plugin;nnmclub"
                    Return GetTopNNMClubList(context)
            End Select



            Dim PathSpliter() As String = path.Split(";")

            Select Case PathSpliter(PathSpliter.Length - 1)
                 'Трекер NNM
                Case "PAGENNM"
                    Return GetPAGENNM(context, PathSpliter(PathSpliter.Length - 2))
                Case "PAGEFILMNNM"
                    Return GetTorrentPAGENNM(context, PathSpliter(PathSpliter.Length - 2))

                  'Торрент тв
                Case "ent"
                    Return LastModifiedPlayList("ent", context)
                Case "child"
                    Return LastModifiedPlayList("child", context)
                Case "common"
                    Return LastModifiedPlayList("common", context)
                Case "discover"
                    Return LastModifiedPlayList("discover", context)
                Case "HD"
                    Return LastModifiedPlayList("HD", context)
                Case "film"
                    Return LastModifiedPlayList("film", context)
                Case "man"
                    Return LastModifiedPlayList("man", context)
                Case "music"
                    Return LastModifiedPlayList("music", context)
                Case "news"
                    Return LastModifiedPlayList("news", context)
                Case "region"
                    Return LastModifiedPlayList("region", context)
                Case "relig"
                    Return LastModifiedPlayList("relig", context)
                Case "sport"
                    Return LastModifiedPlayList("sport", context)
                         'Взрослый контент
                Case "porn"
                    Return LastModifiedPlayList("porn", context)
                Case "all"
                    Return LastModifiedPlayList("all", context)

                    'Настройки
                Case "SETTINGS"
                    Return GetListSettings(context, PathSpliter(PathSpliter.Length - 2))
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
                            .ImageLink = ICO_VideoFile
                            .Link = PlayListItem.Link
                            .Type = ItemType.FILE
                            .Description = Description
                        End With
                        items.Add(Item)
                    Next


                    Return PlayListPlugPar(items, context)

                Case ".m3u"
                    Dim Item As New Item
                    Dim WC As New System.Net.WebClient
                    WC.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0")
                    WC.Encoding = System.Text.Encoding.UTF8
                    With Item
                        .Type = ItemType.DIRECTORY
                        .GetInfo = "http://" & IPAdress & ":" & PortRemoteFork & "/treeview?pluginacetorrentplay%5c.xml&host=" & IPAdress & "%3a8027&pluginPath=getinfo&ID=" & WC.DownloadString(PathFiles)
                    End With
                    items.Add(Item)
                    Return PlayListPlugPar(items, context)
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
                    .ImageLink = ICO_VideoFile
                    .Name = System.IO.Path.GetFileNameWithoutExtension(File)
                    .Link = Microsoft.VisualBasic.Strings.Replace("http://" & IPAdress & ":" & PortRemoteFork & "/?file:/" & File, "\", "/")
                    .Description = .Link
                    .Type = ItemType.FILE
                End With
                items.Add(Item)
            Next

            For Each File As String In System.IO.Directory.EnumerateFiles(PathFiles, "*.*", System.IO.SearchOption.TopDirectoryOnly).Where(Function(s) s.EndsWith(".mp3"))
                Dim Item As New Item
                With Item
                    .ImageLink = ICO_MusicFile
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
                    .ImageLink = ICO_ImageFile
                    .Name = System.IO.Path.GetFileNameWithoutExtension(File)
                    .Link = Microsoft.VisualBasic.Strings.Replace("http://" & IPAdress & ":" & PortRemoteFork & "/?file:/" & File, "\", "/")
                    .Description = .Link
                    .Type = ItemType.FILE
                End With
                items.Add(Item)
            Next

            For Each File As String In System.IO.Directory.EnumerateFiles(PathFiles, "*.*", System.IO.SearchOption.TopDirectoryOnly).Where(Function(s) s.EndsWith(".m3u"))
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

        Function PlayListPlugPar(ByVal items As System.Collections.Generic.List(Of Item), ByVal context As IPluginContext, Optional ByVal next_page_url As String = "") As PluginApi.Plugins.Playlist
            If next_page_url <> "" Then
                Dim pluginParams = New NameValueCollection()
                pluginParams(PLUGIN_PATH) = next_page_url
                PlayList.NextPageUrl = context.CreatePluginUrl(pluginParams)
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

            Dim ItemTop, ItemTorrentTV, ItemNNMClub As New Item
            Try
                AceProxEnabl = True
                Dim AceMadiaGet As String
                AceMadiaGet = WC.DownloadString("http://" & IPAdress & ":" & PortAce & "/webui/api/service?method=get_version&format=jsonp&callback=mycallback")
                AceMadiaGet = "<html> Ответ от движка Ace Media получен: " & "<div>" & AceMadiaGet & "</div></html>"


                With ItemTop
                    .ImageLink = "http://static.acestream.net/sites/acestream/img/ACE-logo.png"
                    .Name = "        - AceTorrentPlay -        "
                    .Link = ""
                    .Type = ItemType.FILE
                    .Description = AceMadiaGet & "<html><p><p><img src="" http://static.acestream.net/sites/acestream/img/ACE-logo.png""></html>"
                End With

                With ItemTorrentTV
                    .Name = "Torrent TV"
                    .Type = ItemType.DIRECTORY
                    .Link = "torrenttv"
                    .ImageLink = "http://s1.iconbird.com/ico/1112/Television/w256h25613523820647.png"

                    If System.IO.File.Exists(System.IO.Path.GetTempPath & "MyTraf.tmp") = False Then
                        WC.DownloadFile("http://super-pomoyka.us.to/trash/ttv-list/MyTraf.php", System.IO.Path.GetTempPath & "MyTraf.tmp")
                    End If
                    .Description = "<html><img src="" http://torrent-tv.ru/images/logo.png""></html>" & WC.DownloadString(System.IO.Path.GetTempPath & "MyTraf.tmp")
                End With

                With ItemNNMClub
                    .ImageLink = ICO_NNMClub
                    .Name = "NoNaMe - Club"
                    .Link = "nnmclub"
                    .Type = ItemType.DIRECTORY
                    .Description = "<html><font face="" Arial"" size="" 5""><b>Трекер " & .Name & "</font></b><p><img src="" http://assets.nnm-club.ws/forum/images/logos/10let8.png"" />"
                End With


                items.Add(ItemTop)
                items.Add(ItemTorrentTV)
                items.Add(ItemNNMClub)

            Catch
                AceProxEnabl = False
                With ItemTop
                    .ImageLink = "http://errorfix48.ru/uploads/posts/2014-09/1409846068_400px-warning_icon.png"
                    .Name = "        - AceTorrentPlay -        "
                    .Link = ""
                    .Type = ItemType.FILE
                    .Description = "Ответ от движка Ace Media не получен!"
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
                .Description = "В скором времени здесь появятся кое-какие настройки. "
            End With
            items.Add(ItemSettings)

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

#Region "NNM Club"
        Dim CookiesNNM As String = "phpbb2mysql_4_data=a%3A2%3A%7Bs%3A11%3A%22autologinid%22%3Bs%3A32%3A%2296229c9a3405ae99cce1f3bc0cefce2e%22%3Bs%3A6%3A%22userid%22%3Bs%3A8%3A%2213287549%22%3B%7D"

        Public Function SearchListNNM(context As IPluginContext, ByVal search As String) As PluginApi.Plugins.Playlist

            Dim RequestPost As System.Net.HttpWebRequest = System.Net.HttpWebRequest.Create(TrackerServerNNM & "/forum/tracker.php")
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
                Dim Item As New Item
                Item.Name = "Ничего не найдено"
                Item.Link = ""

                items.Add(Item)
            End If

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

                .Description = "<html><font face=""Arial"" size=""5""><b>" & .Name & "</font></b><p><img src=""http://assets.nnm-club.ws/forum/images/logos/10let8.png"" />"

            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Name = "Новинки кино"
                .Link = TrackerServerNNM & "/forum/portal.php?c=10;PAGENNM"
                .ImageLink = ICO_Folder
                .Description = "<html><font face=""Arial"" size=""5""><b>" & .Name & "</font></b><p><img src=""http://assets.nnm-club.ws/forum/images/logos/10let8.png"" />"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Name = "Наше кино"
                .Link = TrackerServerNNM & "/forum/portal.php?c=13;PAGENNM"
                .ImageLink = ICO_Folder
                .Description = "<html><font face=""Arial"" size=""5""><b>" & .Name & "</font></b><p><img src=""http://assets.nnm-club.ws/forum/images/logos/10let8.png"" />"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Name = "Зарубежное кино"
                .Link = TrackerServerNNM & "/forum/portal.php?c=6;PAGENNM"
                .ImageLink = ICO_Folder
                .Description = "<html><font face=""Arial"" size=""5""><b>" & .Name & "</font></b><p><img src=""http://assets.nnm-club.ws/forum/images/logos/10let8.png"" />"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Name = "HD (3D) Кино"
                .Link = TrackerServerNNM & "/forum/portal.php?c=11;PAGENNM"
                .ImageLink = ICO_Folder
                .Description = "<html><font face=""Arial"" size=""5""><b>" & .Name & "</font></b><p><img src=""http://assets.nnm-club.ws/forum/images/logos/10let8.png"" />"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Name = "Артхаус"
                .Link = TrackerServerNNM & "/forum/portal.php?c=17;PAGENNM"
                .ImageLink = ICO_Folder
                .Description = "<html><font face=""Arial"" size=""5""><b>" & .Name & "</font></b><p><img src=""http://assets.nnm-club.ws/forum/images/logos/10let8.png"" />"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Name = "Наши сериалы"
                .Link = TrackerServerNNM & "/forum/portal.php?c=4;PAGENNM"
                .ImageLink = ICO_Folder
                .Description = "<html><font face=""Arial"" size=""5""><b>" & .Name & "</font></b><p><img src=""http://assets.nnm-club.ws/forum/images/logos/10let8.png"" />"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Name = "Зарубежные сериалы"
                .Link = TrackerServerNNM & "/forum/portal.php?c=3;PAGENNM"
                .ImageLink = ICO_Folder
                .Description = "<html><font face=""Arial"" size=""5""><b>" & .Name & "</font></b><p><img src=""http://assets.nnm-club.ws/forum/images/logos/10let8.png"" />"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Name = "Театр, МузВидео, Разное"
                .Link = TrackerServerNNM & "/forum/portal.php?c=21;PAGENNM"
                .ImageLink = ICO_Folder
                .Description = "<html><font face=""Arial"" size=""5""><b>" & .Name & "</font></b><p><img src=""http://assets.nnm-club.ws/forum/images/logos/10let8.png"" />"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Name = "Док. TV-бренды"
                .Link = TrackerServerNNM & "/forum/portal.php?c=22;PAGENNM"
                .ImageLink = ICO_Folder
                .Description = "<html><font face=""Arial"" size=""5""><b>" & .Name & "</font></b><p><img src=""http://assets.nnm-club.ws/forum/images/logos/10let8.png"" />"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Name = "Док. и телепередачи"
                .Link = TrackerServerNNM & "/forum/portal.php?c=23;PAGENNM"
                .ImageLink = ICO_Folder
                .Description = "<html><font face=""Arial"" size=""5""><b>" & .Name & "</font></b><p><img src=""http://assets.nnm-club.ws/forum/images/logos/10let8.png"" />"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Name = "Спорт и Юмор"
                .Link = TrackerServerNNM & "/forum/portal.php?c=24;PAGENNM"
                .ImageLink = ICO_Folder
                .Description = "<html><font face=""Arial"" size=""5""><b>" & .Name & "</font></b><p><img src=""http://assets.nnm-club.ws/forum/images/logos/10let8.png"" />"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Name = "Аниме и Манга"
                .Link = TrackerServerNNM & "/forum/portal.php?c=1;PAGENNM"
                .ImageLink = ICO_Folder
                .Description = "<html><font face=""Arial"" size=""5""><b>" & .Name & "</font></b><p><img src=""http://assets.nnm-club.ws/forum/images/logos/10let8.png"" />"
            End With
            items.Add(Item)

            Return PlayListPlugPar(items, context)
        End Function

        Function GetPAGENNM(ByVal context As IPluginContext, ByVal URL As String) As PluginApi.Plugins.Playlist

            Dim items As New System.Collections.Generic.List(Of Item)()

            Try
                Dim RequestGet As System.Net.HttpWebRequest = System.Net.HttpWebRequest.Create(URL)
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
                Item.Name = "ERROR"
                Item.Description = ex.Message
                Item.Link = "plugin"
                items.Add(Item)
            End Try

            PlayList.IsIptv = "false"
            Return PlayListPlugPar(items, context, next_page_url)

        End Function

        Function Base64Encode(ByVal plainText As String) As String
            Dim plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText)
            Return System.Convert.ToBase64String(plainTextBytes)
        End Function

        Public Function GetTorrentPAGENNM(ByVal context As IPluginContext, ByVal URL As String) As PluginApi.Plugins.Playlist
            PlayList.IsIptv = "false"
            Dim RequestGet As System.Net.HttpWebRequest = Net.HttpWebRequest.Create(URL)
            If ProxyEnablerNNM = True Then RequestGet.Proxy = New System.Net.WebProxy(ProxyServr, ProxyPort)
            RequestGet.Method = "GET"
            RequestGet.Headers.Add("Cookie", CookiesNNM)

            Dim Response As Net.HttpWebResponse = RequestGet.GetResponse
            Dim dataStream As System.IO.Stream = Response.GetResponseStream()
            Dim reader As New System.IO.StreamReader(dataStream, Text.Encoding.GetEncoding(1251))
            Dim responseFromServer As String = reader.ReadToEnd
            reader.Close()
            dataStream.Close()
            Response.Close()

            Dim Regex As New System.Text.RegularExpressions.Regex("(?<=<span class=""genmed""><b><a href="").*?(?=&amp;)")
            Dim TorrentPath As String = Nothing
            TorrentPath = TrackerServerNNM & "/forum/" & Regex.Matches(responseFromServer)(0).Value


            Dim Title As String
            Regex = New System.Text.RegularExpressions.Regex("(?<=<span style=""font-weight: bold"">).*?(?=</span>)")
            Title = Regex.Matches(responseFromServer)(0).Value




                Dim RequestTorrent As System.Net.HttpWebRequest = Net.HttpWebRequest.Create(TorrentPath)
            If ProxyEnablerNNM = True Then RequestTorrent.Proxy = New System.Net.WebProxy(ProxyServr, ProxyPort)
            RequestTorrent.Method = "GET"
            RequestTorrent.Headers.Add("Cookie", CookiesNNM)

            Response = RequestTorrent.GetResponse
            dataStream = Response.GetResponseStream()
            reader = New System.IO.StreamReader(dataStream, System.Text.Encoding.GetEncoding(1251))
            Dim FileTorrent As String = reader.ReadToEnd
            System.IO.File.WriteAllText(System.IO.Path.GetTempPath & "TorrentTemp", FileTorrent, System.Text.Encoding.GetEncoding(1251))
            reader.Close()
            dataStream.Close()
            Response.Close()

            Dim items As New System.Collections.Generic.List(Of Item)
            Try
                Dim Description As String = FormatDescriptionFileNNM(responseFromServer)

                Dim PlayListtoTorrent() As TorrentPlayList = GetFileList(System.IO.Path.GetTempPath & "TorrentTemp")

                For Each PlayListItem As TorrentPlayList In PlayListtoTorrent
                    Dim Item As New Item
                    With Item
                        .Name = PlayListItem.Name
                        .ImageLink = ICO_VideoFile
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
                End With
                items.Add(Item)
            End Try

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

        Public Function GetTorrentTV(ByVal context As IPluginContext) As PluginApi.Plugins.Playlist
            Dim items = New Collections.Generic.List(Of Item)
            Dim Item As New Item

            With Item
                .Type = ItemType.DIRECTORY
                .Name = "РАЗВЛЕКАТЕЛЬНЫЕ"
                .Link = "ent"
                .ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Type = ItemType.DIRECTORY
                .Name = "ДЕТСКИЕ"
                .Link = "child"
                .ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Type = ItemType.DIRECTORY
                .Name = "ПОЗНАВАТЕЛЬНЫЕ"
                .Link = "discover"
                .ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Type = ItemType.DIRECTORY
                .Name = "HD"
                .Link = "HD"
                .ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Type = ItemType.DIRECTORY
                .Name = "ОБЩИЕ"
                .Link = "common"
                .ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Type = ItemType.DIRECTORY
                .Name = "ФИЛЬМЫ"
                .Link = "film"
                .ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Type = ItemType.DIRECTORY
                .Name = "МУЖСКИЕ"
                .Link = "man"
                .ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Type = ItemType.DIRECTORY
                .Name = "МУЗЫКАЛЬНЫЕ"
                .Link = "music"
                .ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Type = ItemType.DIRECTORY
                .Name = "НОВОСТИ"
                .Link = "news"
                .ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Type = ItemType.DIRECTORY
                .Name = "РЕГИОНАЛЬНЫЕ"
                .Link = "region"
                .ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Type = ItemType.DIRECTORY
                .Name = "РЕЛИГИОЗНЫЕ"
                .Link = "relig"
                .ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            End With
            items.Add(Item)

            Item = New Item
            With Item
                .Type = ItemType.DIRECTORY
                .Name = "СПОРТ"
                .Link = "sport"
                .ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            End With
            items.Add(Item)

            'Item = New Item
            'With Item
            '    .Type = ItemType.DIRECTORY
            '    .Name = "ЭРОТИКА"
            '    .Link = "porn"
            '    .ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            'End With
            'items.Add(Item)

            'Item = New Item
            'With Item
            '    .Type = ItemType.DIRECTORY
            '    .Name = "ВСЕ КАНАЛЫ"
            '    .Link = "all"
            '    .ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            'End With
            'items.Add(Item)

            Return PlayListPlugPar(items, context)
        End Function

        Function LastModifiedPlayList(ByVal NamePlayList As String, ByVal context As IPluginContext) As PluginApi.Plugins.Playlist

            Dim PathFileUpdateTime As String = System.IO.Path.GetTempPath & NamePlayList & ".UpdateTime.tmp"
            Dim PathFilePlayList As String = System.IO.Path.GetTempPath & NamePlayList & ".PlayList.m3u8"

            Dim request As System.Net.HttpWebRequest = Net.HttpWebRequest.Create("http://super-pomoyka.us.to/trash/ttv-list/ttv." & NamePlayList & ".iproxy.m3u?ip=" & IPAdress & ":" & PortAce)
            request.Method = "HEAD"
            request.ContentType = "text/html"
            request.KeepAlive = True
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8"
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36"
            request.Host = "super-pomoyka.us.to"
            Dim response As System.Net.HttpWebResponse = CType(request.GetResponse(), System.Net.HttpWebResponse)
            Dim responHeader = response.GetResponseHeader("Last-Modified")
            response.Close()

            Dim WC As New System.Net.WebClient
            WC.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36")
            WC.Encoding = System.Text.Encoding.UTF8

            Dim items As New System.Collections.Generic.List(Of Item)
            Dim Item As New Item

            If (System.IO.File.Exists(PathFileUpdateTime) AndAlso System.IO.File.Exists(PathFilePlayList)) = False Then
                UpdatePlayList(NamePlayList, PathFilePlayList, PathFileUpdateTime, responHeader)
                Item.Type = ItemType.DIRECTORY
                Item.GetInfo = "http://" & IPAdress & ":" & PortRemoteFork & "/treeview?pluginacetorrentplay%5c.xml&host=" & IPAdress & "%3a8027&pluginPath=getinfo&ID=" & WC.DownloadString(PathFilePlayList)
                items.Add(Item)
                Return PlayListPlugPar(items, context)
            End If

            If responHeader <> System.IO.File.ReadAllText(PathFileUpdateTime) Then
                UpdatePlayList(NamePlayList, PathFilePlayList, PathFileUpdateTime, responHeader)
                Item.Type = ItemType.DIRECTORY
                Item.GetInfo = "http://" & IPAdress & ":" & PortRemoteFork & "/treeview?pluginacetorrentplay%5c.xml&host=" & IPAdress & "%3a8027&pluginPath=getinfo&ID=" & WC.DownloadString(PathFilePlayList)
                items.Add(Item)
                Return PlayListPlugPar(items, context)
            End If

            Item.Type = ItemType.DIRECTORY
            Item.GetInfo = "http://" & IPAdress & ":" & PortRemoteFork & "/treeview?pluginacetorrentplay%5c.xml&host=" & IPAdress & "%3a8027&pluginPath=getinfo&ID=" & WC.DownloadString(PathFilePlayList)
            items.Add(Item)
            PlayList.IsIptv = "true"
            Return PlayListPlugPar(items, context)

        End Function

        Sub UpdatePlayList(ByVal NamePlayList As String, ByVal PathFilePlayList As String, ByVal PathFileUpdateTime As String, ByVal LastModified As String)
            System.IO.File.WriteAllText(PathFileUpdateTime, LastModified)
            Dim WC As New System.Net.WebClient
            WC.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36")
            WC.Encoding = System.Text.Encoding.UTF8
            WC.DownloadFile("http://super-pomoyka.us.to/trash/ttv-list/ttv." & NamePlayList & ".iproxy.m3u?ip=" & IPAdress & ":" & PortAce, PathFilePlayList)
            WC.DownloadFile("http://super-pomoyka.us.to/trash/ttv-list/MyTraf.php", System.IO.Path.GetTempPath & "MyTraf.tmp")
            WC.Dispose()
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
        End Structure


        Function GetID(ByVal PathTorrent As String) As String
            Dim WC As New System.Net.WebClient
            WC.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0")
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

            Dim response As System.Net.HttpWebResponse = request.GetResponse()
            dataStream = response.GetResponseStream()
            Dim reader As New System.IO.StreamReader(dataStream)
            Dim responseFromServer As String = reader.ReadToEnd()

            Dim responseSplit() As String = responseFromServer.Split("""")
            Dim ID As String = responseSplit(3)
            Return ID
        End Function

        Function GetFileList(ByVal PathTorrent As String) As TorrentPlayList()

            Dim WC As New System.Net.WebClient
            WC.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0")
            WC.Encoding = System.Text.Encoding.UTF8

            Dim ID As String = GetID(PathTorrent)
            Dim PlayListTorrent() As TorrentPlayList

            ''Информация о запущенном файле 
            Dim AceMadiaInfo As String = WC.DownloadString("http://127.0.0.1:" & PortAce & "/ace/manifest.m3u8?id=" & ID & "&format=json&use_api_events=1&use_stop_notifications=1")
            ' IO.File.WriteAllText("d:\My Desktop\AceMadiaInfo.txt", AceMadiaInfo)

            Select Case FunctionsGetTorrentPlayList
                Case "GetFileListJSON"
GetFileListJSON:    Dim CodeZnaki() As String = {"\U0430", "\U0431", "\U0432", "\U0433", "\U0434", "\U0435", "\U0451", "\U0436", "\U0437", "\U0438", "\U0439", "\U043A", "\U043B", "\U043C", "\U043D", "\U043E", "\U043F", "\U0440", "\U0441", "\U0442", "\U0443",
                    "\U0444", "\U0445", "\U0446", "\U0447", "\U0448", "\U0449", "\U044A", "\U044B", "\U044C", "\U044D", "\U044E", "\U044F", "\U0410", "\U0411", "\U0412", "\U0413", "\U0414", "\U0415", "\U0401", "\U0416", "\U0417", "\U0418", "\U0419", "\U041A",
                    "\U041B", "\U041C", "\U041D", "\U041E", "\U041F", "\U0420", "\U0421", "\U0422", "\U0423", "\U0424", "\U0425", "\U0426", "\U0427", "\U0428", "\U0429", "\U042A", "\U042B", "\U042C", "\U042D", "\U042E", "\U042F", "\U00AB", "\U00BB", "U2116"}
                    Dim DecodeZnaki() As String = {"а", "б", "в", "г", "д", "е", "ё", "ж", "з", "и", "й", "к", "л", "м", "н", "о", "п", "р", "с", "т", "у", "ф", "х", "ц", "ч", "ш", "щ", "ъ", "ы", "ь", "э", "ю", "я",
                    "А", "Б", "В", "Г", "Д", "Е", "Ё", "Ж", "З", "И", "Й", "К", "Л", "М", "Н", "О", "П", "Р", "С", "Т", "У", "Ф", "Х", "Ц", "Ч", "Ш", "Щ", "Ъ", "Ы", "Ь", "Э", "Ю", "Я", "«", "»", "№"}

                    'Dim ContentID As String = GetID(PathTorrent)
                    Dim ItogStr As String = WC.DownloadString("http://127.0.0.1:" & PortAce & "/server/api?method=get_media_files&content_id=" & ID)
                    For I As Integer = 0 To 68
                        ItogStr = Microsoft.VisualBasic.Strings.Replace(ItogStr, Microsoft.VisualBasic.Strings.LCase(CodeZnaki(I)), DecodeZnaki(I))
                    Next
                    WC.Dispose()

                    Dim PlayListJson As String = ItogStr
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
                        I += 1
                        N += 1
                    Next
                    Return PlayListTorrent
                Case "GetFileListM3U"
                    If AceMadiaInfo.StartsWith("{""response"": {""event_url"": """) = True Then
                        GoTo GetFileListJSON
                    End If

                    'Получение потока в формате HLS
                    AceMadiaInfo = WC.DownloadString("http://" & IPAdress & ":" & PortAce & "/ace/manifest.m3u8?id=" & ID)

                    Dim Regex As New System.Text.RegularExpressions.Regex("(?<=EXTINF:-1,).*(.*)")
                    Dim Itog As System.Text.RegularExpressions.MatchCollection = Regex.Matches(AceMadiaInfo)

                    ReDim PlayListTorrent(Itog.Count - 1)
                    Dim N As Integer
                    For Each Match As System.Text.RegularExpressions.Match In Itog
                        PlayListTorrent(N).Name = Match.Value
                        N += 1
                    Next

                    N = 0
                    Regex = New System.Text.RegularExpressions.Regex("(http:).*(?=.*?)")
                    Itog = Regex.Matches(AceMadiaInfo)
                    For Each Match As System.Text.RegularExpressions.Match In Itog
                        PlayListTorrent(N).Link = Match.Value
                        N += 1
                    Next

                    Return PlayListTorrent
            End Select
            Return Nothing

        End Function


#End Region

    End Class

End Namespace
