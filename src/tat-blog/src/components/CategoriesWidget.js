import { useState, useEffect } from 'react'
import { Link } from 'react-router-dom'
import { ListGroup } from 'react-bootstrap'
import categoryApi from '../api/categoryApi'

export default function CategoriesWidget() {
  const [categories, setCategories] = useState([])

  useEffect(() => {
    ;(async () => {
      try {
        const data = await categoryApi.getAll({ PageSize: 10, PageNumber: 1 })
        if (data.isSuccess) {
          setCategories(data.result.items)
        }
      } catch (error) {
        console.log(error)
      }
    })()
  }, [])

  return (
    <div className='mb-4'>
      <h3 className='mb-2 text-success'>Các chủ đề</h3>
      {categories.length > 0 && (
        <ListGroup>
          {categories.map((category, index) => (
            <ListGroup.Item key={index}>
              <Link to={`/blog/category?slug=${category.urlSlug}`} title={category.description}>
                {category.name}
                <span>&nbsp;({category.postCount})</span>
              </Link>
            </ListGroup.Item>
          ))}
        </ListGroup>
      )}
    </div>
  )
}
