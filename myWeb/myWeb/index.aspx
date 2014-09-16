<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="myWeb.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<meta http-equiv="Content-Type" content="text/html;charset=UTF-8" />
	<title>Dizi - Anasayfa</title>
	<link rel="stylesheet" href="Content/css/reset.css"/>
	<link rel="stylesheet" href="Content/css/style.css"/>
	<link href='http://fonts.googleapis.com/css?family=Tahoma' rel='stylesheet' type='text/css'/>
    <script src='Scripts/jwplayer.js' type='text/javascript' /'></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="nav">
		<div class="genel">
			<div class="navMenu">
				<ul>
					<li><a href="#" class="a">Anasayfa</a></li>
					<li><a href="#" class="b">Bize Ulasın</a></li>
					<li><a href="#" class="r">Reklamları Kapat</a></li>
				</ul>
			</div>
			<div class="navStatu">
				<a href="#" class="d">Dizi Sitemizde <strong>48</strong> Kategoride <strong>5236</strong> Dizi Bulunmaktadır.</a>
			</div>
		</div>
	</div>
	<div class="header">
		<div class="genel">
			<h1 class="logo">FLAT<strong>DİZİ</strong></h1>
			<div class="login">
				<ul>
					<li><a href="#" class="opt">&nbsp;</a></li>
					<li><a href="#" class="fb">Facebook Giris</a></li>
					<li><a href="#" class="ug">Üye Girisi</a></li>
					<li><a href="#" class="ko">Kayıt Ol</a></li>
				</ul>
			</div>
			<div class="clear"></div>
			<div class="menu">
				<ul>
					<li class="i"><a href="#" class="i1">&nbsp;</a></li>
					<li><a href="#" class="d">Diziler</a></li>
					<li><a href="#" class="d1">Dizi Türleri</a></li>
					<li><a href="#" class="d2">Dizi Tavsiye</a></li>
					<li><a href="#" class="d3">Dizi Takvimi</a></li>
				</ul>
			</div>
			<div class="arama">
				<form action="#" method="POST">
					<input type="text" name="kelime" placeholder="Arama yap..."/>
					<button type="submit" name="gonder"></button>
				</form>
			</div>
			<div class="clear"></div>
			<div class="liste">
				<ul>
					<li><a href="#">a</a></li>
					<li><a href="#">b</a></li>
					<li><a href="#">c</a></li>
					<li><a href="#">d</a></li>
					<li><a href="#">e</a></li>
					<li><a href="#">f</a></li>
					<li><a href="#">g</a></li>
					<li><a href="#">h</a></li>
					<li><a href="#">i</a></li>
					<li><a href="#">j</a></li>
					<li><a href="#">k</a></li>
					<li><a href="#">l</a></li>
					<li><a href="#">m</a></li>
					<li><a href="#">n</a></li>
					<li><a href="#">o</a></li>
					<li><a href="#">p</a></li>
					<li><a href="#">q</a></li>
					<li><a href="#">r</a></li>
					<li><a href="#">s</a></li>
					<li><a href="#">t</a></li>
					<li><a href="#">u</a></li>
					<li><a href="#">v</a></li>
					<li><a href="#">w</a></li>
					<li><a href="#">y</a></li>
					<li><a href="#">z</a></li>
				</ul>
			</div>
		</div>
	</div>
	<div class="clear"></div>
	<div class="content">
		<div class="genel">
			<div class="title">
				<h3>Popüler Dizilerden Son Bölümler</h3>
			</div>
			<div class="diziList">
				<ul>
					<li>
						<script type='text/javascript'>
						    jwplayer('mediaspace').setup({
						        'flashplayer': 'Scripts/player.swf',
						        'file': 'http://content.longtailvideo.com/videos/flvplayer.flv',
						        'controlbar': 'bottom',
						        'width': '470',
						        'height': '320'
						    });
                    </script>
					</li>
					<li>
						<a href="#">
							<p>HD</p><p class="tr"></p>
							<span>23 Mayıs</span>
							<img src="/Content/img/diziResim.png" alt="Resim1" width="222" height="140"/>
							<strong>Ironman <br/> 2.Sezon 1.Bölüm</strong>
						</a>
					</li>
					<li>
						<a href="#">
							<p>HD</p><p class="tr"></p>
							<span>23 Mayıs</span>
							<img src="/Content/img/diziResim.png" alt="Resim1" width="222" height="140"/>
							<strong>Ironman <br/> 2.Sezon 1.Bölüm</strong>
						</a>
					</li>
					<li>
						<a href="#">
							<p>HD</p><p class="tr"></p>
							<span>23 Mayıs</span>
							<img src="/Content/img/diziResim.png" alt="Resim1" width="222" height="140"/>
							<strong>Ironman <br/> 2.Sezon 1.Bölüm</strong>
						</a>
					</li>
					<li>
						<a href="#">
							<p>HD</p><p class="tr"></p>
							<span>23 Mayıs</span>
							<img src="/Content/img/diziResim.png" alt="Resim1" width="222" height="140"/>
							<strong>Ironman <br/> 2.Sezon 1.Bölüm</strong>
						</a>
					</li>
					<li>
						<a href="#">
							<p>HD</p><p class="tr"></p>
							<span>23 Mayıs</span>
							<img src="/Content/img/diziResim.png" alt="Resim1" width="222" height="140"/>
							<strong>Ironman <br/> 2.Sezon 1.Bölüm</strong>
						</a>
					</li>
					<li>
						<a href="#">
							<p>HD</p><p class="tr"></p>
							<span>23 Mayıs</span>
							<img src="/Content/img/diziResim.png" alt="Resim1" width="222" height="140"/>
							<strong>Ironman <br/> 2.Sezon 1.Bölüm</strong>
						</a>
					</li>
					<li>
						<a href="#">
							<p>HD</p><p class="tr"></p>
							<span>23 Mayıs</span>
							<img src="/Content/img/diziResim.png" alt="Resim1" width="222" height="140"/>
							<strong>Ironman <br/> 2.Sezon 1.Bölüm</strong>
						</a>
					</li>
				</ul>
			</div>
			<div class="clear"></div>
			<div class="premium">
				<span class="close"></span>
				<span>Sync your content from Youtube, Vimeo, DailyMotion, SoundCloud and more.<br />Lorem ipsum dolor sit ametconscturom the island-studded seas...</span>
				<a href="#">Premium Üyelik</a>
			</div>
			<div class="title">
				<h3>En Son Eklenen Bölümler</h3>
			</div>
			<div class="clear"></div>
			<div class="diziList2">
				<ul>
					<li>
						<a href="#">
							<p>HD</p><p class="tr"></p>
							<span>23 Mayıs</span>
							<img src="/Content/img/diziResim.png" alt="Resim1" width="196" height="125"/>
							<strong>Ironman <br/> 2.Sezon 1.Bölüm</strong>
						</a>
					</li>
					<li>
						<a href="#">
							<p>HD</p><p class="tr"></p>
							<span>23 Mayıs</span>
							<img src="/Content/img/diziResim.png" alt="Resim1" width="196" height="125"/>
							<strong>Ironman <br/> 2.Sezon 1.Bölüm</strong>
						</a>
					</li>
					<li>
						<a href="#">
							<p>HD</p><p class="tr"></p>
							<span>23 Mayıs</span>
							<img src="/Content/img/diziResim.png" alt="Resim1" width="196" height="125"/>
							<strong>Ironman <br/> 2.Sezon 1.Bölüm</strong>
						</a>
					</li>
					<li>
						<a href="#">
							<p>HD</p><p class="tr"></p>
							<span>23 Mayıs</span>
							<img src="/Content/img/diziResim.png" alt="Resim1" width="196" height="125"/>
							<strong>Ironman <br/> 2.Sezon 1.Bölüm</strong>
						</a>
					</li>
					<li>
						<a href="#">
							<p>HD</p><p class="tr"></p>
							<span>23 Mayıs</span>
							<img src="/Content/img/diziResim.png" alt="Resim1" width="196" height="125"/>
							<strong>Ironman <br/> 2.Sezon 1.Bölüm</strong>
						</a>
					</li>
					<li>
						<a href="#">
							<p>HD</p><p class="tr"></p>
							<span>23 Mayıs</span>
							<img src="/Content/img/diziResim.png" alt="Resim1" width="196" height="125"/>
							<strong>Ironman <br/> 2.Sezon 1.Bölüm</strong>
						</a>
					</li>
					<li>
						<a href="#">
							<p>HD</p><p class="tr"></p>
							<span>23 Mayıs</span>
							<img src="/Content/img/diziResim.png" alt="Resim1" width="196" height="125"/>
							<strong>Ironman <br/> 2.Sezon 1.Bölüm</strong>
						</a>
					</li>
					<li>
						<a href="#">
							<p>HD</p><p class="tr"></p>
							<span>23 Mayıs</span>
							<img src="/Content/img/diziResim.png" alt="Resim1" width="196" height="125"/>
							<strong>Ironman <br/> 2.Sezon 1.Bölüm</strong>
						</a>
					</li>
					<li>
						<a href="#">
							<p>HD</p><p class="tr"></p>
							<span>23 Mayıs</span>
							<img src="/Content/img/diziResim.png" alt="Resim1" width="196" height="125"/>
							<strong>Ironman <br/> 2.Sezon 1.Bölüm</strong>
						</a>
					</li>
					<li>
						<a href="#">
							<p>HD</p><p class="tr"></p>
							<span>23 Mayıs</span>
							<img src="/Content/img/diziResim.png" alt="Resim1" width="196" height="125"/>
							<strong>Ironman <br/> 2.Sezon 1.Bölüm</strong>
						</a>
					</li>
					<li>
						<a href="#">
							<p>HD</p><p class="tr"></p>
							<span>23 Mayıs</span>
							<img src="/Content/img/diziResim.png" alt="Resim1" width="196" height="125"/>
							<strong>Ironman <br/> 2.Sezon 1.Bölüm</strong>
						</a>
					</li>
					<li>
						<a href="#">
							<p>HD</p><p class="tr"></p>
							<span>23 Mayıs</span>
							<img src="/Content/img/diziResim.png" alt="Resim1" width="196" height="125"/>
							<strong>Ironman <br/> 2.Sezon 1.Bölüm</strong>
						</a>
					</li>
					<li>
						<a href="#">
							<p>HD</p><p class="tr"></p>
							<span>23 Mayıs</span>
							<img src="/Content/img/diziResim.png" alt="Resim1" width="196" height="125"/>
							<strong>Ironman <br/> 2.Sezon 1.Bölüm</strong>
						</a>
					</li>
					<li>
						<a href="#">
							<p>HD</p><p class="tr"></p>
							<span>23 Mayıs</span>
							<img src="/Content/img/diziResim.png" alt="Resim1" width="196" height="125"/>
							<strong>Ironman <br/> 2.Sezon 1.Bölüm</strong>
						</a>
					</li>
					<li>
						<a href="#">
							<p>HD</p><p class="tr"></p>
							<span>23 Mayıs</span>
							<img src="/Content/img/diziResim.png" alt="Resim1" width="196" height="125"/>
							<strong>Ironman <br/> 2.Sezon 1.Bölüm</strong>
						</a>
					</li>
					<li>
						<a href="#">
							<p>HD</p><p class="tr"></p>
							<span>23 Mayıs</span>
							<img src="/Content/img/diziResim.png" alt="Resim1" width="196" height="125"/>
							<strong>Ironman <br/> 2.Sezon 1.Bölüm</strong>
						</a>
					</li>
					<li>
						<a href="#">
							<p>HD</p><p class="tr"></p>
							<span>23 Mayıs</span>
							<img src="/Content/img/diziResim.png" alt="Resim1" width="196" height="125"/>
							<strong>Ironman <br/> 2.Sezon 1.Bölüm</strong>
						</a>
					</li>
					<li>
						<a href="#">
							<p>HD</p><p class="tr"></p>
							<span>23 Mayıs</span>
							<img src="/Content/img/diziResim.png" alt="Resim1" width="196" height="125"/>
							<strong>Ironman <br/> 2.Sezon 1.Bölüm</strong>
						</a>
					</li>
					<li>
						<a href="#">
							<p>HD</p><p class="tr"></p>
							<span>23 Mayıs</span>
							<img src="/Content/img/diziResim.png" alt="Resim1" width="196" height="125"/>
							<strong>Ironman <br/> 2.Sezon 1.Bölüm</strong>
						</a>
					</li>
					<li>
						<a href="#">
							<p>HD</p><p class="tr"></p>
							<span>23 Mayıs</span>
							<img src="/Content/img/diziResim.png" alt="Resim1" width="196" height="125"/>
							<strong>Ironman <br/> 2.Sezon 1.Bölüm</strong>
						</a>
					</li>
					<li>
						<a href="#">
							<p>HD</p><p class="tr"></p>
							<span>23 Mayıs</span>
							<img src="/Content/img/diziResim.png" alt="Resim1" width="196" height="125"/>
							<strong>Ironman <br/> 2.Sezon 1.Bölüm</strong>
						</a>
					</li>
				</ul>
			</div>
			<div class="title2">
				<h3>Popüler Diziler</h3>
			</div>
			<div class="topList">
				<ul>
					<li><a href="#">The Originals</a></li>
					<li><a href="#">Intelligence</a></li>
					<li><a href="#">Cougar Town</a></li>
					<li><a href="#">House of Cards</a></li>
					<li><a href="#">Teen Wolf</a></li>
					<li><a href="#">Justified</a></li>
					<li><a href="#">Being Human</a></li>
					<li><a href="#">2 Broke Girls</a></li>
					<li><a href="#">Believe</a></li>
					<li><a href="#">Da Vinci’s Demons</a></li>
				</ul>
			</div>
			<div class="reklamAlani"></div>
			<div class="title2">
				<h3>Eski Diziler</h3>
			</div>
			<div class="topList2">
				<ul>
					<li><a href="#">Teen Wolf</a></li>
					<li><a href="#">The Big Bang Theory </a></li>
					<li><a href="#">The Following </a></li>
					<li><a href="#">Mentalist </a></li>
					<li><a href="#">Band Of Brothers</a></li>
					<li><a href="#">Diaries </a></li>
					<li><a href="#">Walking Dead </a></li>
					<li><a href="#">Vikings</a></li>
					<li><a href="#">Almost Human American </a></li>
					<li><a href="#">Dad Horror Story</a></li>
				</ul>
			</div>
		</div>
	</div>
	<div class="clear"></div>
	<div class="footer">
		<div class="genel">
			<span class="copy">&copy; 2014 Copyright Flatdizi.com Tüm Hakları Saklıdır.</span>
			<ul class="social">
				<li><a href="#" class="f" title="Facebook"></a></li>
				<li><a href="#" class="t" title="Twitter"></a></li>
				<li><a href="#" class="g" title="Google+"></a></li>
				<li><a href="#" class="apple" title="Apple"></a></li>
				<li><a href="#" class="android" title="Android"></a></li>
				<li><a href="#" class="rss" title="RSS"></a></li>
			</ul>
		</div>
	</div>
    </form>
</body>
</html>
