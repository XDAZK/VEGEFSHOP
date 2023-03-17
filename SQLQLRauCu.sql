create database QLBanRauCu
GO

use QLBanRauCu
GO


Create Table ADMIN
(
	UserAdmin VARCHAR(30) PRIMARY KEY,
	PassAdmin VARCHAR(30) NOT NULL,
	Hoten nVARCHAR(30)
)
GO
INSERT INTO Admin(UserAdmin,PassAdmin,Hoten)VALUES('sinh','123',N'Sinh')

CREATE TABLE KHACHHANG
(
	MaKH INT IDENTITY(1,1) PRIMARY KEY,
	HoTen nVarchar(50) NOT NULL,
	Taikhoan Varchar(50) UNIQUE,
	Matkhau Varchar(50) NOT NULL,
	Email Varchar(100) UNIQUE,
	DiachiKH nVarchar(200),
	DienthoaiKH Varchar(50),	
	Ngaysinh DATETIME
)
GO

Create Table LOAISANPHAM
(
	MaloaiSP int Identity(1,1) PRIMARY KEY,
	Tensanpham nvarchar(50) NOT NULL,
	hinh NVARCHAR(1000)
)
GO


CREATE TABLE NCC
(
	MaNCC INT IDENTITY(1,1) PRIMARY KEY,
	TenNCC NVARCHAR(50) NOT NULL,
	Diachi NVARCHAR(100),
	Dienthoai VARCHAR(50)
)
GO

CREATE TABLE SANPHAM
(
	MaSP INT IDENTITY(1,1) PRIMARY KEY,
	TenSP NVARCHAR(100) NOT NULL,
	Giaban Decimal(18,0) CHECK (Giaban>=0),
	Mota NVarchar(Max),
	Anhbia NVARCHAR(1000),
	Ngaycapnhat DATETIME,
	Soluongton INT,
	DVT NVARCHAR(10),
	MaloaiSP INT,
	MaNCC INT,
	Constraint FK_Loaisanpham Foreign Key(MaloaiSP) References LOAISANPHAM(MaloaiSP),
	Constraint FK_NCC Foreign KEY (MaNCC) References NCC(MaNCC)
)
GO


CREATE TABLE DONDATHANG
(
	MaDonHang INT IDENTITY(1,1) PRIMARY KEY,
	Dathanhtoan bit,
	Tinhtranggiaohang  bit,
	Ngaydat Datetime,
	Ngaygiao Datetime,	
	MaKH INT,
	TongTien Decimal(18,0) Check(TongTien>=0),
	CONSTRAINT FK_Khachhang FOREIGN KEY (MaKH) REFERENCES Khachhang(MaKH)
)
Go
 CREATE TABLE CHITIETDONHANG
(
	MaDonHang INT,
	MaSP INT,	
	Soluong Int Check(Soluong>0),
	Dongia Decimal(18,0) Check(Dongia>=0),	
	ThanhTien Decimal(18,0) Check(ThanhTien>=0),
	CONSTRAINT PK_CTDatHang PRIMARY KEY(MaDonHang,MaSP),
	CONSTRAINT FK_Donhang FOREIGN KEY (Madonhang) REFERENCES DONDATHANG(Madonhang),
	CONSTRAINT FK_MASP FOREIGN KEY (MaSP) REFERENCES SANPHAM(MaSP)
	
)

CREATE TABLE HOTRO
(
	MaHotro INT IDENTITY(1,1) PRIMARY KEY,
	MaKH INT,	
	Email Varchar(100) UNIQUE,
	HoTen nVarchar(50) NOT NULL,
	LyDo nVarchar(max),
	Constraint FK_HoTro Foreign KEY (MaKH) References KHACHHANG(MaKH)
)

