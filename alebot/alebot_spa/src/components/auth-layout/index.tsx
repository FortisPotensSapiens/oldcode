import React, { FC, useMemo } from "react";

import Link from "next/link";
import { Box, Typography } from "@mui/material";
import AlebotSvg from "@/assets/svg/AlebotSvg";
import { useRouter } from "next/router";

import styles from "./styles.module.scss";

interface IAuthLayoutProps {
  children: React.JSX.Element;
}

const AuthLayout: FC<IAuthLayoutProps> = ({ children }) => {
  const router = useRouter();
  
  const isForgotPage = useMemo(() => router.pathname === "/forgot", [router]);
  const isSignInPage = useMemo(() => router.pathname === "/sign-in", [router]);
  const isSignUpPage = useMemo(() => router.pathname === "/sign-up", [router]);

  return (
    <>
      <div className={styles.container}>
        <div className={styles.content}>
          <div className={styles.logo}>
            <AlebotSvg />
          </div>
          {!isForgotPage && (
            <div className={styles.links}>
              <Link
                href="/sign-in"
                className={isSignInPage ? styles.active : undefined}
              >
                <Typography>Вход</Typography>
              </Link>
              <Typography>/</Typography>
              <Link
                href="/sign-up"
                className={isSignUpPage ? styles.active : undefined}
              >
                <Typography>Регистрация</Typography>
              </Link>
            </div>
          )}
          {children}
        </div>
      </div>
      <footer>
        <Box className="footerItem">© ALE BOT. Все права защищены.</Box>
        <Box className="footerItem"><a href="https://docs.google.com/document/d/1esAdDauzeZqH1I1Au6PUMMT4agJ8Yiq9/edit?usp=drive_link&ouid=100253606333501006370&rtpof=true&sd=true" target="_blank">Пользовательское Соглашение </a></Box>
      </footer>
    </>
  );
};

export default AuthLayout;
