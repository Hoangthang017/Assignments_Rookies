### Cấu trúc: AAA

- Arrange: chuẩn bị tất cả dữ liệu cần thiết và điều kiện tiên quyết.
- Act: thực hiện kiểm tra.
- Assert: Kiểm tra kết quả mong đợi xảy ra chưa.

### TDD: Test-Driven Development

- Quá trình phát triển phần mềm nhằm thúc đẩy việc viết các bài kiểm tra trước khi viết mã ứng dụng.
- Giúp dẫn đến một chu kỳ phát triển ngắn và lặp đi lặp lại dựa trên việc viết một bài kiểm tra và để nó không thành công, sửa lỗi bằng cách viết mã ứng dụng và cấu trúc lại mã ứng dụng để dễ đọc và hiệu suất cao.

### Khái niệm
- *Unit test*: 
	+ Tập trhung vào việc kiểm tra một đơn vị mã: một khối xây dựng của ứng dụng phầm mềm.
	+ Ví dụ: một hàm hoặc một lớp.
	+ Unit test đảm bảo rằng một thành phần biệt lập của ứng dụng phần mềm hoạt động như mong đợi.
- *Integration test*:
	+ Giúp phát hiện bất kỳ vấn đề nào khi các đơn vị mã được lắp ráp lại với nhau để tạo ra các thành phần phức tạp.
	+ Trên thực tế, ngay cả khi mỗi đơn vị mã hoạt động chính xác trong một môi trường biệt lập, thì lúc kết hợp lại với nhau thì vẫn có thể xảy ra vấn đề.
- *End-to-End(E2E)*:
	+ Đảm bảo một chức năng cấp người dùng hoạt động như mong đợi.
	+ Ở một mức độ nào đó thì nó tương tự các bài integration test.
	+ Nó tập trung vào các chức năng mà người dùng phần mềm có thể truy cập trực tiếp hoặc bằng cách nào đó từ bên ngoài ứng dụng
	+ Một bài E2E có thể liên quan đến nhiều hệ thống và nhằm mục đích mô phỏng một kịch bản sản xuất.

=> Thông thường số lượng bài kiểm tra giảm trong khi chuyển từ bài unit test đến E2E

### Các từ khóa: 

#### Attribute:

- Fact: cho xUnit biết đây là một bài kiểm tra
- Theory: 
	+ Là một bài kiểm tra có tham số cho phép bạn thực hiện một tập hợp các bài kiểm tra đơn vị có cùng cấu trúc.
	+ Cho phép triển trai theo hướng kiểm thử theo hướng dữ liệu: là một phương pháp dựa vào sự biến đổi dữ liệu đầu vào.
	+ Bonus: ClassData, MemberData
- InlineData: đại diện cho một bộ dữ liệu được sử dụng trong Theory

#### Method

- Assert: Đối tượng cung cấp các phương thức để xác minh kết quả
- True: phương thức này thành công khi đối số đầu của nó là true. Nếu không nó sẽ trả về thông điệp của đối số thứ 2.
- False: thức này thành công khi đối số đầu của nó là false. Nếu không nó sẽ trả về thông điệp của đối số thứ 2.
- Mock: thiết lập một lớp "giả" trả về một giá trị đã biết cho các lệnh gọi nhất định.
	+ Setup: chỉ định phương thức cần mocking.
	+ It.IsAny<>(): chỉ định bất kì tham số nào cho phương thức.
	+ Return: chỉ định dữ liệu trả về cho phương thức.
	+ Throws: trả về exception
	+ Verify: đảm bảo rằng một phương thức mock được gọi với số lần xác định
	+ Times: mong đợi số lần gọi phương thức