INSERT INTO LOAISANPHAM(Tensanpham)VALUES(N'Rau')
INSERT INTO LOAISANPHAM(Tensanpham)VALUES(N'Củ')
INSERT INTO LOAISANPHAM(Tensanpham)VALUES(N'Quả')
INSERT INTO LOAISANPHAM(Tensanpham)VALUES(N'Trái cây')
INSERT INTO LOAISANPHAM(Tensanpham)VALUES(N'Ngũ cốc, hạt')
INSERT INTO LOAISANPHAM(Tensanpham)VALUES(N'Gia vị')
INSERT INTO LOAISANPHAM(Tensanpham)VALUES(N'Thực phẩm chế biến')
GO
INSERT INTO NCC(TenNCC,	Diachi,	Dienthoai)VALUES(N'Cầu Đất Farm',N'Thon Truong Tho, Xa Tram Hanh, Thanh pho Da Lat, Lam Dong','1250336879')
INSERT INTO NCC(TenNCC,	Diachi,	Dienthoai)VALUES(N'Organic Food',N'Số 93 Trần Não, Phường Bình An, Quận 2, Hồ Chí Minh, Việt Nam','1250336879')
INSERT INTO NCC(TenNCC,	Diachi,	Dienthoai)VALUES(N'Đà Lạt GAP',N'52 Điện Biên Phủ, Phường 6, Quận 3, TP. HCM','1250336879')
INSERT INTO NCC(TenNCC,	Diachi,	Dienthoai)VALUES(N'ORFARM',N'296 Nguyễn Đình Chiểu, Phường 6, Quận 3, TPHCM','1250336879')
GO

--------------------------------RAU
INSERT INTO SANPHAM(TenSP,Giaban,Mota,Anhbia,Ngaycapnhat,Soluongton,DVT,MaloaiSP,MaNCC)VALUES(N'CẢI THÌA HỮU CƠ',21000,N'Cải thìa là loại rau rất gần gũi với các món ăn của người Việt Nam và Trung Hoa. Rau giòn, vị ngon, ngọt. Cải thìa có chứa folate, kali và canxi giúp xương chắc khỏe. Các chất thuộc nhóm carotenoid trong cải thìa có tác dụng như chất làm chậm quá trình oxi hóa và giảm việc hình thành những nguồn gốc có hại trong cơ thể. ',
N'CẢI THÌA HỮU CƠ.png','2022-05-07 00:00:00.000',30,'thùng',1,1)
INSERT INTO SANPHAM(TenSP,Giaban,Mota,Anhbia,Ngaycapnhat,Soluongton,DVT,MaloaiSP,MaNCC)VALUES(N'CẢI NHÚN HỮU CƠ',21000,N'Cải nhún có vị dễ ăn, có tác dụng làm mát gan, thanh lọc, giải nhiệt cơ thể. Rau có chứa vitamin C và nhiều nguyên tố vi lượng giúp ích cho sự trao đổi chất của cơ thể. Ngoài ra, cải nhún còn chống ung thư do các glucosinolate có trong rau này. Bên cạnh đó, khi trẻ em mệt, chán ăn, một bát cháo với cải nhún ngon miệng dễ nuốt và dễ tiêu hóa sẽ giúp bé mau chóng khỏe lại. Ngoài việc cung cấp nhiều canxi, cải nhún còn chứa nhiều vitamin K, chất dinh dưỡng giúp xương chắc khỏe – vitamin K có thể giúp tăng mật độ chất khoáng trong xương cũng như giảm tỷ lệ gãy xương ở những người mắc bệnh loãng xương. ',
N'CẢI NHÚN HỮU CƠ.png','2022-05-07 00:00:00.000',30,'thùng',1,1)
INSERT INTO SANPHAM(TenSP,Giaban,Mota,Anhbia,Ngaycapnhat,Soluongton,DVT,MaloaiSP,MaNCC)VALUES(N'CẢI NGỌT HỮU CƠ',21000,N'Cải ngọt có hơn 10 loại vitamin cần thiết cho cơ thể, trong đó, hàm lượng canxi, vitamin A và vitamin K rất dồi dào, với một lượng đáng kể vitamin B9 và vitamin E. Chính vì thế, rau cải ngọt còn có tác dụng nâng cao sức đề kháng, bảo vệ cơ thể khỏi các tác nhân gây bệnh. ',
N'CẢI NGỌT HỮU CƠ.png','2022-05-07 00:00:00.000',30,'thùng',1,1)
INSERT INTO SANPHAM(TenSP,Giaban,Mota,Anhbia,Ngaycapnhat,Soluongton,DVT,MaloaiSP,MaNCC)VALUES(N'CẢI NGỒNG HỮU CƠ',21000,N'Cải ngồng rất tốt cho phụ nữ bởi nó chứa nhiều dinh dưỡng thiết yếu giúp ngăn ngừa hàm lượng estrogen dư thừa trong cơ thể có thể dẫn đến ung thư. Chất folate và chất xơ trong cải ngồng đều cần thiết cho sức khỏe của phụ nữ. ',
N'CẢI NGỒNG HỮU CƠ.png','2022-05-07 00:00:00.000',30,'thùng',1,1)
INSERT INTO SANPHAM(TenSP,Giaban,Mota,Anhbia,Ngaycapnhat,Soluongton,DVT,MaloaiSP,MaNCC)VALUES(N'CẢI BẸ XANH HỮU CƠ',21000,N'Cải bẹ xanh có lượng calorie thấp nhưng lại nhiều chất xơ cùng các vitamin và khoáng chất thiết yếu. Đặc biệt, chúng là nguồn cung cấp vitamin C và K dồi dào. Theo y học cổ truyền, cải bẹ xanh có vị cay, tính ôn với tác dụng thanh nhiệt, giải độc, giải cảm hàn, thông đàm, lợi khí và lợi tiểu. ',
N'CẢI BẸ XANH HỮU CƠ.png','2022-05-07 00:00:00.000',30,'thùng',1,1)

