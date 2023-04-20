import { Link } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { Button } from 'react-bootstrap';
import { faArrowLeft, faArrowRight } from '@fortawesome/free-solid-svg-icons';
import React from 'react';

const Pager = ({ postquery, metadata }) => {
  let pageCount = metadata.pageCount,
    hasNextPage = metadata.hasNextPage,
    hasPreviousPage = metadata.hasPreviousPage,
    pageNumber = metadata.pageNumber,
    pageSize = metadata.pageSize,
    actionName = '',
    slug = '',
    keyword = postquery.keyword ?? '';
  if (pageCount > 1) {
    return (
      <div className="text-center my-4">
        {hasPreviousPage ? (
          <Link
            to={`/Blog/${String(actionName)}?slug=${String(slug)}&k=${String(
              keyword
            )}&p=${Number(pageNumber - 1)}&ps=${Number(pageSize)}`}
            className="btn btn-info"
          >
            <FontAwesomeIcon icon={faArrowLeft} />
            &nbsp;Trang Trước
          </Link>
        ) : (
          <Button variant="outline-secondary" disabled>
            <FontAwesomeIcon icon={faArrowLeft} />
            &nbsp;Trang Trước
          </Button>
        )}
        {hasNextPage ? (
          <Link
            to={`/Blog/${String(actionName)}?slug=${String(slug)}&k=${String(
              keyword
            )}&p=${Number(pageNumber + 1)}&ps=${Number(pageSize)}`}
            className="btn btn-info"
          >
            <FontAwesomeIcon icon={faArrowRight} />
            Trang sau&nbsp;
          </Link>
        ) : (
          <Button variant="outline-secondary" className="ms-1" disabled>
            <FontAwesomeIcon icon={faArrowRight} />
            Trang sau&nbsp;
          </Button>
        )}
      </div>
    );
  }
  return <Link to={'/'}></Link>;
};

export default Pager;
