import TagList from './TagList'
import Card from 'react-bootstrap/Card'
import { Link } from 'react-router-dom'
import { isEmptyOrSpaces } from '../utils/Utils'

const PostItem = ({ post }) => {
  let imageUrl = isEmptyOrSpaces(post.imageUrl)
    ? process.env.PUBLIC_URL + '/images/img1.jpg'
    : `${post.imageUrl}`

  let postedDate = new Date(post.postedDate)

  return (
    <article className='blog-entry mb-4'>
      <Card>
        <div className='row g-0'>
          <div className='col-md-4'>
            <Card.Img variant='top' src={imageUrl} alt={post.title} />
          </div>

          <div className='col-md-8'>
            <Card.Body>
              <Card.Title>{post.title}</Card.Title>

              <Card.Text>
                <small className='text-text-muted'>Tác giả: </small>
                <span className='text-primary m-1'>{post.author.fullname}</span>

                <small className='text-text-muted'>Chủ đề: </small>
                <span className='text-primary m-1'>{post.category.name}</span>
              </Card.Text>

              <Card.Text>{post.shortDescription}</Card.Text>

              <div className='tag-list'>
                <TagList tagList={post.tags} />
              </div>

              <div className='text-end'>
                <Link
                  to={`/blog/post?year=${postedDate.getFullYear()}&month=${postedDate.getMonth()}&day=${postedDate.getDay()}&slug=${
                    post.urlSlug
                  }`}
                  className='btn btn-primary'
                  title={post.title}
                >
                  Xem chi tiết
                </Link>
              </div>
            </Card.Body>
          </div>
        </div>
      </Card>
    </article>
  )
}

export default PostItem
