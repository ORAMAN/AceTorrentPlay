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
    <PluginAttribute(Id:="tvfeed", Version:="0.15b", Author:="ORAMAN", Name:="TVFeed", Description:="Воспроизведение видео с сайта https://tvfeed.in через меда-сервер Ace Stream", ImageLink:="https://tvfeed.in/img/tvfeedin.png")>
    Public Class TVFeed
        Implements IPlugin

#Region "Иконки"
        Dim ICO_FolderVideo As String = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597246foldervideos.png"
        Dim ICO_FolderVideo2 As String = "http://s1.iconbird.com/ico/1112/Concave/w256h2561352644144Videos.png"
        Dim ICO_Folder As String = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597246folder.png"
        Dim ICO_Folder2 As String = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597240explorer.png"
        Dim ICO_OtherFile As String = "http://s1.iconbird.com/ico/2013/6/364/w256h2561372348486helpfile256.png"
        Dim ICO_VideoFile As String = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597291videofile.png"
        Dim ICO_MusicFile As String = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597283musicfile.png"
        Dim ICO_ImageFile As String = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597278imagefile.png"
        Dim ICO_M3UFile As String = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597278librarymusic.png"
        Dim ICO_NNMClub As String = "http://s1.iconbird.com/ico/0912/MorphoButterfly/w128h1281348669898RhetenorMorpho.png"
        Dim ICO_Search As String = "http://s1.iconbird.com/ico/0612/MustHave/w256h2561339195991Search256x256.png"
        Dim ICO_Error As String = "http://s1.iconbird.com/ico/0912/ToolbarIcons/w256h2561346685474SymbolError.png"
        Dim ICO_Pusto As String = "https://avatanplus.com/files/resources/mid/5788db3ecaa49155ee986d6e.png"


#End Region

#Region "Параметры"
        ' Dim ProxyServr As String = "proxy.antizapret.prostovpn.org"
        ' Dim ProxyPort As Integer = 3128
        ' Dim Proxyer As New Net.WebProxy(ProxyServr, ProxyPort)
        Dim IPAdress As String
        Dim PortRemoteFork As String = "8027"
        Dim PLUGIN_PATH As String = "pluginPath"
        Dim PlayList As New PluginApi.Plugins.Playlist
        Dim next_page_url As String
        Dim IDPlagin As String = "tvfeed"
        Dim FunctionsGetTorrentPlayList As String
        Dim Cookie As String = "4qpyAa7ZVD5PyRMHfeq58xlVA0izZs2s"
        Dim AdressTvFeed As String = "https://tvfeed.in"

#End Region