----------------------------------CỦ
INSERT INTO SANPHAM(TenSP,Giaban,Mota,Anhbia,Ngaycapnhat,Soluongton,MaloaiSP,MaNCC)VALUES(N'CỦ GỪNG',42000,N'Củ gừng có vị cay và hương thơm đặc biệt, có thể dùng để điều vị thêm hương, là thứ gia vị vô cùng hấp dẫn và không thể thiếu trong cuộc sống. ',
N'CỦ GỪNG.png','2022-05-07 00:00:00.000',30,2,1)
INSERT INTO SANPHAM(TenSP,Giaban,Mota,Anhbia,Ngaycapnhat,Soluongton,MaloaiSP,MaNCC)VALUES(N'CỦ DỀN',31500,N'Củ dền mang lại nhiều lợi ích tuyệt vời cho sức khỏe: chứa chất chống oxy hóa, giảm viêm, cải thiện sức khỏe tim mạch, hỗ trợ giải độc, tăng cường chức năng não bộ, hỗ trợ tiêu hóa, hỗ trợ giảm cân,... ',
N'CỦ DỀN.png','2022-05-07 00:00:00.000',30,2,1)
INSERT INTO SANPHAM(TenSP,Giaban,Mota,Anhbia,Ngaycapnhat,Soluongton,MaloaiSP,MaNCC)VALUES(N'CỦ CẢI TRẮNG',37000,N'Củ cải trắng không chỉ là một món ăn phổ biến, mà còn được biết tới như loại thần dược có công năng không thua kém gì nhân sâm, củ cải trắng giúp trị đờm, thanh nhiệt, dưỡng da, chống ung thư, phòng tránh thiếu máu và giúp cho cơ thể đủ nước...',
N'CỦ CẢI TRẮNG.png','2022-05-07 00:00:00.000',30,2,4)
INSERT INTO SANPHAM(TenSP,Giaban,Mota,Anhbia,Ngaycapnhat,Soluongton,MaloaiSP,MaNCC)VALUES(N'CỦ CẢI ĐỎ',63000,N'rong củ cải đỏ chứa nhiều vitamin như vitamin A, B9, C (có nhiều nhất trong lá củ cải đỏ) và các khoáng chất cần thiết khác như: sắt, axit folic, kali, magie,... Màu củ cải đỏ là do chứa chất beta cyanin. Chất này có khả năng loại bỏ các độc tố có trong gan, giúp giải độc gan và ngăn chặn sự thành của các lớp mô mỡ trong cơ thể. Đồng thời, beta cyanin có trong củ cải đỏ còn rất tốt đối với bệnh tim mạch. ',
N'CỦ CẢI ĐỎ.png','2022-05-07 00:00:00.000',30,2,4)
INSERT INTO SANPHAM(TenSP,Giaban,Mota,Anhbia,Ngaycapnhat,Soluongton,MaloaiSP,MaNCC)VALUES(N'CÀ RỐT',34000,N'Ăn cà rốt chứa vitamin A, vitamin C, carotenoid, kali và các chất chống oxy hoá, không chỉ có tác dụng tốt cho mắt, mà còn mang đến cho bạn làn da khỏe mạnh, hồng hào, tốt cho xương, làm chậm quá trình lão hóa và thậm chí ngăn ngừa bệnh ung thư. ',
N'CÀ RỐT.jpg','2022-05-07 00:00:00.000',30,2,4)

