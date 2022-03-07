import React from "react";
import "./topbar.css";
import { Container, Dropdown, Nav, Navbar } from "react-bootstrap";
import { Person, Language } from "@mui/icons-material";

function Topbar({ setToken, title, url, userName, userEmail }) {
    function handleLogout() {
        setToken({});
        sessionStorage.clear();
    }

    return (
        <Navbar bg="light" expand="lg" fixed="top" className="topbar">
            <Container fluid>
                <Navbar.Brand href="">
                    <h2 className="topbarTitle">{title}</h2>
                </Navbar.Brand>
                <Navbar.Toggle aria-controls="basic-navbar-nav" />
                <Navbar.Collapse id="basic-navbar-nav">
                    <Nav className="me-auto">
                        <Nav.Link href="" className="languageWrapper">
                            <Language />
                        </Nav.Link>

                        <Dropdown>
                            <Dropdown.Toggle
                                id="dropdown-basic"
                                className="dropdownUserImage"
                            >
                                <img
                                    className="accountAvatar"
                                    src={url}
                                    alt="user image"
                                />
                            </Dropdown.Toggle>

                            <Dropdown.Menu id="basic-nav-dropdown" align="end">
                                <Container className="userInfo">
                                    <p className="userName">{userName}</p>
                                    <p>{userEmail}</p>
                                </Container>
                                <Dropdown.Divider />
                                <Dropdown.Item href="">
                                    <Person className="iconProfile" />
                                    Profile
                                </Dropdown.Item>
                                <Dropdown.Divider />
                                <Dropdown.Item
                                    href=""
                                    onClick={handleLogout}
                                    className="logoutWrapper"
                                >
                                    Đăng xuất
                                </Dropdown.Item>
                            </Dropdown.Menu>
                        </Dropdown>
                    </Nav>
                </Navbar.Collapse>
            </Container>
        </Navbar>
    );
}

export default Topbar;