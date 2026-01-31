import React, { FC, useEffect, useState } from "react";
import Sidebar from "./components/sidebar";
import Header from "./components/header";

import styles from "./style.module.scss";
import { useRouter } from "next/router";
import { getUserInfoRequest } from "@/service/auth";
import ReviewIcon from "@/assets/svg/ReviewIcon";
import WalletIcon from "@/assets/svg/WalletIcon";
import ProductsIcon from "@/assets/svg/ProductsIcon";
import Link from "next/link";
import api from "@/service/api";

interface ILayoutProps {
  children: React.JSX.Element;
}

const Layout: FC<ILayoutProps> = ({ children }) => {
  const [isMenuVisible, setIsMenuVisible] = useState(false);
  const openMenu = () => {
    setIsMenuVisible(true);
  };
  const closeMenu = () => {
    setIsMenuVisible(false);
  };
  const router = useRouter();
  useEffect(() => {

    console.log("USE EFFETC FROM APP");

    const accessToken = localStorage.getItem("accessToken");
    const refreshToken = localStorage.getItem("refreshToken");
    if (!refreshToken || !accessToken) {
      localStorage.removeItem("accessToken");
      localStorage.removeItem("refreshToken");
      router.push("/sign-in");
    } else {
      api.get('/api/v1/sso/users/my-user-info')
        .then(res => {
          amplitude.setUserId(res.data.email);
        })
        .catch(e => console.log(e))
    }
  }, [children]);
  return (
    <main className={`${styles.wrapper} ${isMenuVisible ? styles.isOpen : ""}`}>
      <Sidebar isMenuVisible={isMenuVisible} closeMenu={closeMenu} />
      <section className={styles.content}>
        <Header openMenu={openMenu} />
        <div className={styles["section-main-panel"]}>{children}</div>
        <div className={styles.mobileNav}>
          <Link href='/' className={styles.navItem}>
            <ReviewIcon/>
            <p>Обзор</p>
          </Link>
          <Link href='/wallet' className={styles.navItem}>
            <WalletIcon/>
            <p>Мой кошелек</p>
          </Link>
          <Link href='/products' className={styles.navItem}>
            <ProductsIcon/>
            <p>Продукты</p>
          </Link>
          <div className={styles.navItem}>
            <div className={styles.menuButton} onClick={()=>{ setIsMenuVisible(!isMenuVisible);}}>
              Меню
            </div>
          </div>
        </div>
      </section>
    </main>
  );
};

export default Layout;