----------------------------------TRÁI CÂY
INSERT INTO SANPHAM(TenSP,Giaban,Mota,Anhbia,Ngaycapnhat,Soluongton,MaloaiSP,MaNCC)VALUES(N'CHUỐI GIÀ HỮU CƠ',99000,N'Chuối già Nam Mỹ là vua Vitamin B6 trong các loại trái cây. Ba lợi ích của Vitamin B6 cho sự phát triển thai nhi và bé:
- Phát triển trí não trong thời kì mang thai và trẻ sơ sinh.
- Tăng cường hệ thống miễn dịch.
- Ngăn ngừa thiếu máu. ',
N'CHUỐI GIÀ HỮU CƠ.jpg','2022-05-07 00:00:00.000',30,4,3)
INSERT INTO SANPHAM(TenSP,Giaban,Mota,Anhbia,Ngaycapnhat,Soluongton,MaloaiSP,MaNCC)VALUES(N'CAM XOÀN',99000,N'Nhờ lượng vitamin C dồi dào trong cam xoàn giúp cơ thể tăng cường sự trao đổi chất, tăng sức đề kháng hiệu quả. Lượng chất xơ trong cam giúp hỗ trợ quá trình tiêu hóa. Axit folic trong cam còn tốt cho bà bầu, giúp thai nhi phát triển tốt nhất, ngăn ngừa dị tật bẩm sinh ở trẻ. ',
N'CAM XOÀN.jpg','2022-05-07 00:00:00.000',30,4,3)
INSERT INTO SANPHAM(TenSP,Giaban,Mota,Anhbia,Ngaycapnhat,Soluongton,MaloaiSP,MaNCC)VALUES(N'DỨA HOÀNG HẬU',99000,N'Dứa Hoàng Hậu hay còn gọi là Dứa Gai được trồng ở Nghệ An. Là loại dứa có thịt vàng, miếng dứa giòn và vị ngọt nhiều. Đây là một sản phẩm được Organica chọn lọc từ vùng đất có thời tiết khắc nghiệt ở Bắc Trung Bộ, mùa đông thì lạnh cóng và mùa hè thì bỏng rát với gió Lào. Mùa nào thức ấy, hi vọng đây sẽ là 1 sản phẩm theo mùa và được mọi người yêu thích để năm sau Organica mạnh dạn nhân trồng trong hệ thống vườn của mình. ',
N'DỨA HOÀNG HẬU.jpg','2022-05-07 00:00:00.000',30,4,2)
INSERT INTO SANPHAM(TenSP,Giaban,Mota,Anhbia,Ngaycapnhat,Soluongton,MaloaiSP,MaNCC)VALUES(N'BƯỞI NĂM ROI',99000,N'Bưởi giàu hàm lượng vitamin C cùng chất chống oxy hóa, có khả năng bảo vệ tế bào khỏi sự gây hại của vi khuẩn và virus. Hàm lượng vitamin C cao chứa trong bưởi là yếu tố khiến bưởi đứng ở nhóm đầu bảng xếp hạng trong nhóm cây họ cam. Ngoài ra, các loại vitamin và khoáng chất khác chứa trong bưởi cũng rất tốt cho hệ miễn dịch. ',
N'BƯỞI NĂM ROI.jpg','2022-05-07 00:00:00.000',30,4,3)

