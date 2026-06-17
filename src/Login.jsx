import { useState } from "react";

export default function Login() {
  const [error, setError] = useState("");

  const handleSubmit = (e) => {
    e.preventDefault();
    setError("Tên đăng nhập hoặc mật khẩu không đúng");
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-100">
      <div className="bg-white p-8 rounded-xl shadow-lg w-full max-w-md">
        <h1 className="text-2xl font-bold text-center mb-6">Đăng nhập</h1>
        <form onSubmit={handleSubmit}>
          <input type="text" placeholder="Tên đăng nhập" className="w-full border p-3 rounded mb-4" />
          <input type="password" placeholder="Mật khẩu" className="w-full border p-3 rounded mb-4" />
          {error && <p className="text-red-500 mb-3">{error}</p>}
          <button className="w-full bg-blue-600 text-white p-3 rounded">Đăng nhập</button>
        </form>
      </div>
    </div>
  );
}
