/* eslint-disable import/export */

import { joinPath } from '~/utils';

const getHomepagePath = (): string => joinPath();
const getStorefrontPath = getHomepagePath;

export * from './about';
export * from './admin';
export * from './cart';
export * from './individualOrder';
export * from './orders';
export * from './params';
export * from './partner';
export * from './products';
export * from './profile';
export * from './sellers';

export { getHomepagePath, getStorefrontPath };
