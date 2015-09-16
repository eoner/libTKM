# libTKM

İstanbul Büyükşehir Belediyesi [Trafik Kontrol Merkezi (TKM)](http://tkm.ib.gov.tr)  trafik yoğunluğu uygulamasından trafik verisini çekmek için bir yazılım kütüphanesi ve demo uygulaması.

Kütüphanenin kullanımı hakkında bilgi edinmek için TKMDemoApp uygulamasını inceleyebilirsiniz. 

TKMDemoApp uygulaması, TKM websitesinden çektiği veriler ile:
- Anlık trafik yoğunluğu haritası oluşturur.
- Trafik yoğunluğu ortalama değerlerini günceller ve grafik için gerekli çıktıları üretir. 

Verileri görüntülemek için, libTKM/TKMDemoApp/demo dizini içinde
```
python -m SimpleHTTPServer 8080
```
ile bir web sunucusu başlatıp <code>http://localhost:8080</code> adresine bağlanabilirsiniz.

<strong>Online demo: [http://www.onikimaymun.org/projects/tkm/](http://www.onikimaymun.org/projects/tkm/)</strong>

