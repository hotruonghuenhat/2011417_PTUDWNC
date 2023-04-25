import React from "react";
import PropTypes from "prop-types";

Footer.propTypes = {};

function Footer(props) {
  return (
    <footer className="border-top footer text-muted">
      <div className="container-fluid text-center">
        &copy; 2023 - Tips & Tricks blog
      </div>
    </footer>
  );
}

export default Footer;
