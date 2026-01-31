import React, {useEffect, useMemo, useState} from "react";
import SearchIcon from "@/assets/svg/SearchIcon";
import ShareIcon from "@/assets/svg/ShareIcon";
import TelegramIcon from "@/assets/svg/TelegramIcon";
import FileIcon from "@/assets/svg/FileIcon";
import Tradeup from "@/assets/svg/Tradeup";
import UserIcon from "@/assets/svg/Usericon";
import { HEADLINES } from "../../constants";

import styles from "../../style.module.scss";
import { useRouter } from "next/router";
import MenuIcon from '@mui/icons-material/Menu';
import {Alert, Button, Stack} from "@mui/material";
import {copyText} from "@/helpers/copyText";
import api from "@/service/api";
import Link from "next/link";
import ReviewIcon from "@/assets/svg/ReviewIcon";
import WalletIcon from "@/assets/svg/WalletIcon";
import ProductsIcon from "@/assets/svg/ProductsIcon";

const Header = ({ openMenu }: any) => {
  const router = useRouter();
  const headLine = useMemo(() => HEADLINES[router.pathname] || "", [router]);
  const isSearchVisible = useMemo(
    () => [""].includes(router.pathname),
    [router]
  );
  const [showAlert,setShowAlert] = useState(false)
  const [coursePath,setCoursePath] = useState('')
  const [accountId,setAccountId] = useState('')
  const [amount,setAmount] = useState(0)
  useEffect(()=>{
          api.get(`/api/v1/wallet/accounts`)
              .then(res => {
                if(res.status === 200){
                  setAmount(res.data[0]?.amount)
                  setAccountId(res.data[0]?.userId)
                }
              })
              .catch(e => console.log(e))
    setCoursePath(window.location.href)
  },[])
  return (
    <div
      className={`${styles.contentHeader} ${
        isSearchVisible ? styles.withSearch : ""
      }`}
    >
      <div className={styles.searchOverlay}>
        <div className={styles.burgerIconWrapper} onClick={openMenu}>
          <div className={styles.burgerIcon}>

          </div>
        </div>
        <h2 className={styles.searchHeadline}>{headLine}</h2>
        {isSearchVisible && (
          <div className={styles.inputOverlay}>
            <input type="text" placeholder="Название курса" />
            <div className={styles.searchIcon}>
              <SearchIcon />
            </div>
          </div>
        )}
      </div>
      <div className={styles.userActions}>
        <div className={styles.fone}>
          <ul>
            <li>
              <Button onClick={()=> {
                copyText(`${window.location.origin}/sign-up?referrer=${accountId}`)
                setShowAlert(true)
                setTimeout(()=>{setShowAlert(false)},3000)
              }}>
                <ShareIcon />
              </Button>
            </li>
            <li>
              <Button href='https://t.me/Ale_bot1' target='_blank'>
                <TelegramIcon />
              </Button>
            </li>
            <li>
              <Link href='/courses'>
                <Button>
                  <FileIcon />
                </Button>
              </Link>
            </li>
          </ul>
        </div>
        <Link href='/wallet'>
          <Button >
            <div className={styles.fone}>
              <div className={styles.tradeInfo}>
                <div className={styles.icon}>
                  <Tradeup />
                </div>
                $ {amount}
              </div>
            </div>
          </Button>
        </Link>

        <div className={`${styles.fone} ${styles.userIcon}`}>
          <Button href='/settings'>
            <UserIcon />
          </Button>
        </div>
      </div>
      {showAlert && <Stack sx={{width: '400px', position: 'fixed', right: '20px', bottom: '20px'}} spacing={2}>
        <Alert severity="success">Реферальная ссылка скопирована</Alert>
      </Stack>}
    </div>
  );
};

export default Header;
