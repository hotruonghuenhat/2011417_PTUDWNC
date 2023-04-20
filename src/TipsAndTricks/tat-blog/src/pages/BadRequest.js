import { Link } from 'react-router-dom';

export default function BadRequest() {
  // const { search } = useLocation();
  // const params = new URLSearchParams(search);
  // const redirectTo = params.get('redirectTo') ?? '/';

  return (
    <div className="d-flex flex-column align-items-center mt-5 pt-5">
      <img
        src="/assets/images/error.jpg"
        alt="not-found"
        className="rounded shadow"
        style={{ width: 512 }}
      />
      <h1 className="mt-4 fw-bold">400</h1>
      <p className="mb-3 fs-3">Yêu cầu không hợp lệ</p>
      <Link to="/" className="btn btn-primary">
        Quay về trang chủ
      </Link>
    </div>
  );
}
