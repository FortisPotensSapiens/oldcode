import { generatePath, Params, useParams } from 'react-router-dom';

import { joinPath } from '~/utils';

import {
  INDIVIDUAL_ORDER_ID_PARAM,
  INDIVIDUAL_ORDER_ID_PART,
  OFFER_ID_PARAM,
  OFFER_ID_PART,
  ORDER_ID_PART,
  PRODUCT_ID_PARAM,
  PRODUCT_ID_PART,
} from './params';

type AppParams = Params<typeof INDIVIDUAL_ORDER_ID_PARAM>;
type OfferParams = Params<typeof OFFER_ID_PARAM>;
type ProductParams = Params<typeof PRODUCT_ID_PARAM>;

const PARTNER_APPS_PART = 'apps';
const PARTNER_NEW_APPS_PART = 'new-apps';
const PARTNER_NEW_PRODUCT_PART = 'new-product';
const PARTNER_PART = 'partner';
const PARTNER_ORDERS_PART = 'orders';
const PARTNER_OFFERS_PART = 'offers';
const PARTNER_PRODUCTS_PART = 'products';
const PARTNER_SETTINGS_PART = 'settings';

const getPartnerNewAppPath = (): string => joinPath(PARTNER_PART, PARTNER_NEW_APPS_PART, INDIVIDUAL_ORDER_ID_PART);
const getPartnerNewAppsPath = (): string => joinPath(PARTNER_PART, PARTNER_NEW_APPS_PART);
const getPartnerAppsPath = (): string => joinPath(PARTNER_PART, PARTNER_APPS_PART);
const getPartnerNewProductPath = (): string => joinPath(PARTNER_PART, PARTNER_NEW_PRODUCT_PART);
const getPartnerOfferPath = (): string => joinPath(PARTNER_PART, PARTNER_OFFERS_PART, OFFER_ID_PART);
const getPartnerOffersPath = (): string => joinPath(PARTNER_PART, PARTNER_OFFERS_PART);
const getPartnerOrderPath = (): string => joinPath(PARTNER_PART, PARTNER_ORDERS_PART, ORDER_ID_PART);
const getPartnerOrdersPath = (): string => joinPath(PARTNER_PART, PARTNER_ORDERS_PART);
const getPartnerProductPath = (): string => joinPath(PARTNER_PART, PARTNER_PRODUCTS_PART, PRODUCT_ID_PART);
const getPartnerProductsPath = (): string => joinPath(PARTNER_PART, PARTNER_PRODUCTS_PART);
const getPartnerSettingsPath = (): string => joinPath(PARTNER_PART, PARTNER_SETTINGS_PART);

const generatePartnerNewAppPath = (individualOrderId: string): string =>
  generatePath(getPartnerNewAppPath(), { individualOrderId });
const generatePartnerProductPath = (params: ProductParams): string => generatePath(getPartnerProductPath(), params);
const generatePartnerOrderInfoPath = (orderId: string): string => generatePath(getPartnerOrderPath(), { orderId });

const usePartnerProductParams = (): Readonly<Partial<ProductParams>> => useParams();
const usePartnerNewAppParams = (): Readonly<Partial<AppParams>> => useParams();
const usePartnerOfferParams = (): Readonly<Partial<OfferParams>> => useParams();

export {
  generatePartnerNewAppPath,
  generatePartnerOrderInfoPath,
  generatePartnerProductPath,
  getPartnerAppsPath,
  getPartnerNewAppPath,
  getPartnerNewAppsPath,
  getPartnerNewProductPath,
  getPartnerOfferPath,
  getPartnerOffersPath,
  getPartnerOrderPath,
  getPartnerOrdersPath,
  getPartnerProductPath,
  getPartnerProductsPath,
  getPartnerSettingsPath,
  PARTNER_APPS_PART,
  PARTNER_NEW_APPS_PART,
  PARTNER_NEW_PRODUCT_PART,
  PARTNER_OFFERS_PART,
  PARTNER_ORDERS_PART,
  PARTNER_PART,
  PARTNER_PRODUCTS_PART,
  PARTNER_SETTINGS_PART,
  usePartnerNewAppParams,
  usePartnerOfferParams,
  usePartnerProductParams,
};
