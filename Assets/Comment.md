# Ưu điểm

- Code ngắn gọn, dễ hiểu.
- Cách tổ chức thư mục hợp lý.

# Nhược điểm
- Class GameManager đảm nhiệm nhiều chức năng, nên break ra thành các class khác nhau.
- Hơi khó để phát triển tiếp mà không động vào code cũ
# Đề xuất
- Nên có class để quản lý các funtion UI , hiện tại đang được implement trong class GameManager .
- Sử dụng các interface và lớp PlayerController sẽ giao tiếp qua các interface đó để thực hiện các funtion 
Từ đó, mỗi khi thêm mới một tính năng như skin hoặc các tính năng liên quan đến trạng thái của player. 
Ta chỉ việc implement thông qua interface hoặc khi chỉnh sửa các tính năng đó, không phải sửa code trong cả PlayerController.

