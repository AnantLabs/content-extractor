using System;
using System.IO;
using System.Reflection;
using System.Xml;
using NUnit.Framework;
using ContentExtractor.Core;

namespace ContentExtractorTests
{
	/// <summary>
	/// Tests for WebDocument class
	/// </summary>
	[TestFixture]
	public class WebDocumentTests
	{
	  [Test]
	  public void ParseSimpleCode()
	  {
      XmlDocument doc = WebDocument.HtmlParse("<html><body><p>code</p></body></html>");
      TestUtils.AssertXmlAreEqual(
        "<HTML><HEAD></HEAD><BODY><P>code</P></BODY></HTML>", 
        doc);
	  }
	  
	  [Test]
	  public void ParseDocWithTwoRoots()
	  {
      XmlDocument doc = WebDocument.HtmlParse(
	      @"<script language=""JavaScript""> var a = 0;</script><html><body>text</body></html>");
      TestUtils.AssertXmlAreEqual(
        @"<HTML><HEAD><SCRIPT language='JavaScript'>var a = 0;</SCRIPT></HEAD><BODY>text</BODY></HTML>",
        doc);
	  }
	  
	  [Test]
    public void EmptyDocument()
    {
      XmlDocument doc = WebDocument.HtmlParse(string.Empty);
      Assert.IsNotNull(doc.DocumentElement, "doc.Document is null");
      Assert.AreEqual("HTML", doc.DocumentElement.Name);
    }
	  
    [Test]
    public void XmlHeaderAndEncodingHtml()
    {
      XmlDocument doc = WebDocument.HtmlParse(
        @"<?xml version=""1.0"" encoding=""Windows-1251""?><html></html>");
      Assert.IsNotNull(doc, "doc is null.");
      Assert.IsNotNull(doc.DocumentElement, 
                       "doc.DocumentElement wasn't initialized");
      Assert.AreEqual("HTML", doc.DocumentElement.Name);
    }
    [Test]
    public void NotClosedDocument()
    {
      XmlDocument doc = WebDocument.HtmlParse("<html><body><div class='asd");
      Assert.IsNotNull(doc, "doc is null.");
      Assert.IsNotNull(doc.DocumentElement, 
                       "doc.DocumentElement wasn't initialized");
      Assert.AreEqual("HTML", doc.DocumentElement.Name);
    }
    
    [Test]
    // ToDo: Need to simplify
    public void TestFromInternet1()
    {
      string content = @"
<html>
<head>
  <meta http-equiv=""Content-Type"" content=""text/html; charset=windows-1251"">
  <link rel=""stylesheet"" href='/cifrovik/themes/default/css/default.css' type='text/css'>
  <link rel='shortcut icon' href='/favicon.ico' type='image/x-icon'>
  <title>CECT IP1000 - 'цветы' распускаются зимой</title>
  <meta name='description' content='CECT IP1000 - &quot;цветы&quot; распускаются зимой'>
  <meta name='keywords' content='CECT IP1000 - &quot;цветы&quot; распускаются зимой'>
  <script src='http://www.google-analytics.com/urchin.js' type='text/javascript'></script>
  <script type='text/javascript'>    _uacct = 'UA-149746-2';
    urchinTracker();
  </script>
</head>

<body bgcolor='#ffffff' text='#505050' style='margin: 0px; padding: 0px;'>
<пред&nbsp;&bull;&nbsp;след></body>
</html>";
      XmlDocument doc = WebDocument.HtmlParse(content);
      Assert.IsNotNull(doc, "doc is null.");
      Assert.IsNotNull(doc.DocumentElement, 
                       "doc.DocumentElement wasn't initialized");
      Assert.AreEqual("HTML", doc.DocumentElement.Name);
    }

