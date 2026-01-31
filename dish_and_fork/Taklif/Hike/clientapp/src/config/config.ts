const pageSize = parseInt(process.env.REACT_APP_ITEMS_PER_PAGE ?? '100', 10);

const config = {
  api: {
    authClientId: process.env.REACT_APP_API_AUTH_CLIENT_ID ?? '',
    authConfigUrl: process.env.REACT_APP_API_AUTH_CONFIG_URL ?? '',
    baseURL: process.env.REACT_APP_API_BASE_URL ?? '',

    keys: {
      getPartners: process.env.REACT_APP_API_KEYS_GET_PARTNERS ?? 'apiV1GetPartners',
    },

    retries: parseInt(process.env.REACT_APP_ORDERS_ITEMS_PER_PAGE ?? '3', 10),
  },

  hubs: {
    baseUrl: process.env.REACT_APP_SOCKET_HUBS ?? '',
  },

  applications: {
    pageSize: parseInt(process.env.REACT_APP_APPLICATIONS_ITEMS_PER_PAGE ?? '12', 10),
  },

  contacts: {
    telegram: process.env.REACT_APP_TELEGRAM_URL ?? '',
  },

  offers: {
    pageSize: process.env.REACT_APP_OFFERS_ITEMS_PER_PAGE
      ? parseInt(process.env.REACT_APP_OFFERS_ITEMS_PER_PAGE, 10)
      : pageSize,
  },

  orders: {
    pageSize: process.env.REACT_APP_ORDERS_ITEMS_PER_PAGE
      ? parseInt(process.env.REACT_APP_ORDERS_ITEMS_PER_PAGE, 10)
      : pageSize,
  },

  pages: {
    pageNumberParamName: 'pageNumber',
    pageSize,
    pageSizeParamName: 'pageSize',
  },

  product: {
    maxImages: 4,
  },

  roles: {
    admin: process.env.REACT_APP_ROLES_ADMIN ?? 'ADMIN',
    seller: process.env.REACT_APP_ROLES_SELLER ?? 'SELLER',
  },

  storageKeys: {
    lastOrderDataStorageKey: 'dnf:lastOrderDataStorageKey',
    logoutCallback: 'dnf:logoutCallback',
  },

  storefront: {
    pageSize: parseInt(process.env.REACT_APP_STOREFRONT_ITEMS_PER_PAGE ?? '25', 10),
  },

  messaging: {
    firebaseConfig: {
      apiKey: 'AIzaSyDlgX8WS7VDbwdFJgQaMFKOZl9aEOLm5iE',
      authDomain: 'dishandfork-53d91.firebaseapp.com',
      projectId: 'dishandfork-53d91',
      storageBucket: 'dishandfork-53d91.appspot.com',
      messagingSenderId: '162543491524',
      appId: '1:162543491524:web:5f213e3e3c04361c812c91',
      measurementId: 'G-2DT2WEJ49T',
    },
    vapidKey: 'BK8swPzf_THAE2P89XzKgy0o3FdUkEfuuEkx1rL4VCSAmTjO-vlzpzdX8cvRN3S5wW0kGjlnNp9c1hbhHhKgreg',
    localStorageToken: 'device_token',
  },

  isTestingMode: false,

  yooReferalLink: 'https://yoomoney.ru/joinups/?source=mp-317814',
};

export default config;
