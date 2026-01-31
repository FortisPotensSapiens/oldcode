import { generatePath, Params, useParams } from 'react-router-dom';

import { joinPath } from '~/utils';

import { PRODUCT_ID_PARAM, PRODUCT_ID_PART } from './params';

type ProductParams = Params<typeof PRODUCT_ID_PARAM>;

const PRODUCT_PART = 'product';

const getProductPath = (): string => joinPath(PRODUCT_PART, PRODUCT_ID_PART);

const generateProductPath = (params: ProductParams): string => generatePath(getProductPath(), params);

const useProductParams = (): Readonly<Partial<ProductParams>> => useParams();

export { generateProductPath, getProductPath, PRODUCT_PART, useProductParams };
export type { ProductParams };
