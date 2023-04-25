import { useState } from 'react'
import { Form, Button } from 'react-bootstrap'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faSearch } from '@fortawesome/free-solid-svg-icons'

export default function SearchForm() {
  const [keyword, setKeyword] = useState('')

  const handleSubmit = (e) => {
    e.preventDefault()
    window.location = `/blog?Keyword=${keyword}`
  }

  return (
    <div className='mb-4'>
      <Form method='get' onSubmit={handleSubmit}>
        <Form.Group className='input-group mb-3'>
          <Form.Control
            type='text'
            name='Keyword'
            placeholder='Enter keyword'
            value={keyword}
            onChange={(e) => {
              setKeyword(e.target.value)
            }}
            aria-label='Enter keyword'
            aria-describedby='btnSearchPost'
          />
          <Button id='btnSearchPost' variant='outline-secondary' type='submit'>
            <FontAwesomeIcon icon={faSearch} />
          </Button>
        </Form.Group>
      </Form>
    </div>
  )
}
