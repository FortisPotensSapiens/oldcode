import { FC, lazy } from 'react';
import { Route, Routes as RouterRoutes } from 'react-router-dom';

import { useAuthConfig } from '~/api';
import { useConfig } from '~/contexts';
import { withSuspend } from '~/hocs';
import {
  ABOUT_PART,
  ADMIN_COLLENCTIONS_PART,
  ADMIN_GLOBAL_PART,
  ADMIN_PART,
  ADMIN_PRODUCTS_PART,
  ADMIN_SELERS_PART,
  ADMIN_TAGS_PART,
  BECOME_SELLER_PART,
  CART_PART,
  INDIVIDUAL_ORDER_ID_PART,
  INDIVIDUAL_ORDER_OFFER_ID_PART,
  OFFER_ID_PART,
  ORDER_ID_PART,
  ORDER_PART,
  ORDERS_PART,
  PARTNER_NEW_APPS_PART,
  PARTNER_NEW_PRODUCT_PART,
  PARTNER_OFFERS_PART,
  PARTNER_ORDERS_PART,
  PARTNER_PART,
  PARTNER_PRODUCTS_PART,
  PARTNER_SETTINGS_PART,
  PLACE_INDIVIDUAL_ORDER_PART,
  PLACE_ORDER_PART,
  PRODUCT_ID_PART,
  PRODUCT_PART,
  PROFILE_PART,
  REPEAT_ORDER_PART,
  SELLER_ID_PART,
  SELLER_PART,
  SELLER_PRODUCT_PART,
  SELLERS_PART,
} from '~/routing';
import { INDIVIDUAL_ORDER_PART } from '~/routing/individualOrder';

import { Layout } from './Layout';
import { Logout } from './Logout';
import { Protected } from './Protected';

const About = withSuspend(lazy(() => import(/* webpackChunkName: "about" */ '~/pages/About')));
const BecomeSeller = withSuspend(lazy(() => import(/* webpackChunkName: "become-seller" */ '~/pages/BecomeSeller')));
const Cart = withSuspend(lazy(() => import(/* webpackChunkName: "cart" */ '~/pages/Cart')));
const NotFound = withSuspend(lazy(() => import(/* webpackChunkName: "not-found" */ '~/pages/NotFound')));
const Order = withSuspend(lazy(() => import(/* webpackChunkName: "order" */ '~/pages/Order')));
const Orders = withSuspend(lazy(() => import(/* webpackChunkName: "orders" */ '~/pages/Orders')));
const PlaceOrder = withSuspend(lazy(() => import(/* webpackChunkName: "place-order" */ '~/pages/PlaceOrder')));
const Product = withSuspend(lazy(() => import(/* webpackChunkName: "product" */ '~/pages/Product')));
const Profile = withSuspend(lazy(() => import(/* webpackChunkName: "profile" */ '~/pages/Profile')));
const RepeatOrder = withSuspend(lazy(() => import(/* webpackChunkName: "repeat-order" */ '~/pages/RepeatOrder')));
const Seller = withSuspend(lazy(() => import(/* webpackChunkName: "seller" */ '~/pages/Seller')));
const Sellers = withSuspend(lazy(() => import(/* webpackChunkName: "sellers" */ '~/pages/Sellers')));
const Storefront = withSuspend(
  lazy(() => import(/* webpackChunkName: "storefront" */ '~/pages/Storefront/Storefront')),
);

const PartnerNewApp = withSuspend(lazy(() => import(/* webpackChunkName: "partner" */ '~/pages/PartnerNewApp')));
const PartnerNewApps = withSuspend(lazy(() => import(/* webpackChunkName: "partner" */ '~/pages/PartnerNewApps')));
const PartnerNewProduct = withSuspend(
  lazy(() => import(/* webpackChunkName: "partner" */ '~/pages/PartnerNewProduct')),
);
const PartnerOrders = withSuspend(lazy(() => import(/* webpackChunkName: "partner" */ '~/pages/PartnerOrders')));
const PartnerOrder = withSuspend(lazy(() => import(/* webpackChunkName: "partner" */ '~/pages/PartnerOrder')));
const PartnerProduct = withSuspend(lazy(() => import(/* webpackChunkName: "partner" */ '~/pages/PartnerProduct')));
const PartnerProducts = withSuspend(lazy(() => import(/* webpackChunkName: "partner" */ '~/pages/PartnerProducts')));
const PartnerSettings = withSuspend(lazy(() => import(/* webpackChunkName: "partner" */ '~/pages/PartnerSettings')));
const PartnerOffer = withSuspend(lazy(() => import(/* webpackChunkName: "admin" */ '~/pages/PartnerOffer')));
const PartnerOffers = withSuspend(lazy(() => import(/* webpackChunkName: "admin" */ '~/pages/PartnerOffers')));

const IndividualOrders = withSuspend(
  lazy(() => import(/* webpackChunkName: "induvidual_order" */ '~/pages/IndividualOrders/IndividualOrders')),
);
const IndividualOrder = withSuspend(
  lazy(() => import(/* webpackChunkName: "induvidual_order" */ '~/pages/IndividualOrder')),
);
const IndividualOrderOffer = withSuspend(
  lazy(() => import(/* webpackChunkName: "induvidual_order" */ '~/pages/IndividualOrderOffer/IndividualOrderOffer')),
);