----------------------------------QUẢ
INSERT INTO SANPHAM(TenSP,Giaban,Mota,Anhbia,Ngaycapnhat,Soluongton,MaloaiSP,MaNCC)VALUES(N'SU SU HỮU CƠ',99000,N'Su su chứa nhiều vitamin, khoáng chất và chất xơ thiết yếu. Su su giàu dinh dưỡng, chứa các chất chống oxy hóa, tăng cường sức khỏe tim mạch, thức đẩy quá trình kiểm soát đường máu, hỗ trợ thai kì khỏe mạnh vì chưa nhiều folate, phòng chống ung thư, làm chậm quá trình lão hóa, hỗ trợ tiêu hóa, hỗ trợ gan, kiểm soát cân nặng. ',
N'SU SU HỮU CƠ.png','2022-05-07 00:00:00.000',30,3,3)
INSERT INTO SANPHAM(TenSP,Giaban,Mota,Anhbia,Ngaycapnhat,Soluongton,MaloaiSP,MaNCC)VALUES(N'DƯA LEO',99000,N'Dưa leo chứa hàm lượng calo thấp nhưng lại rất giàu dưỡng chất quan trọng cho cơ thể. Ngoài hàm lượng nước cao thì dưa leo chứa rất nhiều vitamin, khoáng chất đa dạng như vitamin C, K, magie, kali, mangan… Do đó, bạn có thể cung cấp vitamin và khoáng chất hiệu quả bằng cách ăn dưa leo mỗi ngày. ',
N'Dualeo.png','2022-05-07 00:00:00.000',30,3,3)
INSERT INTO SANPHAM(TenSP,Giaban,Mota,Anhbia,Ngaycapnhat,Soluongton,MaloaiSP,MaNCC)VALUES(N'KHỔ QUA HỮU CƠ',99000,N'Lượng vitamin C trong khổ qua giúp tăng sức đề kháng cho cơ thể, kháng viêm tốt, ngăn ngừa và có tác dụng tiêu diệt tế bào ung thư. Kali trong khổ qua chứa có tác dụng giảm huyết áp, beta-carotene giúp sáng mắt, giúp điều trị chứng trào ngược axit và chứng khó tiêu. ',
N'KHỔ QUA HỮU CƠ.png','2022-05-07 00:00:00.000',30,3,3)
INSERT INTO SANPHAM(TenSP,Giaban,Mota,Anhbia,Ngaycapnhat,Soluongton,MaloaiSP,MaNCC)VALUES(N'BÀU',99000,N'Bầu là thực phẩm quen thuộc của người Việt, có rất nhiều cách chế biến món ăn từ quả bầu như nấu canh với tôm, xương thịt, luộc chấm kho quet,... thậm chí nước ép tươi từ quả bầu cũng dần phổ biến hơn. ',
N'bau.png','2022-05-07 00:00:00.000',30,3,1)
INSERT INTO SANPHAM(TenSP,Giaban,Mota,Anhbia,Ngaycapnhat,Soluongton,MaloaiSP,MaNCC)VALUES(N'ỚT CHUÔNG',99000,N'Ớt chuông có hàm lượng vitamin C cao hơn cả cam, chanh, đu đủ, dứa/ thơ. Tại Organica, chúng tôi trồng được những trái ớt chuông hữu cơ có cùi giày, ăn giòn ngọt không khác gì trái cây. ',
N'OtChuon.png','2022-05-07 00:00:00.000',30,3,4)
INSERT INTO SANPHAM(TenSP,Giaban,Mota,Anhbia,Ngaycapnhat,Soluongton,MaloaiSP,MaNCC)VALUES(N'BÍ ĐỎ HỮU CƠ',99000,N'Bí đỏ là thực phẩm chứa nhiều chất xơ có tác dụng làm chậm tốc độ hấp thụ đường vào máu, cũng như thúc đẩy nhu động ruột thường xuyên và tiêu hóa trơn tru hơn. Với hàm lượng vitamin, khoáng chất và chất chống oxy hóa cao, bí đỏ hỗ trợ tăng cường hệ thống miễn dịch, bảo vệ thị lực, giảm nguy cơ mắc một số bệnh ung thư và tăng cường sức khỏe của trái tim và làn da. ',
N'Bido.png','2022-05-07 00:00:00.000',30,3,1)
INSERT INTO SANPHAM(TenSP,Giaban,Mota,Anhbia,Ngaycapnhat,Soluongton,MaloaiSP,MaNCC)VALUES(N'ĐẬU CUVE',99000,N'Một số lợi ích sức khỏe của đậu que: tăng cường sức khỏe tim mạch; ngăn ngừa ung thư ruột già và điều trị các vấn đề về dạ dày, ruột; cải thiện sức khỏe xương; tốt cho mắt. Vitamin B12, magie, chất xơ và folate trong đậu que giúp giảm cholesterol, ngừa bệnh cao huyết áp và thúc đẩy lưu thông tuần hoàn máu. ',
N'daucuve.png','2022-05-07 00:00:00.000',30,3,4)
----------------------------------NGŨ CỐC
INSERT INTO SANPHAM(TenSP,Giaban,Mota,Anhbia,Ngaycapnhat,Soluongton,MaloaiSP,MaNCC)VALUES(N'ĐẬU ĐEN',99000,N'Sản phẩm được phát triển bởi cộng đồng dân tộc thiểu số tỉnh Quảng Nam với sự hỗ trợ từ Tổ chức Cứu trợ/ Phát triển Quốc Tế (FIDR) nhằm thúc đẩy phát triển nông thôn bền vũng. ',
N'ĐẬU ĐEN.png','2022-05-07 00:00:00.000',30,5,1)
INSERT INTO SANPHAM(TenSP,Giaban,Mota,Anhbia,Ngaycapnhat,Soluongton,MaloaiSP,MaNCC)VALUES(N'ĐẬU NÀNH HỮU CƠ',99000,N'Đậu nành là một nguồn thực phẩm có giá trị dinh dưỡng cao, giàu chất đạm, isoflavones, chất béo chưa bão hòa, các vitamin, khoáng chất, carbohydrate phức hợp và chất xơ. Các sản phẩm từ đậu nành như đậu phụ, sữa đậu nành, tào phớ, cháo…là những món ăn phong phú, ngon miệng. Ngoài ra đậu nành còn có các công dụng sau: tốt cho tim mạch; giảm nguy cơ béo phì; giảm nguy cơ tiểu đường; ngăn ngừa loãng xương; ngăn ngừa ung thư; ngăn ngừa các triệu chứng của thời kỳ mãn kinh. ',
N'ĐẬU NÀNH HỮU CƠ.png','2022-05-07 00:00:00.000',30,5,1)
INSERT INTO SANPHAM(TenSP,Giaban,Mota,Anhbia,Ngaycapnhat,Soluongton,MaloaiSP,MaNCC)VALUES(N'ĐẬU PHỘNG',99000,N'Đậu phộng vỏ đỏ chứa vitamin E, protein, khoáng chất như mangan, hỗ trợ cơ thể hấp thu canxi và ổn định đường huyết. Các dưỡng chất trong đậu phộng còn có tác dụng hỗ trợ kiểm soát hàm lượng cholesterol có trong máu. Đậu phộng thích hợp dùng làm nguyên liệu chế biến nhiều món ngon. ',
N'ĐẬU PHỘNG.png','2022-05-07 00:00:00.000',30,5,1)
----------------------------------GIA VỊ
INSERT INTO SANPHAM(TenSP,Giaban,Mota,Anhbia,Ngaycapnhat,Soluongton,MaloaiSP,MaNCC)VALUES(N'BỘT NÊM',99000,N'Chiết xuất nấm men được làm từ men bánh mì tự nhiên, đã được sử dụng hàng ngàn năm nay và có độ an toàn tuyệt đối với sức khỏe con người. Quá trình sản xuất hoàn toàn không dùng hóa chất, chỉ sử dụng các quá trình vi sinh, vật lý và được kiểm soát chặt chẽ. Chiết xuất nấm men chính là nhân của Tế bào Nấm Men, chủ yếu là men bánh mì (Saccharomyces cerevisiae) -> Nuôi bằng rỉ mật mía đường -> Phân giải -> Tách chiết -> Cô đặc -> (Sấy phun) -> Đóng gói. ',
N'BỘT NÊM.png','2022-05-07 00:00:00.000',30,6,1)
INSERT INTO SANPHAM(TenSP,Giaban,Mota,Anhbia,Ngaycapnhat,Soluongton,MaloaiSP,MaNCC)VALUES(N'LÁ HƯƠNG THẢO',99000,N'- Dùng cho món nướng như rau, khoai tây, cà rốt, bí ngô hoặc khoai lang.

- Ướp gia vị cho gà, thịt bò, thịt cừu.

- Dùng để nướng bánh, làm các món tráng miệng ',
N'LÁ HƯƠNG THẢO.png','2022-05-07 00:00:00.000',30,6,1)
INSERT INTO SANPHAM(TenSP,Giaban,Mota,Anhbia,Ngaycapnhat,Soluongton,MaloaiSP,MaNCC)VALUES(N'SỐT CÀ CHUA Ý HỮU CƠ',99000,N'Có thể dùng ngay, hoặc hâm nóng 10 phút rồi dùng với mỳ ý, nui, pizza… hoặc các món thịt, cá sốt cà. ',
N'SỐT CÀ CHUA Ý HỮU CƠ.jpg','2022-05-07 00:00:00.000',30,6,1)