import './App.css'
import Navbar from './components/Navbar'
import { BrowserRouter, Routes, Route } from 'react-router-dom'
import Sidebar from './components/Sidebar'
import Footer from './components/Footer'
import Index from './pages/Index'
import Layout from './pages/Layout'
import Contact from './pages/Contact'
import About from './pages/About'
import Rss from './pages/Rss'

function App() {
  return (
    <div>
      <BrowserRouter>
        <Navbar />
        <div className='container-fluid'>
          <div className='row'>
            <div className='col-9'>
              <Routes>
                <Route path='/' element={<Layout />}>
                  <Route path='/' element={<Index />} />
                  <Route path='blog' element={<Index />} />
                  <Route path='blog/contact' element={<Contact />} />
                  <Route path='blog/About' element={<About />} />
                  <Route path='blog/Rss' element={<Rss />} />
                </Route>
              </Routes>
            </div>

            <div className='col-3 border-start'>
              <Sidebar />
            </div>
          </div>
        </div>
        <Footer />
      </BrowserRouter>
    </div>
  )
}

export default App
