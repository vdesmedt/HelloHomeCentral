import logo from '../assets/hh-logo.png'
import styles from './Banner.module.css'
import {Component} from "react";

class Banner extends Component<{ headerText: string }> {
    render() {
        const {headerText: headerText} = this.props;
        return (
            <header className="row">
                <div className="col-lg-5">
                    <img src={logo} alt="logo" className={styles.logo}/>
                </div>
                <div className="col-lg-7">
                    <h1>{headerText}</h1>
                </div>
            </header>);
    }
}

export default Banner;