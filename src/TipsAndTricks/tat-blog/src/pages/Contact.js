import React, { useEffect } from "react";
import PropTypes from "prop-types";

Contact.propTypes = {};

function Contact(props) {
  useEffect(() => {
    document.title = "Trang lien hệ";
  });

  return (
    <div>
      <h1>Đây là trang lien hệ</h1>
    </div>
  );
}

export default Contact;
