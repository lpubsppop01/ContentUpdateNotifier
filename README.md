# ContentUpdateNotifier
[![Build status](https://ci.appveyor.com/api/projects/status/alu5ho1xjgy3byas?svg=true)](https://ci.appveyor.com/project/lpubsppop01/contentupdatenotifier)

This is a simple application to notify update of target files.

## Features
- Run in the system tray
- Show target list window by click the tray icon
- When registered target files are edited or removed, show toast notification
- Open folder that contains the edited or removed file by click the toast notification

## Download
[ContentUpdateNotifier Latest Build - AppVeyor](https://ci.appveyor.com/api/projects/lpubsppop01/contentupdatenotifier/artifacts/lpubsppop01.ContentUpdateNotifier_Publish_Any_CPU.zip)

## Requirements
- To use
    - .NET Framework 4.6.1 or later
    - Windows 8.1 or later
- To build
    - Visual Studio 2017

## Author
[lpubsppop01](https://github.com/lpubsppop01)

## License
[zLib License](https://github.com/lpubsppop01/ContentUpdateNotifier/raw/master/LICENSE.txt)

This software uses the following NuGet packages:
- [Microsoft.WindowsAPICodePack.Core](https://www.nuget.org/packages/Microsoft.WindowsAPICodePack-Core/)  
  (c) 2010 Microsoft Corporation  
  Released under the [Ms-PL](http://web.archive.org/web/20101226004522/http://code.msdn.microsoft.com/WindowsAPICodePack)
- [Microsoft.WindowsAPICodePack.Shell](https://www.nuget.org/packages/Microsoft.WindowsAPICodePack-Shell/)  
  (c) 2010 Microsoft Corporation  
  Released under the [Ms-PL](http://web.archive.org/web/20101226004522/http://code.msdn.microsoft.com/WindowsAPICodePack)
- [System.ValueTuple](https://www.nuget.org/packages/System.ValueTuple/)  
  Copyright (c) .NET Foundation and Contributors  
  Released under the [MIT License](https://github.com/dotnet/corefx/blob/master/LICENSE.TXT)
- [System.Data.SQLite.Core](https://www.nuget.org/packages/System.Data.SQLite.Core/)  
  Dedicated to the [Public Domain](https://www.sqlite.org/copyright.html) by the authors

## References
My initial purpose to create this app was to learn how to show tray icon and toast notification by WPF app.
The following links (Japanese site) were helpful for me:
- [C# WPF で タスクトレイ 常駐アプリ の 開発 - galife](https://garafu.blogspot.jp/2015/06/dev-tasktray-residentapplication.html)
- [非.NET4.5でもトースト通知](http://8thway.blogspot.jp/2014/02/notdotnet45-toast.html)
- [デスクトップ アプリからのWinRT API利用 | ++C++; // 未確認飛行 C ブログ](https://ufcpp.wordpress.com/2012/09/18/%E3%83%87%E3%82%B9%E3%82%AF%E3%83%88%E3%83%83%E3%83%97-%E3%82%A2%E3%83%97%E3%83%AA%E3%81%8B%E3%82%89%E3%81%AEwinrt-api%E5%88%A9%E7%94%A8/)
