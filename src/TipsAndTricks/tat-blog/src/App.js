import Navbar from './Components/Navbar'
import Sidebar from './Components/Sidebar';
import Footer from './Components/Footer';
import Index from './Pages/Index';


import Contact from './Pages/Contact'
import About from './Pages/About'
import Rss from './Pages/Rss'
import './App.css';
import React from 'react';
import {
   BrowserRouter as Router,
   Routes,
   Route
 } from 'react-router-dom';



function App() {
  return (
    <div>
      <Router>
        <Navbar />
        <div className='container-fluid'>
          <div className='row'>
              <div className='col-9'>
                 <Routes>                  
                    <Route path='/' element={<Index/>}></Route>
                    <Route path='/blog' element={<Index/>}></Route>
                    <Route path='/blog/Contact' element={<Contact/>}></Route>
                    <Route path='/blog/About' element={<About/>}></Route>
                    <Route path='/blog/Rss' element={<Rss/>}></Route>
                 </Routes>
              </div>
              <div className='col-3 border-start'>
                <Sidebar/>
              </div>
          </div>
        </div>
        <Footer/>
      </Router>
    </div>
  );
}


export default App;