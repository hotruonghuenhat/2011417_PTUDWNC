import { Link } from 'react-router-dom'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faArrowLeft, faArrowRight } from '@fortawesome/free-solid-svg-icons'
import { Button } from 'react-bootstrap'

export default function Pager({ postQuery, metadata }) {
  const { keyword } = postQuery || ''
  const { pageCount, hasNextPage, hasPreviousPage, pageNumber, pageSize } = metadata

  return (
    <>
      {pageCount > 1 ? (
        <div className='my-4 text-center'>
          {hasPreviousPage ? (
            <Link
              to={`/blog/?Keyword=${keyword}&PageNumber=${pageNumber - 1}&PageSize=${pageSize}`}
              className='btn btn-info'
            >
              <FontAwesomeIcon icon={faArrowLeft} />
              &nbsp;Trang trước
            </Link>
          ) : (
            <Button variant='outline-secondary' disabled>
              <FontAwesomeIcon icon={faArrowLeft} />
              &nbsp;Trang trước
            </Button>
          )}
          {hasNextPage ? (
            <Link
              to={`/blog/?Keyword=${keyword}&PageNumber=${pageNumber + 1}&PageSize=${pageSize}`}
              className='btn btn-info ms-1'
            >
              Trang sau&nbsp;
              <FontAwesomeIcon icon={faArrowRight} />
            </Link>
          ) : (
            <Button variant='outline-secondary' className='ms-1' disabled>
              Trang sau&nbsp;
              <FontAwesomeIcon icon={faArrowRight} />
            </Button>
          )}
        </div>
      ) : null}
    </>
  )
}
