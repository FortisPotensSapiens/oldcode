import CourseIcon from "@/assets/svg/CourseIcon";
import LicenceIcon from "@/assets/svg/LicenceIcon";
import ProductsIcon from "@/assets/svg/ProductsIcon";
import ReferalIcon from "@/assets/svg/ReferalIcon";
import ReviewIcon from "@/assets/svg/ReviewIcon";
import WalletIcon from "@/assets/svg/WalletIcon";

export const LINKS = [
  { text: "Обзор", href: "/", icon: ReviewIcon },
  { text: "Мой кошелек", href: "/wallet", icon: WalletIcon },
  { text: "Продукты", href: "/products", icon: ProductsIcon },
  { text: "Мои курсы", href: "/courses", icon: CourseIcon },
  { text: "Мои рефералы", href: "/referrals", icon: ReferalIcon },
  { text: "Сервера VDS", href: "/servers", icon: LicenceIcon },
];

export const HEADLINES: { [key: string]: string } = {
  "/": "Обзор",
  "/wallet": "Мой кошелек",
  "/products": "Продукты",
  "/courses": "Мои курсы",
  "/referrals": "Мои рефералы",
  "/servers": "Сервера VDS",
};
