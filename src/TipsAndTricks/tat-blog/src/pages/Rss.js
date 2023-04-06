import React, { useEffect } from "react";
import PropTypes from "prop-types";

Rss.propTypes = {};

function Rss(props) {
  useEffect(() => {
    document.title = "Trang RSS";
  });

  return (
    <div>
      <h1>Đây là trang RSS</h1>
    </div>
  );
}

export default Rss;
