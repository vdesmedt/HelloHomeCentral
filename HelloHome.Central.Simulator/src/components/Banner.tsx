import logo from '../assets/hh-logo.png'
import styles from './Banner.module.css'
import {Component} from "react";

class Banner extends Component<{ headerText: string }> {
    render() {
        const {headerText: headerText} = this.props;
        return (
            <nav className="navbar navbar-dark mb-4">
                <div className="container-fluid">
            <span className="navbar-brand">
                <img src={logo} className={styles.logo} alt="HelloHome Logo"/>
                <i className="bi"></i> {headerText}
            </span>
                    <span className="text-white">
                <i className="bi bi-circle-fill text-success me-1"></i> System Online
            </span>
                </div>
            </nav>);
    }
}

export default Banner;