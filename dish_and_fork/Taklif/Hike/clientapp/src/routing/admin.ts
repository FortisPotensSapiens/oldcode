import { joinPath } from '~/utils';

const ADMIN_PART = 'admin';
const ADMIN_PRODUCTS_PART = 'products';
const ADMIN_SELERS_PART = 'sellers';
const ADMIN_USERS_PART = 'users';
const ADMIN_TAGS_PART = 'tags';
const ADMIN_COLLENCTIONS_PART = 'collections';
const ADMIN_GLOBAL_PART = 'global';

const getAdminProductsPath = (): string => joinPath(ADMIN_PART, ADMIN_PRODUCTS_PART);
const getAdminSellersPath = (): string => joinPath(ADMIN_PART, ADMIN_SELERS_PART);
const getAdminUsersPath = (): string => joinPath(ADMIN_PART, ADMIN_USERS_PART);
const getAdminTagsPath = (): string => joinPath(ADMIN_PART, ADMIN_TAGS_PART);
const getAdminGlobalPath = (): string => joinPath(ADMIN_PART, ADMIN_GLOBAL_PART);
const getAdminCollectionsPath = (): string => joinPath(ADMIN_PART, ADMIN_COLLENCTIONS_PART);

export {
  ADMIN_COLLENCTIONS_PART,
  ADMIN_GLOBAL_PART,
  ADMIN_PART,
  ADMIN_PRODUCTS_PART,
  ADMIN_SELERS_PART,
  ADMIN_TAGS_PART,
  ADMIN_USERS_PART,
  getAdminCollectionsPath,
  getAdminGlobalPath,
  getAdminProductsPath,
  getAdminSellersPath,
  getAdminTagsPath,
  getAdminUsersPath,
};
