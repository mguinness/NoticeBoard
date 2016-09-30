# Notice Board
A website which acts as a notice board to share information and news.  It uses [NET Core](https://www.microsoft.com/net/core) which runs on Windows, Mac and Linux and utilizes [SQLite](https://sqlite.org/) to store content.

## Introduction
This was developed in reponse to the [10k Apart](https://blogs.windows.com/msedgedev/2016/08/15/10k-apart/) competition where each page must be usable in 10kB or less.  While not looking to win I thought it would be an interesting challenge to undertake.

Using either [Bootstrap](http://getbootstrap.com/) or [jQuery](http://jquery.com/) would quickly blow through the limit even with compression.  I looked around the interwebs for alternatives and settled on [Min](http://mincss.com/) and [ki.js](https://github.com/dciccale/ki.js) as lightweight replacements.

[Markdown](https://daringfireball.net/projects/markdown/syntax) is used to edit page content with the [Markdig](https://github.com/lunet-io/markdig) processor.  This makes it easy to create attractive looking content with relatively little effort.  A preview option is available to quickly review output.

## Installation

`dotnet ef database update --context IdentityContext`

`dotnet ef database update --context ContentContext`

## Operation

This website requires that a user first register and then login to modify content.  Once logged in and if on an editable page, you click on the "Edit Page" link in the footer to make changes to the markup.

To review existing pages, you can click on the "Page List" link in the navigation bar.  This will show you all user defined pages with the exception of the Home page which cannot be changed or removed.
