import Logo from "@/assets/images/logo.png";
import ProductsIcon from "@/assets/svg/ProductsIcon";
import Image from "next/image";

import React, { FC, useState } from "react";
import Link from "next/link";
import { useRouter } from "next/router";
import styles from "../../style.module.scss";
import { LINKS } from "../../constants";
import SettingIcon from "@/assets/svg/SettingIcon";
import LogoutIcon from "@/assets/svg/LogoutIcon";
import ArrowLeftIcon from "@/assets/svg/ArrowLeftIcon";
import CloseIcon from "@/assets/svg/CloseIcon";

const Sidebar: FC<any> = ({ isMenuVisible, closeMenu }) => {
  const router = useRouter();
  const onLogout = () => {
    localStorage.removeItem("accessToken");
    localStorage.removeItem("refreshToken");
    router.push("/sign-in");
  };

  return (
    <nav className={`${styles.sidebar} ${!isMenuVisible ? styles.hide : ""}`}>
      <div className={styles.hideButtonWrapper}>
        <div className={styles.hideButton} onClick={closeMenu}>
          <CloseIcon/>
        </div>
      </div>
      <div className={styles.sidebarContent}>
        <Link className={styles.brand} href="/">
          <Image src={Logo} alt="Logo" priority={true} />
        </Link>
        <ul>
          {LINKS.map(({ href, icon: Icon, text }) => (
            <li
              key={href}
              className={`${router.pathname === href ? styles.active : ""}`}
              onClick={closeMenu}
            >
              <Link className={styles.listItemContent} href={href}>
                <div className={styles.icon}>
                  <Icon fill={router.pathname === href ? "#fff" : undefined} />
                </div>
                <p className={styles.listItemName}>{text}</p>
              </Link>
            </li>
          ))}

          <li
            className={`${
              router.pathname === "/settings" ? styles.active : ""
            }`}
            onClick={closeMenu}
          >
            <Link className={styles.listItemContent} href="/settings">
              <div className={styles.icon}>
                <SettingIcon
                  fill={router.pathname === "/settings" ? "#fff" : undefined}
                />
              </div>
              <p className={styles.listItemName}>Настройки</p>
            </Link>
          </li>
          <li>
            <div className={styles.listItemContent} onClick={onLogout}>
              <div className={styles.icon}>
                <LogoutIcon />
              </div>
              <p className={styles.listItemName}>Выйти</p>
            </div>
          </li>
        </ul>
      </div>
    </nav>
  );
};

export default Sidebar;