#Region "MAIN"

        Public Function GetTopList(ByVal context As IPluginContext) As PluginApi.Plugins.Playlist
            Dim STR As String = ReqHTML("/")

            Dim items As New System.Collections.Generic.List(Of Item)
            Dim ItemSearch As New Item
            With ItemSearch
                .Name = "Поиск"
                .Link = "search_all"
                .Type = ItemType.DIRECTORY
                .SearchOn = "Поиск"
                .ImageLink = ICO_Search
                .Description = "<div align=""center""><img src= ""https://tvfeed.in/img/about-bg.jpg"" width=100% ></div><p><strong>О проекте:</strong></p><p>Основанный в 2012 году проект TVFeed был одной из показательных площадок системы Ace Stream. Изначальный акцент был сделан на зарубежные сериалы. Сделав ставку на высокое качество (HD) и разнообразие студий озвучек, мы начали занимать лидирующие позиции в поиске и искать своих пользователей, единомышленников.</p>"
                items.Add(ItemSearch)
            End With


            ''
            Dim ItemSerials As New Item
            With ItemSerials
                .Name = "<span style=""color:#3090F0""><u><strong>СЕРИАЛЫ</strong></u></span>"
                .ImageLink = "http://s1.iconbird.com/ico/2013/6/319/w128h1281371898000folder.png"
                .Link = "/serial/;1;SERIALEZESLIST"
                .Description = .Name & "<div align=""center""><img src= ""https://tvfeed.in/img/tvfeed-collection_v2.jpg"" width=100% ></div>"
                items.Add(ItemSerials)
            End With

            Dim ItemNewSerials As New Item
            With ItemNewSerials
                .Name = "<strong>Новые сериалы</strong>"
                .ImageLink = AdressTvFeed & "/img/icon/new.svg"
                .Link = ";/serial/newest/;1;SERIALEZESLIST"
                .Description = .Name & "<div align=""center""><img src= """ & AdressTvFeed & "/img/icon/new.svg"" width=40% ></div>"
                items.Add(ItemNewSerials)
            End With

            Dim ItemPopularSerials As New Item
            With ItemPopularSerials
                .Name = "<strong>Популярные сериалы</strong>"
                .ImageLink = AdressTvFeed & "/img/icon/popular.svg"
                .Link = ";/serial/popular/;1;SERIALEZESLIST"
                .Description = .Name & "<div align=""center""><img src= """ & AdressTvFeed & "/img/icon/popular.svg"" width=40% ></div>"
                items.Add(ItemPopularSerials)
            End With

            Dim ItemRecomendSerials As New Item
            With ItemRecomendSerials
                .Name = "<strong>Рекомендуем сериалы</strong>"
                .ImageLink = AdressTvFeed & "/img/icon/recommend.svg"
                .Link = ";/serial/recommended/;1;SERIALEZESLIST"
                .Description = .Name & "<div align=""center""><img src= """ & AdressTvFeed & "/img/icon/recommend.svg"" width=40% ></div>"
                items.Add(ItemRecomendSerials)
            End With

            Dim ItemRandomSerials As New Item
            With ItemRandomSerials
                .Name = "<strong>Случайный сериал</strong>"
                .ImageLink = AdressTvFeed & "/img/icon/random.svg"
                .Link = ";RANDOMSERIAL"
                .Description = .Name & "<div align=""center""><img src= """ & AdressTvFeed & "/img/icon/random.svg"" width=40% ></div>"
                items.Add(ItemRandomSerials)
            End With


            '
            ''
            Dim ItemFilms As New Item
            With ItemFilms
                .Name = "<span style=color:#86F4EB><u><strong>ФИЛЬМЫ</strong></u></span>"
                .ImageLink = "http://s1.iconbird.com/ico/2013/6/319/w128h1281371898069foldergreen.png"
                .Link = ";/film/;1;FILMESLIST"
                .Description = .Name & "<div align=""center""><img src= ""https://tvfeed.in/img/tvfeed-movie.jpg"" width=100% ></div>"
                items.Add(ItemFilms)
            End With

            Dim ItemNewFilms As New Item
            With ItemNewFilms
                .Name = "<strong>Новые фильмы</strong>"
                .ImageLink = AdressTvFeed & "/img/icon/new.svg"
                .Link = ";/film/newest/;1;FILMESLIST"
                .Description = .Name & "<div align=""center""><img src= """ & AdressTvFeed & "/img/icon/new.svg"" width=40% ></div>"
                items.Add(ItemNewFilms)
            End With

            Dim ItemPopularFilms As New Item
            With ItemPopularFilms
                .Name = "<strong>Популярные фильмы</strong>"
                .ImageLink = AdressTvFeed & "/img/icon/popular.svg"
                .Link = ";/film/popular/;1;FILMESLIST"
                .Description = .Name & "<div align=""center""><img src= """ & AdressTvFeed & "/img/icon/popular.svg"" width=40% ></div>"
                items.Add(ItemPopularFilms)
            End With

            Dim ItemRecomendFilms As New Item
            With ItemRecomendFilms
                .Name = "<strong>Рекомендуем фильмы</strong>"
                .ImageLink = AdressTvFeed & "/img/icon/recommend.svg"
                .Link = ";/film/recommended/;1;FILMESLIST"
                .Description = .Name & "<div align=""center""><img src= """ & AdressTvFeed & "/img/icon/recommend.svg"" width=40% ></div>"
                items.Add(ItemRecomendFilms)
            End With

            Dim ItemRandomFilm As New Item
            With ItemRandomFilm
                .Name = "<strong>Случайный фильм</strong>"
                .ImageLink = AdressTvFeed & "/img/icon/random.svg"
                .Link = ";RANDOMFILM"
                .Description = .Name & "<div align=""center""><img src= """ & AdressTvFeed & "/img/icon/random.svg"" width=40% ></div>"
                items.Add(ItemRandomFilm)
            End With
            '
            ''
            '''



            Dim ItemTVs As New Item
            With ItemTVs
                .Name = "<span style=""color:#CA91EB""><u><strong>ТЕЛЕКАНАЛЫ</strong></u></span>"
                .ImageLink = "http://s1.iconbird.com/ico/2013/6/319/w128h1281371898216folderviolet.png"
                .Link = ";/tv/popular/;1;TVS"
                .Description = .Name & "<div align=""center""><img src= ""https://tvfeed.in/img/tvfeed-tv.jpg"" width=100% ></div>"
                items.Add(ItemTVs)
            End With

            Dim ItemRandomTV As New Item
            With ItemRandomTV
                .Name = "<strong>Случайный телеканал</strong>"
                .ImageLink = AdressTvFeed & "/img/icon/random.svg"
                .Link = ";RANDOMTV"
                .Description = .Name & "<div align=""center""><img src= """ & AdressTvFeed & "/img/icon/random.svg"" width=40% ></div>"
                items.Add(ItemRandomTV)
            End With

            PlayList.IsIptv = "false"
            Return PlayListPlugPar(items, context)
        End Function

        Public Function GetList(context As IPluginContext) As PluginApi.Plugins.Playlist Implements IPlugin.GetList
            Dim items As New System.Collections.Generic.List(Of Item)
            IPAdress = context.GetRequestParams.Get("host").Split(":")(0)

            PlayList.source = Nothing
            Dim path = context.GetRequestParams().Get(PLUGIN_PATH)
            path = (If((path Is Nothing), "plugin", "plugin;" & path))

            If context.GetRequestParams.Get("search") <> Nothing Then
                Select Case path
                    Case "plugin;search_all"
                        Return GetSearchList(context, context.GetRequestParams("search"))
                End Select
            End If


            Select Case path
                Case "plugin"
                    Return GetTopList(context)
            End Select


            Dim PathSpliter() As String = path.Split(";")

            Select Case PathSpliter(PathSpliter.Length - 1)

                    'СЕРИАЛЫ
                Case "RANDOMSERIAL"
                    Return GetRandomSerial(context)
                Case "PAGE_SERIAL"
                    Return GetPageSerial(context, PathSpliter(PathSpliter.Length - 2))
                Case "SEZON_PAGE"
                    Return GetPageSezon(context, PathSpliter(PathSpliter.Length - 3), PathSpliter(PathSpliter.Length - 2))
                Case "SEZONE"
                    Return GetSezon(context, PathSpliter(PathSpliter.Length - 3), PathSpliter(PathSpliter.Length - 2))
                Case "SERIALEZESLIST"
                    Return GetListSerialezes(context, PathSpliter(PathSpliter.Length - 3), PathSpliter(PathSpliter.Length - 2))

                    'ФИЛЬМЫ
                Case "PAGE_FILM"
                    Return GetPageFilm(context, PathSpliter(PathSpliter.Length - 2))
                Case "FILM"
                    Return GetFilm(context, PathSpliter(PathSpliter.Length - 3), PathSpliter(PathSpliter.Length - 2))
                Case "FILMESLIST"
                    Return GetListFilmes(context, PathSpliter(PathSpliter.Length - 3), PathSpliter(PathSpliter.Length - 2))
                Case "RANDOMFILM"
                    Return GetRandomFilm(context)

                    'ТВ
                Case "PAGE_TV"
                    Return GetPageTV(context, PathSpliter(PathSpliter.Length - 2))
                Case "TVS"
                    Return GetListTV(context, PathSpliter(PathSpliter.Length - 3), PathSpliter(PathSpliter.Length - 2))
                Case "RANDOMTV"
                    Return GetRandomTV(context)
                    'АКТЁРЫ
                Case "PAGE_AKTER"
                    Return GetPageAkter(context, PathSpliter(PathSpliter.Length - 2))

                    'ОЗВУЧКИ
                Case "PAGE_OTZVUCH"
                    Return GetPageOzvuch(context, PathSpliter(PathSpliter.Length - 3), PathSpliter(PathSpliter.Length - 2))

            End Select


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
            Item.Description = "peers2<br>"
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
            PlayList.Timeout = "40" 'sec

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

#End Region

#Region "ПОИСК"
        'Public Function GetSearchListJSON(ByVal context As IPluginContext, ByVal SearchText As String) As PluginApi.Plugins.Playlist
        '    Dim Req As Net.HttpWebRequest = Net.HttpWebRequest.Create(AdressTvFeed  & "/ajax_search/")
        '    Req.Method = "POST"
        '    Req.ContentType = "application/x-www-form-urlencoded; charset=UTF-8"
        '    Req.Headers.Add("cookie", "csrftoken=RqfGuMA3tO2VQWjdFdYhVqxLZJWCX2wE")
        '    Req.Referer = AdressTvFeed 
        '    Req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3071.115 Safari/537.36"
        '    Req.Headers.Add("x-requested-with", "XMLHttpRequest")

        '    Dim myStream As System.IO.Stream = Req.GetRequestStream
        '    Dim DataStr As String = "q=" & SearchText & "&csrfmiddlewaretoken=RqfGuMA3tO2VQWjdFdYhVqxLZJWCX2wE"
        '    Dim DataByte() As Byte = System.Text.Encoding.UTF8.GetBytes(DataStr)
        '    myStream.Write(DataByte, 0, DataByte.Length)
        '    myStream.Close()

        '    Dim Response As System.Net.HttpWebResponse = Req.GetResponse
        '    Dim dataStream As System.IO.Stream = Response.GetResponseStream
        '    Dim reader As New System.IO.StreamReader(dataStream, System.Text.Encoding.UTF8)

        '    Dim STR As String = reader.ReadToEnd()
        '    Dim CodeZnaki() As String = {"\U0430", "\U0431", "\U0432", "\U0433", "\U0434", "\U0435", "\U0451", "\U0436", "\U0437", "\U0438", "\U0439", "\U043A", "\U043B", "\U043C", "\U043D", "\U043E", "\U043F", "\U0440", "\U0441", "\U0442", "\U0443",
        '        "\U0444", "\U0445", "\U0446", "\U0447", "\U0448", "\U0449", "\U044A", "\U044B", "\U044C", "\U044D", "\U044E", "\U044F", "\U0410", "\U0411", "\U0412", "\U0413", "\U0414", "\U0415", "\U0401", "\U0416", "\U0417", "\U0418", "\U0419", "\U041A",
        '        "\U041B", "\U041C", "\U041D", "\U041E", "\U041F", "\U0420", "\U0421", "\U0422", "\U0423", "\U0424", "\U0425", "\U0426", "\U0427", "\U0428", "\U0429", "\U042A", "\U042B", "\U042C", "\U042D", "\U042E", "\U042F", "\U00AB", "\U00BB", "U2116"}
        '    Dim DecodeZnaki() As String = {"а", "б", "в", "г", "д", "е", "ё", "ж", "з", "и", "й", "к", "л", "м", "н", "о", "п", "р", "с", "т", "у", "ф", "х", "ц", "ч", "ш", "щ", "ъ", "ы", "ь", "э", "ю", "я",
        '        "А", "Б", "В", "Г", "Д", "Е", "Ё", "Ж", "З", "И", "Й", "К", "Л", "М", "Н", "О", "П", "Р", "С", "Т", "У", "Ф", "Х", "Ц", "Ч", "Ш", "Щ", "Ъ", "Ы", "Ь", "Э", "Ю", "Я", "«", "»", "№"}

        '    For I As Integer = 0 To 68
        '        STR = Microsoft.VisualBasic.Strings.Replace(STR, Microsoft.VisualBasic.Strings.LCase(CodeZnaki(I)), DecodeZnaki(I))
        '    Next



        '    Dim items As New System.Collections.Generic.List(Of Item)
        '    If STR = "{""error"" 2}" Then
        '        Dim Item As New Item
        '        Item.Link = ""
        '        Item.ImageLink = ICO_Pusto
        '        Item.Name = "<span style=""color#F68648"">" & " - Ничего не найдено - " & "</span>"
        '        Item.Description = "Поиск не дал результатов"
        '        items.Add(Item)
        '        PlayList.IsIptv = "False"
        '        Return PlayListPlugPar(items, context)
        '    End If

        '    Dim Regex As New System.Text.RegularExpressions.Regex("({).*?(})")
        '    Dim RegexUrl As New System.Text.RegularExpressions.Regex("(?<=""url"": "").*?(?="")")
        '    Dim RegexName As New System.Text.RegularExpressions.Regex("(?<=""name"": "").*?(?="")")
        '    Dim RegexImg As New System.Text.RegularExpressions.Regex("(?<=""img"": "").*?(?="")")

        '    For Each Reg As System.Text.RegularExpressions.Match In Regex.Matches(STR)
        '        Dim Item As New Item
        '        Item.Type = ItemType.DIRECTORY
        '        Item.Link = RegexUrl.Match(Reg.Value).Value & ";PAGE_FILM"
        '        Item.Name = RegexName.Match(Reg.Value).Value
        '        Item.ImageLink = RegexImg.Match(Reg.Value).Value
        '        Item.Description = Item.ImageLink
        '        items.Add(Item)
        '    Next

        '    PlayList.IsIptv = "False"
        '    Return PlayListPlugPar(items, context)
        'End Function

        Public Function GetSearchList(ByVal context As IPluginContext, ByVal SearchText As String) As PluginApi.Plugins.Playlist
            Dim STR As String = ReqHTML(AdressTvFeed & "/search/", SearchText, "POST")
            Dim Nom As String = ""
            Dim items As New System.Collections.Generic.List(Of Item)
            Dim ReGexTop As New System.Text.RegularExpressions.Regex("(?<=<h3 class=""heading"">).*(</main>)")
            If ReGexTop.IsMatch(STR) = True Then
                STR = ReGexTop.Match(STR).Value

                Dim ReGex As New System.Text.RegularExpressions.Regex(".*?(<h3 class=""heading"">|</main>)")
                Dim ColorText, CategoryContent As String : ColorText = "#00000" : CategoryContent = "OTHER"

                For Each ReGx As System.Text.RegularExpressions.Match In ReGex.Matches(STR)
                    Dim SearchItog As Boolean = False

                    Dim RegexCategory As New System.Text.RegularExpressions.Regex("(?<=^).*?(?=</h3>)")

                    Dim Item As New Item
                    Item.Type = ItemType.DIRECTORY
                    Dim RegNENaiden As New System.Text.RegularExpressions.Regex("не найдены")
                    Select Case UCase(RegexCategory.Match(ReGx.Value).Value)
                        Case "СЕРИАЛЫ"
                            If RegNENaiden.IsMatch(ReGx.Value) = True Then
                                'Item.Name = "<span style=""color:#979797""><strong>СЕРИАЛЫ</strong></span>"
                                'Item.ImageLink = "http://s1.iconbird.com/ico/2013/6/319/w128h1281371898010folderblack.png"
                            Else
                                Nom = ""
                                Item.Name = "<span style=""color:#3090F0""><u><strong>СЕРИАЛЫ</strong></u></span>"
                                Item.ImageLink = "http://s1.iconbird.com/ico/2013/6/319/w128h1281371898000folder.png"
                                ColorText = "#E0EEFC"
                                CategoryContent = "PAGE_SERIAL"
                                Item.Link = ""
                                '   Item.Description = ReGx.Value.Replace("<img", "<img width=""200""")
                                '  items.Add(Item)
                                SearchItog = True
                            End If
                        Case "ФИЛЬМЫ"
                            If RegNENaiden.IsMatch(ReGx.Value) = True Then
                                'Item.Name = "<span style=""color:#979797""><strong>ФИЛЬМЫ</strong></span>"
                                'Item.ImageLink = "http://s1.iconbird.com/ico/2013/6/319/w128h1281371898010folderblack.png"
                            Else
                                Nom = ""
                                Item.Name = "<span style=color:#86F4EB><u><strong>ФИЛЬМЫ</strong></u></span>"
                                Item.ImageLink = "http://s1.iconbird.com/ico/2013/6/319/w128h1281371898069foldergreen.png"
                                ColorText = "#DFFDFB"
                                CategoryContent = "PAGE_FILM"
                                '   Item.Description = ReGx.Value.Replace("<img", "<img width=""200""")
                                Item.Link = ""

                                '   items.Add(Item)
                                SearchItog = True
                            End If
                        Case "ТЕЛЕКАНАЛЫ"
                            If RegNENaiden.IsMatch(ReGx.Value) = True Then
                                'Item.Name = "<span style=""color:#979797""><strong>ТЕЛЕКАНАЛЫ</strong></span>"
                                'Item.ImageLink = "http://s1.iconbird.com/ico/2013/6/319/w128h1281371898010folderblack.png"
                            Else
                                Nom = ""
                                Item.Name = "<span style=""color:#CA91EB""><u><strong>ТЕЛЕКАНАЛЫ</strong></u></span>"
                                Item.ImageLink = "http://s1.iconbird.com/ico/2013/6/319/w128h1281371898216folderviolet.png"
                                ColorText = "#F2E5F9"
                                CategoryContent = "PAGE_TV"
                                Item.Link = ""
                                ' Item.Description = ReGx.Value.Replace("<img", "<img width=""200""")
                                ' items.Add(Item)
                                SearchItog = True
                            End If
                        Case "ОЗВУЧКИ"
                            If RegNENaiden.IsMatch(ReGx.Value) = True Then
                                'Item.Name = "<span style=""color:#979797""><strong>ОЗВУЧКИ</strong></span>"
                                'Item.ImageLink = "http://s1.iconbird.com/ico/2013/6/319/w128h1281371898010folderblack.png"
                            Else
                                Nom = "1;"
                                Item.Name = "<span style=""color:#F491B3""><u><strong>ОЗВУЧКИ</strong></u></span>"
                                Item.ImageLink = "http://s1.iconbird.com/ico/2013/6/319/w128h1281371898172folderred.png"
                                ColorText = "#F4DFE6"
                                CategoryContent = "PAGE_OTZVUCH"
                                Item.Link = ""
                                '  Item.Description = ReGx.Value.Replace("<img", "<img width=""200""")
                                '  items.Add(Item)
                                SearchItog = True
                            End If
                        Case "АКТЕРЫ"
                            If RegNENaiden.IsMatch(ReGx.Value) = True Then
                                'Item.Name = "<span style=""color:#979797""><strong>АКТЕРЫ</strong></span>"
                                'Item.ImageLink = "http://s1.iconbird.com/ico/2013/6/319/w128h1281371898010folderblack.png"
                            Else
                                Nom = ""
                                Item.Name = "<span style=""color:#E7F78F""><u><strong>АКТЕРЫ</strong></u></span>"
                                Item.ImageLink = "http://s1.iconbird.com/ico/2013/6/319/w128h1281371898226folderyellow.png"
                                ColorText = "#F4F8DF"
                                CategoryContent = "PAGE_AKTER"
                                Item.Link = ""
                                ' Item.Description = ReGx.Value.Replace("<img", "<img width=""200""")
                                '  items.Add(Item)
                                SearchItog = True
                            End If

                    End Select

                    If SearchItog = True Then


                        Dim ReGexFilm = New System.Text.RegularExpressions.Regex("(href=""|<div class=""row6 spad"">|<div class=""col-18 spad"">).*?(</a> </div>|</a>                     </div>|</a>     </div>      </div>)")
                        For Each ReGxFilm As System.Text.RegularExpressions.Match In ReGexFilm.Matches(ReGx.Value)
                            Dim RegLink As New System.Text.RegularExpressions.Regex("(?<=href="").*?(?="")")
                            Dim RegName As New System.Text.RegularExpressions.Regex("(?<=alt="").*?(?="")")
                            Dim RegImage As New System.Text.RegularExpressions.Regex("(?<=<img src=""|src="").*?(?="")")
                            Dim RegGanreF As New System.Text.RegularExpressions.Regex("(?<=<div class=""genre h60"">).*?(?=</div>)")
                            Dim RegGanreS As New System.Text.RegularExpressions.Regex("(?<=genre"">).*?(?=</div>)")

                            Item = New Item
                            Item.Link = RegLink.Match(ReGxFilm.Value).Value & ";" & Nom & CategoryContent
                            Item.Type = ItemType.DIRECTORY
                            Item.Name = "<span style=color:" & ColorText & ">" & RegName.Match(ReGxFilm.Value).Value & "</span>"
                            Item.ImageLink = RegImage.Match(ReGxFilm.Value).Value
                            ' Item.Description = "<div id=""poster"" style=""float:left;padding:4px;        background-color:#EEEEEE;margin:0px 13px 1px 0px;""><img src=""" & Item.ImageLink & """ style=float:left;"" /></div><span style=""color:#3090F0"">" & Item.Name & "</span><br><font face=""Arial Narrow"" size=""4""><span style=""color:#70A4A3"">" & "</font></span>" & RegGanreF.Match(ReGxFilm.Value).Value & RegGanreS.Match(ReGxFilm.Value).Value
                            Item.Description = New Text.RegularExpressions.Regex("(<img src="").*").Match(ReGxFilm.Value).Value & "<br>" & UCase(RegexCategory.Match(ReGx.Value).Value)
                            items.Add(Item)
                        Next
                    End If
                Next

            End If

            PlayList.IsIptv = "False"
            Return PlayListPlugPar(items, context)
        End Function



#End Region

#Region "ЗАПРОСЫ"

        Public Function ReqHTML(ByVal URL As String, Optional ByVal DatStr As String = "", Optional ByVal Method As String = "GET")
            If Left(URL, 1) = "/" Then
                URL = AdressTvFeed & URL
            End If
            Dim Req As Net.HttpWebRequest = Net.HttpWebRequest.Create(URL)

            Req.Referer = AdressTvFeed & "/dashboard/"
            Req.ContentType = "application/x-www-form-urlencoded"
            Req.Headers.Add("cookie", "csrftoken=" & Cookie)
            Req.Headers.Add("x-requested-with", "XMLHttpRequest")

            Select Case UCase(Method)
                Case "GET"
                    Req.Method = Method
                Case "POST"
                    Req.Method = Method
                    Dim myStream As System.IO.Stream = Req.GetRequestStream
                    Dim DataStr As String = "q=" & DatStr & "&csrfmiddlewaretoken=" & Cookie
                    Dim DataByte As Byte() = System.Text.Encoding.UTF8.GetBytes(DataStr)
                    myStream.Write(DataByte, 0, DataByte.Length)
                    myStream.Close()
            End Select

            Dim Res As Net.HttpWebResponse = Req.GetResponse
            Dim Reader As New IO.StreamReader(Res.GetResponseStream)
            Dim STR As String = Reader.ReadToEnd.Replace("acestream://", "").Replace(vbLf, " ").Replace("src=""/", "src=""" & AdressTvFeed & "/")

            Reader.Close()
            Res.Close()
            Return STR
        End Function

#End Region

#Region "СЕРИАЛЫ"

        Public Function GetListSerialezes(ByVal context As IPluginContext, ByVal URL As String, ByVal NomerPage As Integer) As PluginApi.Plugins.Playlist
            Dim items As New System.Collections.Generic.List(Of Item)
            Dim ColorText, CategoryContent As String : ColorText = "#E0EEFC" : CategoryContent = "PAGE_SERIAL"
            Dim STR As String = ReqHTML(URL & "/?page=" & NomerPage)

            Dim RegexSerials As New System.Text.RegularExpressions.Regex("(<a class=""serial-short container"").*?(</div>     </a> </div>)")
            Dim RegexName As New System.Text.RegularExpressions.Regex("(?<=<h4 itemprop=""name"">).*?(?=<)")
            Dim RegexImage As New System.Text.RegularExpressions.Regex("(?<=<img src="").*?(?="")")
            Dim RegexDescript As New System.Text.RegularExpressions.Regex("(<img src=).*")
            Dim RegexLink As New System.Text.RegularExpressions.Regex("(?<=href="").*?(?="")")


            For Each Reg As System.Text.RegularExpressions.Match In RegexSerials.Matches(STR)
                Dim Item As New Item
                With Item
                    .Type = ItemType.DIRECTORY
                    .Name = RegexName.Match(Reg.Value).Value
                    .ImageLink = RegexImage.Match(Reg.Value).Value
                    .Description = RegexDescript.Match(Reg.Value).Value & "<br>" & "СЕРИАЛЫ"
                    Item.Link = RegexLink.Match(Reg.Value).Value & ";" & CategoryContent

                    items.Add(Item)
                End With
            Next

            next_page_url = URL & ";" & NomerPage + 1 & ";SERIALEZESLIST"
            PlayList.IsIptv = "False"
            Return PlayListPlugPar(items, context, next_page_url)
        End Function

        Public Function GetSezon(ByVal context As IPluginContext, ByVal URL As String, ByVal Description As String) As PluginApi.Plugins.Playlist
            Dim items As New System.Collections.Generic.List(Of Item)
            Dim STR As String = ReqHTML(URL)

            Dim Regex As New System.Text.RegularExpressions.Regex("(<h4 class=""tcenter"">Выберите плеер</h4>).*?(<div class=""col-auto info ellipsis"">)")
            Dim RegexName As New System.Text.RegularExpressions.Regex("(?<=<p class=""title"">).*?(?=</p>)")
            Dim RegexLink As New System.Text.RegularExpressions.Regex("(?<=data="").*?(?="")")
            Dim RegexDescript As New System.Text.RegularExpressions.Regex("(<div class=""container vpad20 tcenter"">).*?(?=<script)")


            For Each Reg As System.Text.RegularExpressions.Match In Regex.Matches(STR)

                Dim Item As New Item
                With Item
                    .Type = ItemType.DIRECTORY
                    .Name = RegexName.Match(Reg.Value).Value
                    Dim TXT As New System.Text.UTF8Encoding()
                    .Description = TXT.GetString(Convert.FromBase64String(Description))
                    .ImageLink = New System.Text.RegularExpressions.Regex("(?<=<img src="").*?(?="")").Match(.Description).Value
                    .Link = RegexLink.Match(Reg.Value).Value & ";" & Description & ";FILM"
                    items.Add(Item)
                End With
            Next

            Dim RegexError As New System.Text.RegularExpressions.Regex("(?<=f24"">).*?(?=</)")
            If RegexError.IsMatch(STR) = True Then
                Dim Item As New Item
                With Item
                    .Type = ItemType.DIRECTORY
                    .Name = RegexError.Match(STR).Value
                    .ImageLink = ICO_Error
                    items.Add(Item)
                End With
            End If

            PlayList.IsIptv = "False"
            Return PlayListPlugPar(items, context)
        End Function

        Public Function GetPageSezon(ByVal context As IPluginContext, ByVal URL As String, ByVal Description As String) As PluginApi.Plugins.Playlist
            Dim items As New System.Collections.Generic.List(Of Item)
            Dim STR As String = ReqHTML(URL)


            Dim RegexOzvuch As New System.Text.RegularExpressions.Regex("(<figure>).*?(""/>)")
            Dim RegexImage As New System.Text.RegularExpressions.Regex("(?<=<img src="").*?(?="")")
            Dim RegexName As New System.Text.RegularExpressions.Regex("(?<=title="").*?(?="")")
            Dim RegexLink As New System.Text.RegularExpressions.Regex("(?<=<a href="").*?(?="")")
            Dim RegexReiting As New System.Text.RegularExpressions.Regex("(?<=<a href=""/voice/rating/"" title=""Рейтинг озвучек"">).*?(?=</a>)")

            For Each Reg As System.Text.RegularExpressions.Match In RegexOzvuch.Matches(STR)
                Dim Item As New Item
                With Item
                    .Type = ItemType.DIRECTORY

                    .ImageLink = RegexImage.Match(Reg.Value).Value
                    .Name = RegexName.Match(Reg.Value).Value

                    Select Case UCase(RegexReiting.Match(Reg.Value).Value)
                        Case "ТОП-1"
                            .Name = .Name & "<span style=""color:#FFEA5E""><br>Рейтинг озвучек: ТОП-1</span>"
                        Case "ТОП-2"
                            .Name = .Name & "<span style=""color:#748289""><br>Рейтинг озвучек: ТОП-2</span>"
                        Case "ТОП-3"
                            .Name = .Name & "<span style=""color:#8B7E75""><br>Рейтинг озвучек: ТОП-3</span>"
                    End Select
                    Dim TXT As New System.Text.UTF8Encoding()
                    .Description = TXT.GetString(Convert.FromBase64String(Description)).Replace("||", .Name)
                    .Link = RegexLink.Match(Reg.Value).Value & ";" & Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(.Description)) & ";SEZONE"
                    items.Add(Item)
                End With

            Next


            Dim RegexError As New System.Text.RegularExpressions.Regex("(?<=<h4>).*?(?=</)")
            If RegexError.IsMatch(STR) = True Then
                Dim Item As New Item
                With Item
                    .Type = ItemType.DIRECTORY
                    .Name = RegexError.Match(STR).Value
                    .ImageLink = ICO_Error
                    items.Add(Item)
                End With
            End If

            PlayList.IsIptv = "False"
            Return PlayListPlugPar(items, context)
        End Function

        Public Function GetRandomSerial(ByVal context As IPluginContext) As PluginApi.Plugins.Playlist
            Return GetPageSerial(context, New System.Text.RegularExpressions.Regex("(?<=href="").*?(?="")").Match(ReqHTML("/serial/random/")).Value, ";RANDOMSERIAL")
        End Function

        Public Function GetPageSerial(ByVal context As IPluginContext, ByVal URL As String, ByVal Optional NextPage As String = "") As PluginApi.Plugins.Playlist

            Dim items As New System.Collections.Generic.List(Of Item)
            Dim STR As String = ReqHTML(URL)

            Dim Regex As New System.Text.RegularExpressions.Regex("(<h3>Сезоны сериала</h3>).*?(<div class=""container vpad30"">)")
            Dim SezonsStr As String = Regex.Match(STR).ToString



            Dim RegexSezons As New System.Text.RegularExpressions.Regex("(<div class=""row5 spad"" itemprop=""containsSeason"" itemscope itemtype=""http://schema.org/TVSeason"">).*?(</h4>)")
            Dim RegexImage As New System.Text.RegularExpressions.Regex("(?<=<img src="").*?(?="")")
            Dim RegexName As New System.Text.RegularExpressions.Regex("(?<=title="").*?(?="")")
            Dim RegexLink As New System.Text.RegularExpressions.Regex("(?<=<a href="").*?(?="")")
            Dim RegexDescript As New System.Text.RegularExpressions.Regex("(?<=<div class=""about"" itemprop=""about"">).*?(</p>)")
            Dim Description As String = RegexDescript.Match(STR).Value


            Dim RegexInfo As New System.Text.RegularExpressions.Regex("(<div class=""col-auto serial_info hpad50"">).*?(</div>)")
            Dim StrInfo As String = ""
            If RegexInfo.IsMatch(STR) = True Then
                Dim InfoHTMLstr As String = RegexInfo.Match(STR).Value
                StrInfo = "<p><span style="" color: #FFF000; font-size: 12; font-style: italic"""
                Dim RegexTitle As New System.Text.RegularExpressions.Regex("(?<=title="").*?(?="")")
                For Each Reg As System.Text.RegularExpressions.Match In RegexTitle.Matches(InfoHTMLstr)
                    StrInfo = StrInfo & "<br>" & Reg.Value
                Next
                StrInfo = StrInfo & "</span>"
            End If

            For Each Reg As System.Text.RegularExpressions.Match In RegexSezons.Matches(SezonsStr)
                Dim Item As New Item
                With Item
                    .Type = ItemType.DIRECTORY
                    .ImageLink = RegexImage.Match(Reg.Value).Value
                    .Name = RegexName.Match(Reg.Value).Value
                    .Description = "<div id=""poster"" style=""float:left;padding:4px;  background-color:#EEEEEE;margin:0px 13px 1px 0px;""><img src=""" & .ImageLink _
                        & """ style=""width:240px;float:left;""/></div><span style=""color:#3090F0"">" & .Name & "</span><span>" & Description & "</span>" & StrInfo
                    Dim Descriptions As String = "<div id=""poster"" style=""float:left;padding:4px;  background-color:#EEEEEE;margin:0px 13px 1px 0px;""><img src=""" & .ImageLink _
                        & """ style=""width:240px;float:left;""/></div><span style=""color:#3090F0"">||</span><span>" & Description & "</span>" & StrInfo
                    Descriptions = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(Descriptions))
                    .Link = RegexLink.Match(Reg.Value).Value & ";" & Descriptions & ";SEZON_PAGE"
                    items.Add(Item)
                End With
            Next

            Dim RegAkters As New System.Text.RegularExpressions.Regex("(<h3>Актеры и персонажи</h3>).*?(<div class=""container vpad20"">)")
            If RegAkters.IsMatch(STR) = True Then
                Dim Item As New Item
                Item.ImageLink = ICO_Pusto
                items.Add(Item)
                Dim RegAkter As New System.Text.RegularExpressions.Regex("(<a href="").*?(</a>)")
                For Each Reg As System.Text.RegularExpressions.Match In RegAkter.Matches(RegAkters.Match(STR).Value)
                    Item = New Item
                    Item.Type = ItemType.DIRECTORY
                    Item.ImageLink = New System.Text.RegularExpressions.Regex("(?<=<img src="").*?(?="")").Match(Reg.Value).Value
                    Item.Name = New System.Text.RegularExpressions.Regex("(?<=title="").*?(?="")").Match(Reg.Value).Value
                    Item.Link = New System.Text.RegularExpressions.Regex("(?<=<a href="").*?(?="")").Match(Reg.Value).Value & ";PAGE_AKTER"
                    Item.Description = "<div id=""poster"" style=""float:left;padding:4px;        background-color:#EEEEEE;margin:0px 13px 1px 0px;""><img src=""" & Item.ImageLink & """ style=float:left;"" /></div><span style=""color:#3090F0"">" & Item.Name & "</span><br><font face=""Arial Narrow"" size=""4""><span style=""color:#70A4A3"">" & "</font></span>"
                    items.Add(Item)
                Next
            End If


            PlayList.IsIptv = "False"
            Return PlayListPlugPar(items, context, NextPage)
        End Function
#End Region

#Region "ФИЛЬМЫ"
        Public Function GetRandomFilm(ByVal context As IPluginContext) As PluginApi.Plugins.Playlist
            Return GetPageFilm(context, New System.Text.RegularExpressions.Regex("(?<=href="").*?(?="")").Match(ReqHTML("/film/random/")).Value, ";RANDOMFILM")
        End Function

        Public Function GetListFilmes(ByVal context As IPluginContext, ByVal URL As String, ByVal NomerPage As Integer) As PluginApi.Plugins.Playlist
            Dim items As New System.Collections.Generic.List(Of Item)
            Dim ColorText, CategoryContent As String : ColorText = "#DFFDFB" : CategoryContent = "PAGE_FILM"
            Dim STR As String = ReqHTML(URL & "/?page=" & NomerPage)

            Dim RegexFilmes As New System.Text.RegularExpressions.Regex("(<div class=""row6 spad"">).*?(</div>     </a> </div>)")
            Dim RegexName As New System.Text.RegularExpressions.Regex("(?<=alt="").*?(?="")")
            Dim RegexImage As New System.Text.RegularExpressions.Regex("(?<=<img src="").*?(?="")")
            Dim RegexDescript As New System.Text.RegularExpressions.Regex("(<img src=).*")
            Dim RegexLink As New System.Text.RegularExpressions.Regex("(?<=href="").*?(?="")")

            For Each Reg As System.Text.RegularExpressions.Match In RegexFilmes.Matches(STR)
                Dim Item As New Item
                With Item
                    .Type = ItemType.DIRECTORY
                    .Name = RegexName.Match(Reg.Value).Value
                    .ImageLink = RegexImage.Match(Reg.Value).Value
                    .Description = RegexDescript.Match(Reg.Value).Value & "<br>" & "ФИЛЬМЫ"
                    Item.Link = RegexLink.Match(Reg.Value).Value & ";" & CategoryContent

                    items.Add(Item)
                End With
            Next

            next_page_url = URL & ";" & NomerPage + 1 & ";FILMESLIST"
            PlayList.IsIptv = "False"
            Return PlayListPlugPar(items, context, next_page_url)
        End Function

        Public Function GetPageFilm(ByVal context As IPluginContext, ByVal URL As String, ByVal Optional NextPage As String = "") As PluginApi.Plugins.Playlist
            Dim items As New System.Collections.Generic.List(Of Item)
            Dim STR As String = ReqHTML(URL)

            Dim Regex As New System.Text.RegularExpressions.Regex("(<div class=""col-14"">).*?(title=""Открыть в Ace Player"">)")
            Dim RegexLink As New System.Text.RegularExpressions.Regex("(?<=data="").*?(?="")")
            Dim RegexName As New System.Text.RegularExpressions.Regex("(?<=<p class=""title"">).*?(?=</p>)")
            Dim Descript As String = FormatDescriptFilm(STR)
            Dim Image As String = New System.Text.RegularExpressions.Regex("(?<=<div class=""container"">                     <img src="").*?(?="")").Match(STR).Value


            For Each Reg As System.Text.RegularExpressions.Match In Regex.Matches(STR)
                Dim Item As New Item
                With Item
                    .Type = ItemType.DIRECTORY
                    .ImageLink = Image
                    .Description = Descript
                    .Name = RegexName.Match(Reg.Value).Value
                    .Link = RegexLink.Match(Reg.Value).Value & ";" & Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(.Description)) & ";FILM"
                    items.Add(Item)
                End With
            Next
            '  Dim RegexErrorer As New System.Text.RegularExpressions.Regex("(<div class=""col-14"">).*?(title=""Открыть в Ace Player"">)")
            ''
            ''''
            Dim RegRegisers As New System.Text.RegularExpressions.Regex("(<div class=""container flex-container centered circled vpad20"">).*?(?=<div class=""container tcenter vpad20"">)")
            If RegRegisers.IsMatch(STR) = True Then
                Dim Item As New Item
                Item.ImageLink = ICO_Pusto
                items.Add(Item)
                Dim RegRgiser As New System.Text.RegularExpressions.Regex("(<div class=""column"">).*?(</div>)")
                For Each Reg As System.Text.RegularExpressions.Match In RegRgiser.Matches(RegRegisers.Match(STR).Value)
                    Item = New Item
                    Item.Type = ItemType.DIRECTORY
                    Item.ImageLink = New System.Text.RegularExpressions.Regex("(?<=<img src="").*?(?="")").Match(Reg.Value).Value
                    Item.Name = New System.Text.RegularExpressions.Regex("(?<=title="").*?(?="")").Match(Reg.Value).Value
                    Item.Link = New System.Text.RegularExpressions.Regex("(?<=<a href="").*?(?="")").Match(Reg.Value).Value & ";PAGE_AKTER" '";PAGE_REZISOR"
                    Item.Description = "<div id=""poster"" style=""float:left;padding:4px;        background-color:#EEEEEE;margin:0px 13px 1px 0px;""><img src=""" & Item.ImageLink & """ style=float:left;"" /></div><span style=""color:#3090F0"">" & Item.Name & "</span><br><font face=""Arial Narrow"" size=""4""><span style=""color:#70A4A3"">" & "</font></span>"
                    items.Add(Item)
                Next
            End If


            Dim RegAkters As New System.Text.RegularExpressions.Regex("(<h3>Актеры и персонажи</h3>).*?(<div class=""container vpad20"">)")
            If RegAkters.IsMatch(STR) = True Then
                Dim Item As New Item
                Item.ImageLink = ICO_Pusto
                items.Add(Item)
                Dim RegAkter As New System.Text.RegularExpressions.Regex("(<a href="").*?(</a>)")
                For Each Reg As System.Text.RegularExpressions.Match In RegAkter.Matches(RegAkters.Match(STR).Value)
                    Item = New Item
                    Item.Type = ItemType.DIRECTORY
                    Item.ImageLink = New System.Text.RegularExpressions.Regex("(?<=<img src="").*?(?="")").Match(Reg.Value).Value
                    Item.Name = New System.Text.RegularExpressions.Regex("(?<=title="").*?(?="")").Match(Reg.Value).Value
                    Item.Link = New System.Text.RegularExpressions.Regex("(?<=<a href="").*?(?="")").Match(Reg.Value).Value & ";PAGE_AKTER"
                    Item.Description = "<div id=""poster"" style=""float:left;padding:4px;        background-color:#EEEEEE;margin:0px 13px 1px 0px;""><img src=""" & Item.ImageLink & """ style=float:left;"" /></div><span style=""color:#3090F0"">" & Item.Name & "</span><br><font face=""Arial Narrow"" size=""4""><span style=""color:#70A4A3"">" & "</font></span>"
                    items.Add(Item)
                Next
            End If


            PlayList.IsIptv = "False"
            Return PlayListPlugPar(items, context, NextPage)
        End Function
        Public Function GetFilm(ByVal context As IPluginContext, ByVal URL As String, ByVal Description As String) As PluginApi.Plugins.Playlist
            Dim items As New System.Collections.Generic.List(Of Item)

            Dim PlayListtoTorrent() As TorrentPlayList = GetFileList(URL)

            For Each PlayListItem As TorrentPlayList In PlayListtoTorrent

                Dim Item As New Item
                With Item
                    .Name = PlayListItem.Name
                    .ImageLink = PlayListItem.ImageLink
                    .Link = PlayListItem.Link
                    .Type = ItemType.FILE
                    Dim TXT As New System.Text.UTF8Encoding()
                    .Description = TXT.GetString(Convert.FromBase64String(Description))
                End With
                items.Add(Item)
            Next

            PlayList.IsIptv = "False"
            Return PlayListPlugPar(items, context)
        End Function
        Private Function FormatDescriptFilm(ByVal HTM As String) As String

            Dim RegexDescript As New System.Text.RegularExpressions.Regex("(?<=<div class=""about"" itemprop=""about"">                         <p>).*?(?=</p>)")
            Dim RegexImage As New System.Text.RegularExpressions.Regex("(?<=<div class=""container"">                     <img src="").*?(?="")")
            Dim RegexName As New System.Text.RegularExpressions.Regex("(?<=<meta charset=""utf-8"">          <title>).*?(?=🎞</title>)")
            Dim RegexInfo As New System.Text.RegularExpressions.Regex("(<div class=""col-auto serial_info hpad50"">).*?(</div>)")


            Dim StrInfo As String = ""

            If RegexInfo.IsMatch(HTM) = True Then
                Dim InfoHTMLstr As String = RegexInfo.Match(HTM).Value
                StrInfo = "<p><span style="" color: #CBCAFF; font-style:oblique; font-size: 20"" > " & New System.Text.RegularExpressions.Regex("(?<=<div class=""container vpad10 additional thin"">).*?(?=</div>)").Match(InfoHTMLstr).Value _
                    & "</span></p><p><span style="" color: #FFF000; font-size: 12; font-style: italic"""
                Dim RegexTitle As New System.Text.RegularExpressions.Regex("(?<=title="").*?(?="")")

                For Each Reg As System.Text.RegularExpressions.Match In RegexTitle.Matches(InfoHTMLstr)
                    StrInfo = StrInfo & "<br>" & Reg.Value
                Next
                StrInfo = StrInfo & "</span>"
            End If
            Return "<div id=""poster"" style=""float:left;padding:4px;  background-color:#EEEEEE;margin:0px 13px 1px 0px;""><img src=""" & RegexImage.Match(HTM).Value _
                & """ style=""width:240px;float:left;"" /></div><span style=""color:#3090F0"">" & RegexName.Match(HTM).Value & "</span><br><span>" & RegexDescript.Match(HTM).Value & "</span>" & StrInfo
        End Function
#End Region

#Region "ТВ"
        Public Function GetRandomTV(ByVal context As IPluginContext) As PluginApi.Plugins.Playlist
            Return GetPageTV(context, New System.Text.RegularExpressions.Regex("(?<=href="").*?(?="")").Match(ReqHTML("/tv/random/")).Value, ";RANDOMTV")
        End Function

        Public Function GetListTV(ByVal context As IPluginContext, ByVal URL As String, ByVal NomerPage As Integer) As PluginApi.Plugins.Playlist
            Dim items As New System.Collections.Generic.List(Of Item)
            Dim ColorText, CategoryContent As String : ColorText = "#F2E5F9" : CategoryContent = "PAGE_TV"
            Dim STR As String = ReqHTML(URL & "/?page=" & NomerPage)


            Dim RegexTV As New System.Text.RegularExpressions.Regex("(<div class=""row6 spad"">).*?(</div>     </a> </div>)")
            Dim RegexName As New System.Text.RegularExpressions.Regex("(?<=alt="").*?(?="")")
            Dim RegexImage As New System.Text.RegularExpressions.Regex("(?<=<img src="").*?(?="")")
            Dim RegexDescript As New System.Text.RegularExpressions.Regex("(<img src=).*")
            Dim RegexLink As New System.Text.RegularExpressions.Regex("(?<=href="").*?(?="")")

            For Each Reg As System.Text.RegularExpressions.Match In RegexTV.Matches(STR)
                Dim Item As New Item
                With Item
                    .Type = ItemType.DIRECTORY
                    .Name = RegexName.Match(Reg.Value).Value
                    .ImageLink = RegexImage.Match(Reg.Value).Value
                    .Description = RegexDescript.Match(Reg.Value).Value & "<br>" & "ТЕЛЕКАНАЛЫ"
                    Item.Link = RegexLink.Match(Reg.Value).Value & ";" & CategoryContent

                    items.Add(Item)
                End With
            Next

            next_page_url = URL & ";" & NomerPage + 1 & ";TVS"
            PlayList.IsIptv = "False"
            Return PlayListPlugPar(items, context, next_page_url)
        End Function

        Public Function GetPageTV(ByVal context As IPluginContext, ByVal URL As String, ByVal Optional NextPage As String = "") As PluginApi.Plugins.Playlist
            Dim items As New System.Collections.Generic.List(Of Item)
            Dim STR As String = ReqHTML(URL)


            Dim Regex As New System.Text.RegularExpressions.Regex("(<h4 class=""tcenter"">Выберите плеер</h4>).*?(качество</p>)")
            Dim RegexName As New System.Text.RegularExpressions.Regex("(?<=<p class=""title"">).*?(?=</p>)")
            Dim RegexInfo As New System.Text.RegularExpressions.Regex("(?<=<div class=""col-auto info ellipsis"">).*?(?=</div>)")
            Dim RegexLink As New System.Text.RegularExpressions.Regex("(?<=data="").*?(?="")")
            Dim ImageLink As String = New System.Text.RegularExpressions.Regex("(?<=<div class=""container"">                     <img src="").*?(?="")").Match(STR).Value
            Dim RegexDescript As New System.Text.RegularExpressions.Regex("(<div class=""col-auto serial_info hpad50"">).*?(?=<script)")

            For Each Reg As System.Text.RegularExpressions.Match In Regex.Matches(STR)
                Dim Item As New Item
                With Item
                    .Type = ItemType.DIRECTORY
                    .Name = RegexName.Match(Reg.Value).Value & " " & RegexInfo.Match(Reg.Value).Value
                    .Description = RegexDescript.Match(STR).Value
                    .ImageLink = ImageLink
                    .Link = RegexLink.Match(Reg.Value).Value & ";" & Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(.Description)) & ";FILM"
                    items.Add(Item)
                End With
            Next

            PlayList.IsIptv = "False"
            Return PlayListPlugPar(items, context, NextPage)
        End Function
#End Region

#Region "ОЗВУЧКИ"
        Public Function GetPageOzvuch(ByVal context As IPluginContext, ByVal URL As String, ByVal NomerPage As Integer) As PluginApi.Plugins.Playlist
            Dim items As New System.Collections.Generic.List(Of Item)
            Dim ColorText, CategoryContent As String : ColorText = "#00000" : CategoryContent = "OTHER"
            Dim STR As String = ReqHTML(URL & "/feed/?page=" & NomerPage)


            Dim RegexSerials As New System.Text.RegularExpressions.Regex("(<div class=""container season-short row3"" itemscope itemtype=""http://schema.org/TVSeries"">).*?(</div>     </div>)")
            Dim RegexName As New System.Text.RegularExpressions.Regex("(?<=title="").*?(?="")")
            Dim RegexImage As New System.Text.RegularExpressions.Regex("(?<=<img src="").*?(?="")")
            Dim RegexDescript As New System.Text.RegularExpressions.Regex("(<img src=).*")
            Dim RegexLink As New System.Text.RegularExpressions.Regex("(?<=<a class=""quicklink"" href="").*?(?="")")

            If RegexSerials.IsMatch(STR) = True Then
                Dim Item As New Item
                Item.Name = "<span style=""color:#3090F0""><u><strong>СЕРИАЛЫ</strong></u></span>"
                Item.ImageLink = "http://s1.iconbird.com/ico/2013/6/319/w128h1281371898000folder.png"
                ColorText = "#E0EEFC"
                CategoryContent = "SEZON_PAGE"
                Item.Link = ""
                items.Add(Item)
            End If

            For Each Reg As System.Text.RegularExpressions.Match In RegexSerials.Matches(STR)
                Dim Item As New Item
                With Item
                    .Type = ItemType.DIRECTORY
                    .Name = RegexName.Matches(Reg.Value)(1).Value
                    .ImageLink = RegexImage.Match(Reg.Value).Value
                    .Description = RegexDescript.Match(Reg.Value).Value
                    Item.Link = RegexLink.Match(Reg.Value).Value & ";" & Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(.Description)) & ";" & CategoryContent
                    items.Add(Item)
                End With
            Next

            next_page_url = URL & ";" & NomerPage + 1 & ";PAGE_OTZVUCH"
            PlayList.IsIptv = "False"
            Return PlayListPlugPar(items, context, next_page_url)
        End Function
#End Region

#Region "АКТЁРЫ"
        Public Function GetPageAkter(ByVal context As IPluginContext, ByVal URL As String) As PluginApi.Plugins.Playlist
            Dim items As New System.Collections.Generic.List(Of Item)
            Dim STR As String = ReqHTML(URL)
            Dim ColorText, CategoryContent As String : ColorText = "#00000" : CategoryContent = "OTHER"

            Dim RegexSerials As New System.Text.RegularExpressions.Regex("(<div class=""row4 spad"" itemscope itemtype=""http://schema.org/TVSeries"">).*?(</div>     </a> </div>)")
            Dim RegexName As New System.Text.RegularExpressions.Regex("(?<=<h4 itemprop=""name"">).*?(?=<)")
            Dim RegexImage As New System.Text.RegularExpressions.Regex("(?<=<img src="").*?(?="")")
            Dim RegexDescript As New System.Text.RegularExpressions.Regex("(<img src=).*")
            Dim RegexLink As New System.Text.RegularExpressions.Regex("(?<=href="").*?(?="")")

            If RegexSerials.IsMatch(STR) = True Then
                Dim Item As New Item
                Item.Name = "<span style=""color:#3090F0""><u><strong>СЕРИАЛЫ</strong></u></span>"
                Item.ImageLink = "http://s1.iconbird.com/ico/2013/6/319/w128h1281371898000folder.png"
                ColorText = "#E0EEFC"
                CategoryContent = "PAGE_SERIAL"
                Item.Link = ""
                '  items.Add(Item)
            End If

            For Each Reg As System.Text.RegularExpressions.Match In RegexSerials.Matches(STR)
                Dim Item As New Item
                With Item
                    .Type = ItemType.DIRECTORY
                    .Name = RegexName.Match(Reg.Value).Value
                    .ImageLink = RegexImage.Match(Reg.Value).Value
                    .Description = RegexDescript.Match(Reg.Value).Value & "<br>" & "СЕРИАЛЫ"
                    Item.Link = RegexLink.Match(Reg.Value).Value & ";" & CategoryContent
                    items.Add(Item)
                End With
            Next

            Dim RegexFilms As New System.Text.RegularExpressions.Regex("(<div class=""row4 spad"">).*?(</div>     </a> </div>)")

            RegexName = New System.Text.RegularExpressions.Regex("(?<=<h4>).*?(?=<)")
            If RegexFilms.IsMatch(STR) = True Then
                Dim Item As New Item
                Item.Name = "<span style=color:#86F4EB><u><strong>ФИЛЬМЫ</strong></u></span>"
                Item.ImageLink = "http://s1.iconbird.com/ico/2013/6/319/w128h1281371898069foldergreen.png"
                ColorText = "#DFFDFB"
                CategoryContent = "PAGE_FILM"
                Item.Link = ""
                '   items.Add(Item)
            End If

            For Each Reg As System.Text.RegularExpressions.Match In RegexFilms.Matches(STR)
                Dim Item As New Item
                With Item
                    .Type = ItemType.DIRECTORY
                    .Name = RegexName.Match(Reg.Value).Value
                    .ImageLink = RegexImage.Match(Reg.Value).Value
                    .Description = RegexDescript.Match(Reg.Value).Value & "<br>" & "ФИЛЬМЫ"
                    Item.Link = RegexLink.Match(Reg.Value).Value & ";" & CategoryContent
                    items.Add(Item)
                End With
            Next

            PlayList.IsIptv = "False"
            Return PlayListPlugPar(items, context)
        End Function
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

        Function GetFileList(ByVal PathTorrent As String) As TorrentPlayList()
            FunctionsGetTorrentPlayList = "GetFileListJSON"
            Dim WC As New System.Net.WebClient
            WC.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, Like Gecko) Chrome/57.0.2987.133 Safari/537.36")
            WC.Encoding = System.Text.UTF8Encoding.UTF8

            Dim ID As String = ReqHTML(PathTorrent)
            Dim PlayListTorrent() As TorrentPlayList = Nothing
            Dim AceMadiaInfo As String

            Select Case FunctionsGetTorrentPlayList
                Case "GetFileListJSON"
GetFileListJSON:
                    Dim CodeZnaki() As String = {"\U0430", "\U0431", "\U0432", "\U0433", "\U0434", "\U0435", "\U0451", "\U0436", "\U0437", "\U0438", "\U0439", "\U043A", "\U043B", "\U043C", "\U043D", "\U043E", "\U043F", "\U0440", "\U0441", "\U0442", "\U0443",
                    "\U0444", "\U0445", "\U0446", "\U0447", "\U0448", "\U0449", "\U044A", "\U044B", "\U044C", "\U044D", "\U044E", "\U044F", "\U0410", "\U0411", "\U0412", "\U0413", "\U0414", "\U0415", "\U0401", "\U0416", "\U0417", "\U0418", "\U0419", "\U041A",
                    "\U041B", "\U041C", "\U041D", "\U041E", "\U041F", "\U0420", "\U0421", "\U0422", "\U0423", "\U0424", "\U0425", "\U0426", "\U0427", "\U0428", "\U0429", "\U042A", "\U042B", "\U042C", "\U042D", "\U042E", "\U042F", "\U00AB", "\U00BB", "U2116"}
                    Dim DecodeZnaki() As String = {"а", "б", "в", "г", "д", "е", "ё", "ж", "з", "и", "й", "к", "л", "м", "н", "о", "п", "р", "с", "т", "у", "ф", "х", "ц", "ч", "ш", "щ", "ъ", "ы", "ь", "э", "ю", "я",
                    "А", "Б", "В", "Г", "Д", "Е", "Ё", "Ж", "З", "И", "Й", "К", "Л", "М", "Н", "О", "П", "Р", "С", "Т", "У", "Ф", "Х", "Ц", "Ч", "Ш", "Щ", "Ъ", "Ы", "Ь", "Э", "Ю", "Я", "«", "»", "№"}

                    AceMadiaInfo = WC.DownloadString("http://" & IPAdress & ":" & PortAce & "/server/api?method=get_media_files&content_id=" & ID)
                    ' MsgBox(AceMadiaInfo)
                    For I As Integer = 0 To 68
                        AceMadiaInfo = Microsoft.VisualBasic.Strings.Replace(AceMadiaInfo, Microsoft.VisualBasic.Strings.LCase(CodeZnaki(I)), DecodeZnaki(I))
                    Next
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
