import React, { useEffect } from 'react';

const Rss = () => {
  useEffect(() => {
    document.title = 'RSS Feed';
  }, []);
  return <h1>Đây là trang RSS</h1>;
};

export default Rss;