const AdminPartners = withSuspend(lazy(() => import(/* webpackChunkName: "admin" */ '~/pages/AdminPartners')));
const AdminTags = withSuspend(lazy(() => import(/* webpackChunkName: "admin" */ '~/pages/AdminTags/AdminTags')));
const AdminProducts = withSuspend(
  lazy(() => import(/* webpackChunkName: "admin" */ '~/pages/AdminProducts/AdminProducts')),
);
const AdminGlobalSettings = withSuspend(
  lazy(() => import(/* webpackChunkName: "admin" */ '~/pages/AdminGlobalSettings/index')),
);
const AdminCollections = withSuspend(
  lazy(() => import(/* webpackChunkName: "admin" */ '~/pages/AdminCollections/index')),
);

const Test403Error = withSuspend(lazy(() => import(/* webpackChunkName: "admin" */ '~/pages/Test403Error')));

const PlaceIndividualOrder = withSuspend(
  lazy(() => import(/* webpackChunkName: "place-order" */ '~/pages/PlaceIndividualOrder/PlaceIndividualOrder')),
);

const Routes: FC = () => {
  const { data } = useAuthConfig();
  const { roles } = useConfig();

  if (!data) {
    return <></>;
  }

  return (
    <RouterRoutes>
      <Route element={<Layout />}>
        <Route element={<Protected />}>
          <Route element={<Test403Error />} path="/test403" />
        </Route>
      </Route>

      <Route element={<Layout />}>
        <Route element={<Storefront />} index />
      </Route>

      <Route path={new URL(String(data.redirect_uri)).pathname} />

      <Route element={<Logout />} path={new URL(String(data.post_logout_redirect_uri)).pathname} />

      <Route element={<Layout noSmallFooter noSmallHeader />}>
        <Route element={<Protected />}>
          <Route element={<BecomeSeller />} path={BECOME_SELLER_PART} />

          <Route path={PLACE_ORDER_PART}>
            <Route element={<PlaceOrder />} path={SELLER_ID_PART} />
          </Route>

          <Route path={PLACE_INDIVIDUAL_ORDER_PART}>
            <Route path={INDIVIDUAL_ORDER_ID_PART}>
              <Route path={INDIVIDUAL_ORDER_OFFER_ID_PART} element={<PlaceIndividualOrder />} />
            </Route>
          </Route>

          <Route path={REPEAT_ORDER_PART}>
            <Route element={<RepeatOrder />} path={ORDER_ID_PART} />
          </Route>
        </Route>
      </Route>

      <Route element={<Layout noSmallHeader />}>
        <Route element={<Protected role={roles.admin} />} path={ADMIN_PART}>
          <Route element={<AdminPartners />} path={ADMIN_SELERS_PART} />
          <Route element={<AdminTags />} path={ADMIN_TAGS_PART} />
          <Route element={<AdminProducts />} path={ADMIN_PRODUCTS_PART} />
          <Route element={<AdminGlobalSettings />} path={ADMIN_GLOBAL_PART} />
          <Route element={<AdminCollections />} path={ADMIN_COLLENCTIONS_PART} />
        </Route>

        <Route element={<Protected role={roles.seller} />} path={PARTNER_PART}>
          <Route element={<PartnerNewProduct />} path={PARTNER_NEW_PRODUCT_PART} />
          <Route element={<PartnerSettings />} path={PARTNER_SETTINGS_PART} />

          <Route path={PARTNER_NEW_APPS_PART}>
            <Route element={<PartnerNewApps />} index />
            <Route element={<PartnerNewApp />} path={INDIVIDUAL_ORDER_ID_PART} />
          </Route>

          <Route path={PARTNER_ORDERS_PART}>
            <Route element={<PartnerOrders />} index />
            <Route element={<PartnerOrder />} path={ORDER_ID_PART} />
          </Route>

          <Route path={PARTNER_PRODUCTS_PART}>
            <Route element={<PartnerProducts />} index />
            <Route element={<PartnerProduct />} path={PRODUCT_ID_PART} />
          </Route>

          <Route path={PARTNER_OFFERS_PART}>
            <Route element={<PartnerOffers />} index />
            <Route element={<PartnerOffer />} path={OFFER_ID_PART} />
          </Route>
        </Route>

        <Route element={<Protected />}>
          <Route element={<Orders />} path={ORDERS_PART} />
          <Route element={<Profile />} path={PROFILE_PART} />

          <Route path={ORDER_PART}>
            <Route element={<Order />} path={ORDER_ID_PART} />
          </Route>

          <Route path={INDIVIDUAL_ORDER_PART}>
            <Route element={<IndividualOrders />} index />
            <Route path={INDIVIDUAL_ORDER_ID_PART}>
              <Route element={<IndividualOrder />} index />
              <Route element={<IndividualOrderOffer />} path={INDIVIDUAL_ORDER_OFFER_ID_PART} />
            </Route>
          </Route>
        </Route>

        <Route path={SELLER_PART}>
          <Route path={SELLER_ID_PART}>
            <Route element={<Seller />} index />

            <Route path={SELLER_PRODUCT_PART}>
              <Route element={<Product />} path={PRODUCT_ID_PART} />
            </Route>
          </Route>
        </Route>

        <Route path={PRODUCT_PART}>
          <Route element={<Product />} path={PRODUCT_ID_PART} />
        </Route>

        <Route element={<About />} path={ABOUT_PART} />
        <Route element={<Cart />} path={CART_PART} />
        <Route element={<Sellers />} path={SELLERS_PART} />
        <Route element={<NotFound />} path="*" />
      </Route>
    </RouterRoutes>
  );
};

export { Routes };
