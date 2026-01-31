import { generatePath, Params, useParams } from 'react-router-dom';

import { joinPath } from '~/utils';

import {
  INDIVIDUAL_ORDER_ID_PART,
  INDIVIDUAL_ORDER_OFFER_ID_PART,
  ORDER_ID_PARAM,
  ORDER_ID_PART,
  SELLER_ID_PARAM,
  SELLER_ID_PART,
} from './params';

type OrderParams = Params<typeof ORDER_ID_PARAM>;
type PlaceOrderParams = Params<typeof SELLER_ID_PARAM>;

const ORDER_PART = 'orders';
const ORDERS_PART = 'orders';
const PLACE_ORDER_PART = 'place-order';
const PLACE_INDIVIDUAL_ORDER_PART = 'place-individual-order';
const REPEAT_ORDER_PART = 'repeat-order';

const getOrdersPath = (): string => joinPath(ORDERS_PART);
const getOrderPath = (): string => joinPath(ORDER_PART, ORDER_ID_PART);
const getPlaceIndividualOrderPath = (): string =>
  joinPath(PLACE_INDIVIDUAL_ORDER_PART, INDIVIDUAL_ORDER_ID_PART, INDIVIDUAL_ORDER_OFFER_ID_PART);
const getPlaceOrderPath = (): string => joinPath(PLACE_ORDER_PART, SELLER_ID_PART);
const getRepeatOrderPath = (): string => joinPath(REPEAT_ORDER_PART, ORDER_ID_PART);
const generateOrderPath = (params: OrderParams): string => generatePath(getOrderPath(), params);
const generatePlaceIndividualOrderPath = (individualOrderId: string, individualOrderOfferId: string): string =>
  generatePath(getPlaceIndividualOrderPath(), {
    individualOrderOfferId,
    individualOrderId,
  });
const generatePlaceOrderPath = (params: PlaceOrderParams): string => generatePath(getPlaceOrderPath(), params);
const generateRepeatOrderPath = (params: OrderParams): string => generatePath(getRepeatOrderPath(), params);

const useOrderParams = (): Readonly<Partial<OrderParams>> => useParams();
const usePlaceOrderParams = (): Readonly<Partial<PlaceOrderParams>> => useParams();
const useRepeatOrderParams = (): Readonly<Partial<OrderParams>> => useParams();

export {
  generateOrderPath,
  generatePlaceIndividualOrderPath,
  generatePlaceOrderPath,
  generateRepeatOrderPath,
  getOrderPath,
  getOrdersPath,
  getPlaceIndividualOrderPath,
  getPlaceOrderPath,
  getRepeatOrderPath,
  ORDER_PART,
  ORDERS_PART,
  PLACE_INDIVIDUAL_ORDER_PART,
  PLACE_ORDER_PART,
  REPEAT_ORDER_PART,
  useOrderParams,
  usePlaceOrderParams,
  useRepeatOrderParams,
};