    [Test]
    public void NotClosedTags()
    {
      string content = @"
<html><head><title>Энциклопедия кино - награды и номинации премий и кинофестивалей: Ника - 2004</title><meta http-equiv='Content-Type' content='text/html; charset=windows-1251'><meta http-equiv='Content-Language' content='ru'><META name='Description' content='Энциклопедия кино: награды и номинации премий и кинофестивалей, Ника - 2004'><META name='Keywords' content='энциклопедия, кино, фильм, награда, номинация, премия, кинофестиваль, фестиваль, Ника - 2004'><LINK href='/styles/main.css' type='text/css' rel='stylesheet'><link rel='shortcut icon' href='favicon.ico'>
</head><body><table class='m'><tr><td width='10' height='10' class='m'><font size='1'><!--Rating@Mail.ru COUNTER--><script language='JavaScript' type='text/javascript'><!--
d=document;var a='';a+=';r='+escape(d.referrer)
js=10//--></script><script language='JavaScript1.1' type='text/javascript'><!--
a+=';j='+navigator.javaEnabled()
js=11//--></script><script language='JavaScript1.2' type='text/javascript'><!--
s=screen;a+=';s='+s.width+'*'+s.height
a+=';d='+(s.colorDepth?s.colorDepth:s.pixelDepth)
js=12//--></script><script language='JavaScript1.3' type='text/javascript'><!--
js=13//--></script><script language='JavaScript' type='text/javascript'><!--
d.write('<img src='http://top.list.ru/counter'+
'?id=531278;js='+js+a+';rand='+Math.random()+
'' height=1 width=1/>')
if(11<js)d.write('<'+'!-- ')//--></script><noscript><img src='http://top.list.ru/counter?js=na;id=531278' height=1 width=1 alt=''/></noscript><script language='JavaScript' type='text/javascript'><!-- if(11<js)d.write('--'+'>')//--></script><!--/COUNTER--><!--begin of Rambler's Top100 code --><a href='http://top100.rambler.ru/top100/'><img src='http://counter.rambler.ru/top100.cnt?703756' alt='' width=1 height=1 border=0></a><!--end of Top100 code-->
<td width='10' class='m'><td class='m'><td width='10' class='m'><tr height='10'><td class='m'><td class='ia'><td class='iv'><td class='iv'><tr height='10'><td class='m'><td class='ih'><td><td><tr><td class='m'><td class='ih'><td><table><td width='10'><td class='bm' width='250'><h1>Награды и номинации<td>&nbsp;<td class='b' width='480'><a href='http://www.spbvideo.ru/index.php?p=1'><img src='/images/s.gif' class='l' alt='Кино в Санкт-Петербурге: обмен, продажа, прокат'></a><a href='http://www.spbvideo.ru/index.php?p=2'><img src='/images/p.gif' class='l' alt='Энциклопедия кино'></a><a href='http://www.spbvideo.ru/index.php?p=2'><img src='/images/b.gif' class='l' alt='Фильмы, кинофильмы, видеофильмы'></a><a href='http://www.spbvideo.ru/index.php?p=2'><img src='/images/v.gif' class='l' alt='Описания, рецензии'></a><a href='http://www.spbvideo.ru/index.php?p=2'><img src='/images/i.gif' class='l' alt='Режиссёры, актёры, актрисы'></a><a href='http://www.spbvideo.ru/index.php?p=2'><img src='/images/d.gif' class='l' alt='Фотографии, постеры, кадры из фильмов'></a><a href='http://www.spbvideo.ru/index.php?p=5'><img src='/images/e.gif' class='l' alt='Афиша, кинотеатры, киноафиша'></a><a href='http://www.spbvideo.ru/index.php?p=5'><img src='/images/o.gif' class='l' alt='Скачать фильмы бесплатно'></a></table><td><tr height='10'><td class='m'><td background='/images/grad_h.jpg'><td><td><tr height='10'><td class='m'><td background='/images/grad_h.jpg'><td><table><tr><td width='10'><td width='120'><table><tr><td class='bP'><a class='menu' href='http://www.spbvideo.ru/index.php' title='Кино в Санкт-Петербурге'>Главная</a><tr><td class='bP'><a class='menu' href='http://www.spbvideo.ru/index.php?p=1' title='Обмен, продажа, прокат'>О проекте</a><tr><td class='bPA'><b><a class='menuA' href='http://www.spbvideo.ru/index.php?p=2' title='Энциклопедия кино'>Энциклопедия</a><table class='submenu'><tr><td class='bpa'><ul><li><a ' href='http://www.spbvideo.ru/index.php?p=2&pt=1' title='Энциклопедия кино - Фильмы'>Фильмы</a></li><li><a ' href='http://www.spbvideo.ru/index.php?p=2&pt=2' title='Фото, постеры, кадры из фильмов'>Фото</a></li><li><a ' href='http://www.spbvideo.ru/index.php?p=2&pt=3' title='Награды и номинации'>Награды</a></li></ul></table><tr><td class='bP'><a class='menu' href='http://www.spbvideo.ru/index.php?p=3' title='Мультфильмы'>Мультфильмы</a><tr><td class='bP'><a class='menu' href='http://www.spbvideo.ru/index.php?p=4' title='Файлы offline-версии Энциклопедии кино'>Файлы</a><tr><td class='bP'><a class='menu' href='http://www.spbvideo.ru/index.php?p=5' title='Полезные ссылки (фильмы, мультики)'>Ссылки</a><tr><td class='bP'><a class='menu' href='http://www.spbvideo.ru/index.php?p=6' title='Гостевая книга'>Гостевая книга</a><tr><td class='bP'><a class='menu' href='http://www.spbvideo.ru/index.php?p=7' title='Рассылка Энциклопедии кино'>Рассылка</a><tr><td height='10'>
<tr><td><table class='stat'><tr><td class='b'><a href='http://www.spbvideo.ru/index.php?p=2'><h3>Случайный фильм</a><tr><td class='ri'>
<a href='http://www.spbvideo.ru/index.php?p=2&t=f&i=2130' alt='Нападение на участок 13 (Нападение на тринадцатый участок) // Assault on Precinct 13'><img alt='Нападение на участок 13 (Нападение на тринадцатый участок) // Assault on Precinct 13' src='/bi/2442s.jpg'></a><tr><td height='10'>
</table>
<tr><td align='center'>
<!--Rating@Mail.ru LOGO--><a target=_top
href='http://top.mail.ru/jump?from=531278'><img
src='http://top.list.ru/counter?id=531278;t=231;l=1'
border=0 height=31 width=88
alt='Рейтинг@Mail.ru'/></a><!--/LOGO-->
<!--begin of Top100 logo-->
<a href='http://top100.rambler.ru/top100/'>
<img src='http://top100-images.rambler.ru/top100/banner-88x31-rambler-darkblue2.gif' alt='Rambler's Top100' width=88 height=31 border=0></a>
<!--end of Top100 logo -->

</table>
<td width='10'><td><index><table>
<tr><td class='bPA'><h2 align='center'>Ника - 2004</h2><p><p><h3 class='l'>Лучшая женская роль</h3><ul><li><a href='http://www.spbvideo.ru/index.php?p=2&t=f&i=2930'><img src='images/star.gif' width='14' height='14' alt='победитель'/>Старухи</a>: Березуцкая Валентина<li><a href='http://www.spbvideo.ru/index.php?p=2&t=f&i=2789'>Благословите женщину</a>: Ходченкова Светлана<li><a href='http://www.spbvideo.ru/index.php?p=2&t=f&i=2931'>Магнитные бури</a>: Толстоганова Виктория</ul><p><h3 class='l'>Лучшая женская роль второго плана</h3><ul><li><a href='http://www.spbvideo.ru/index.php?p=2&t=f&i=2789'><img src='images/star.gif' width='14' height='14' alt='победитель'/>Благословите женщину</a>: Чурикова Инна<li><a href='http://www.spbvideo.ru/index.php?p=2&t=f&i=2835'>Бедный, бедный Павел</a>: Мысина Оксана<li><a href='http://www.spbvideo.ru/index.php?p=2&t=f&i=1341'>Шик</a>: Дапкунайте Ингеборга</ul><p><h3 class='l'>Лучшая мужская роль</h3><ul><li><a href='http://www.spbvideo.ru/index.php?p=2&t=f&i=2835'><img src='images/star.gif' width='14' height='14' alt='победитель'/>Бедный, бедный Павел</a>: Сухоруков Виктор<li><a href='http://www.spbvideo.ru/index.php?p=2&t=f&i=2835'>Бедный, бедный Павел</a>: Янковский Олег</ul><p><h3 class='l'>Лучшая мужская роль второго плана</h3><ul><li><a href='http://www.spbvideo.ru/index.php?p=2&t=f&i=2795'><img src='images/star.gif' width='14' height='14' alt='победитель'/>Ключ от спальни</a>: Маковецкий Сергей<li><a href='http://www.spbvideo.ru/index.php?p=2&t=f&i=1658'>Прогулка</a>: Гришковец Евгений<li><a href='http://www.spbvideo.ru/index.php?p=2&t=f&i=1341'>Шик</a>: Панин Андрей</ul><p><h3 class='l'>Лучшая музыка к фильму</h3><ul><li><a href='http://www.spbvideo.ru/index.php?p=2&t=f&i=1287'><img src='images/star.gif' width='14' height='14' alt='победитель'/>Бумер</a>: Шнуров Сергей<li><a href='http://www.spbvideo.ru/index.php?p=2&t=f&i=2931'>Магнитные бури</a>: Лебедев Виктор<li><a href='http://www.spbvideo.ru/index.php?p=2&t=f&i=1341'>Шик</a>: Назаров Далер</ul><p><h3 class='l'>Лучшая операторская работа</h3><ul><li><a href='http://www.spbvideo.ru/index.php?p=2&t=f&i=1863'>Возвращение</a>: Кричман Михаил<li><a href='http://www.spbvideo.ru/index.php?p=2&t=f&i=2788'>Коктебель</a>: Беркеши Шандор<li><a href='http://www.spbvideo.ru/index.php?p=2&t=f&i=2931'>Магнитные бури</a>: Шайгарданов Юрий</ul><p><h3 class='l'>Лучшая работа звукорежиссёра</h3><ul><li><a href='http://www.spbvideo.ru/index.php?p=2&t=f&i=2931'><img src='images/star.gif' width='14' height='14' alt='победитель'/>Магнитные бури</a>: Базанов Евгений; Поздняков Евгений<li><a href='http://www.spbvideo.ru/index.php?p=2&t=f&i=2932'>Русский ковчег</a>: Мошков Сергей; Персов Владимир<li><a href='http://www.spbvideo.ru/index.php?p=2&t=f&i=2930'>Старухи</a>: Худяков Андрей</ul><p><h3 class='l'>Лучшая работа художника</h3><ul><li><a href='http://www.spbvideo.ru/index.php?p=2&t=f&i=2932'><img src='images/star.gif' width='14' height='14' alt='победитель'/>Русский ковчег</a>: Жукова Елена; Кочергина Наталья<li><a href='http://www.spbvideo.ru/index.php?p=2&t=f&i=2835'>Бедный, бедный Павел</a>: Загоскин Александр<li><a href='http://www.spbvideo.ru/index.php?p=2&t=f&i=2795'>Ключ от спальни</a>: Борисов Александр; Свинцицкий Леонид</ul><p><h3 class='l'>Лучшая работа художника по костюмам</h3><ul><li><a href='http://www.spbvideo.ru/index.php?p=2&t=f&i=2835'>Бедный, бедный Павел</a>: Конникова Лариса<li><a href='http://www.spbvideo.ru/index.php?p=2&t=f&i=2795'>Ключ от спальни</a>: Иванова Наталия<li><a href='http://www.spbvideo.ru/index.php?p=2&t=f&i=2932'>Русский ковчег</a>: Гришанова Мария; Крюкова Лидия; Сеферян Тамара</ul><p><h3 class='l'>Лучшая режиссёрская работа</h3><ul><li><a href='http://www.spbvideo.ru/index.php?p=2&t=f&i=2931'><img src='images/star.gif' width='14' height='14' alt='победитель'/>Магнитные бури</a>: Абдрашитов Вадим<li><a href='http://www.spbvideo.ru/index.php?p=2&t=f&i=1287'>Бумер</a>: Буслов Петр<li><a href='http://www.spbvideo.ru/index.php?p=2&t=f&i=1863'>Возвращение</a>: Звягинцев Андрей<li><a href='http://www.spbvideo.ru/index.php?p=2&t=f&i=2930'>Старухи</a>: Сидоров Геннадий</ul><p><h3 class='l'>Лучшая сценарная работа</h3><ul><li><a href='http://www.spbvideo.ru/index.php?p=2&t=f&i=2931'><img src='images/star.gif' width='14' height='14' alt='победитель'/>Магнитные бури</a>: Миндадзе Александр<li><a href='http://www.spbvideo.ru/index.php?p=2&t=f&i=1658'>Прогулка</a>: Смирнова Дуня<li><a href='http://www.spbvideo.ru/index.php?p=2&t=f&i=2930'>Старухи</a>: Сидоров Геннадий</ul><p><h3 class='l'>Лучший анимационный фильм</h3><ul><li><a href='http://www.spbvideo.ru/index.php?p=2&t=f&i=732'>Карлик Нос</a></ul><p><h3 class='l'>Лучший игровой фильм</h3><ul><li><a href='http://www.spbvideo.ru/index.php?p=2&t=f&i=1863'><img src='images/star.gif' width='14' height='14' alt='победитель'/>Возвращение</a><li><a href='http://www.spbvideo.ru/index.php?p=2&t=f&i=2789'>Благословите женщину</a><li><a href='http://www.spbvideo.ru/index.php?p=2&t=f&i=2788'>Коктебель</a><li><a href='http://www.spbvideo.ru/index.php?p=2&t=f&i=2931'>Магнитные бури</a><li><a href='http://www.spbvideo.ru/index.php?p=2&t=f&i=2932'>Русский ковчег</a><li><a href='http://www.spbvideo.ru/index.php?p=2&t=f&i=2930'>Старухи</a></ul><p><h3 class='l'>Открытие года</h3><ul><li><a href='http://www.spbvideo.ru/index.php?p=2&t=f&i=2933'><img src='images/star.gif' width='14' height='14' alt='победитель'/>Последний поезд</a>: Герман Алексей (младший) (<i>режиссёр</i>)<li><a href='http://www.spbvideo.ru/index.php?p=2&t=f&i=1863'>Возвращение</a>: Звягинцев Андрей (<i>режиссёр</i>)<li><a href='http://www.spbvideo.ru/index.php?p=2&t=f&i=2930'>Старухи</a>: Сидоров Геннадий (<i>режиссёр</i>)</ul></table>
</index></table><td><tr height='10'><td class='m'><td class='ih'><td><td><tr height='10'><td class='m'><td class='ih'><td align='right'><table><tr><td width='120'>&nbsp;<td width='10'>&nbsp;<td><SCRIPT type=text/javascript>
 var begun_auto_colors = new Array();
 var begun_auto_fonts_size = new Array();
 begun_auto_pad = 45892751; // идентификатор площадки (Эту цифру вы получаете от Бегуна)
 begun_auto_limit = 5; // число объявлений выводимых на площадке
 begun_auto_width=800; // ширина блока объявлений.
 begun_auto_colors[0]='#0000CC'; // цвет ссылки объявлений
 begun_auto_colors[1]='#000000'; // цвет текста объявления
 begun_auto_colors[2]='#00005B'; // цвет домена объявления
 begun_auto_colors[3]='#FFFFFF'; // цвет фона блока объявлений
 begun_auto_fonts_size[0]='10pt'; // р-мер шрифта ссылки объявлений
 begun_auto_fonts_size[1]='10pt'; // р-мер шрифта текста объявления
 begun_auto_fonts_size[2]='10pt'; // р-мер шрифта домена объявления
 begun_auto_fonts_size[3]='10pt'; // р-мер шрифта заглушки
 begun_target = 'blank';
 </SCRIPT>
 <SCRIPT src='http://autocontext.begun.ru/autocontext.js' type='text/javascript'>
 </SCRIPT></table><td><tr height='10'><td class='m'><td class='ih'><td><td></table></body></html>";

      XmlDocument doc = WebDocument.HtmlParse(content);
      Assert.IsNotNull(doc, "doc is null.");
      Assert.IsNotNull(doc.DocumentElement, 
                       "doc.DocumentElement wasn't initialized");
      Assert.AreEqual("HTML", doc.DocumentElement.Name);
      Assert.IsNotNull(
        doc.SelectSingleNode("/HTML[1]/BODY[1]/TABLE[1]/TBODY[1]/TR[6]"),
        "Parsing was mistaken");
      Assert.IsNotNull(
        doc.SelectSingleNode(
          "/HTML[1]/BODY[1]/TABLE[1]/TBODY[1]/TR[6]/TD[3]/TABLE[1]/TBODY[1]/TR[1]/TD[4]/INDEX[1]/TABLE[1]/TBODY[1]/TR[1]/TD[1]/UL[1]/LI[1]"),
        "Parsing was mistaken");
    }
	}
}
