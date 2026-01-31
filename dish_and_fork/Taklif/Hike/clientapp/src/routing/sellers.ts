import { generatePath, Params, useParams } from 'react-router-dom';

import { joinPath } from '~/utils';

import { PRODUCT_ID_PARAM, PRODUCT_ID_PART, SELLER_ID_PARAM, SELLER_ID_PART } from './params';

type SellerParams = Params<typeof SELLER_ID_PARAM>;
type SellerProductParams = Params<typeof SELLER_ID_PARAM | typeof PRODUCT_ID_PARAM>;

const BECOME_SELLER_PART = 'become-seller';
const SELLER_PART = 'seller';
const SELLER_PRODUCT_PART = 'product';
const SELLERS_PART = 'sellers';

const getSellerPath = (): string => joinPath(SELLER_PART, SELLER_ID_PART);

const getSellersPath = (): string => joinPath(SELLERS_PART);

const getSellerProductPath = (): string => joinPath(SELLER_PART, SELLER_ID_PART, SELLER_PRODUCT_PART, PRODUCT_ID_PART);

const getBecomeSellerPath = (): string => joinPath(BECOME_SELLER_PART);

const generateSellerPath = (params: SellerParams): string => generatePath(getSellerPath(), params);

const generateSellerProductPath = (params: SellerProductParams): string => generatePath(getSellerProductPath(), params);

const useSellerParams = (): Readonly<Partial<SellerParams>> => useParams();

const useSellerProductParams = (): Readonly<Partial<SellerProductParams>> => useParams();

export {
  BECOME_SELLER_PART,
  generateSellerPath,
  generateSellerProductPath,
  getBecomeSellerPath,
  getSellerPath,
  getSellerProductPath,
  getSellersPath,
  SELLER_PART,
  SELLER_PRODUCT_PART,
  SELLERS_PART,
  useSellerParams,
  useSellerProductParams,
};

export type { SellerParams, SellerProductParams };
