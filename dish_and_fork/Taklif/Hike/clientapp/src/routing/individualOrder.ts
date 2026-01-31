import { generatePath } from 'react-router-dom';

import { joinPath } from '~/utils';

import { INDIVIDUAL_ORDER_ID_PART, INDIVIDUAL_ORDER_OFFER_ID_PART } from './params';

export const INDIVIDUAL_ORDER_PART = 'individual-orders';

export const getIndividualOrdersPath = (): string => joinPath(INDIVIDUAL_ORDER_PART);

export const getIndividualOrderPath = (): string => joinPath(INDIVIDUAL_ORDER_PART, INDIVIDUAL_ORDER_ID_PART);

export const getIndividualOrderOfferPath = (): string =>
  joinPath(INDIVIDUAL_ORDER_PART, INDIVIDUAL_ORDER_ID_PART, INDIVIDUAL_ORDER_OFFER_ID_PART);

export const generateIndividualOrderPath = (individualOrderId: string): string => {
  return generatePath(getIndividualOrderPath(), { individualOrderId });
};

export const generateIndividualOrderOfferPath = (individualOrderId: string, individualOrderOfferId: string): string => {
  return generatePath(getIndividualOrderOfferPath(), { individualOrderId, individualOrderOfferId });
};
