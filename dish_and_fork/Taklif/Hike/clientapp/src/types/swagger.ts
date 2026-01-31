/* eslint-disable */
/* tslint:disable */
/*
 * ---------------------------------------------------------------
 * ## THIS FILE WAS GENERATED VIA SWAGGER-TYPESCRIPT-API        ##
 * ##                                                           ##
 * ## AUTHOR: acacode                                           ##
 * ## SOURCE: https://github.com/acacode/swagger-typescript-api ##
 * ---------------------------------------------------------------
 */

export interface AddLoginModel {
  /** @minLength 1 */
  loginProvider: string;
}

export interface AddressCreateModel {
  /** Почтовый индекс */
  zipCode?: string | null;
  /** Домофон (код домофона) */
  intercom?: string | null;
  /**
   * Название улицы
   * @minLength 1
   */
  street: string;
  /**
   * Номер здания
   * @minLength 1
   */
  house: string;
  /** Номер квартиры или офиса */
  apartmentNumber?: string | null;
  /** Подьезд */
  entrance?: string | null;
  /**
   * Долгота
   * @format double
   */
  longitude?: number | null;
  /**
   * Широта
   * @format double
   */
  latitude?: number | null;
}

export interface AddressReadModel {
  /** Почтовый индекс */
  zipCode?: string | null;
  /** Домофон (код домофона) */
  intercom?: string | null;
  /**
   * Название улицы
   * @minLength 1
   */
  street: string;
  /**
   * Номер здания
   * @minLength 1
   */
  house: string;
  /** Номер квартиры или офиса */
  apartmentNumber?: string | null;
  /** Подьезд */
  entrance?: string | null;
  country?: string | null;
  /**
   * Для России это область за исключением Москвы и Питера которые сами являются регионами
   * Для США это Штат.
   * Для Китая надо разбиратся что там у них является еденицей административной в которую входя несколько городов.
   */
  region?: string | null;
  city?: string | null;
  /** @format double */
  longitude: number;
  /** @format double */
  latitude: number;
}

export interface AdminMerchandiseUpdateModel {
  /** @format uuid */
  id: string;
  /** Категории (теги) товара */
  categories: string[];
  isTagsAppovedByAdmin: boolean;
  /**
   * Комментарий почему товар заблокирован и что нужно сделать чтобы его разблокировать (картинку сменить например).
   * После установки этого поля товар перестанет показываться покупателям.
   */
  reasonForBlocking?: string | null;
}

/** Данные нарушили внутренние условия сервера */
export interface AppErrorModel {
  /** Сообщения */
  message?: string | null;
  /** Идентификатор по которому можно в логах остследить что произошло на сервере */
  traceId?: string | null;
  /**
   * Время сервера по которому можно отстелидить в логах что происходило на сервере
   * @format date-time
   */
  serverTime: string;
  /**
   * Полнительные данные о ошибке
   * Наример {id:3, name:'user'}
   */
  data?: Record<string, any>;
}

export interface AppProblemDetails {
  type?: string | null;
  title?: string | null;
  /** @format int32 */
  status?: number | null;
  detail?: string | null;
  instance?: string | null;
  errors?: Record<string, string[]>;
  traceId?: string | null;
  [key: string]: any;
}

/** Заявка на индивидуальный заказ */
export interface ApplicationCreateModel {
  /**
   * Название
   * @minLength 2
   * @maxLength 255
   */
  title: string;
  /** Описание */
  description?: string | null;
  /**
   * Дата с которой нужен результат
   * @format date-time
   */
  fromDate?: string | null;
  /**
   * Дата до которой нужен результат
   * @format date-time
   */
  toDate?: string | null;
  /**
   * Сумма минимальная за результат
   * @format double
   */
  sumFrom?: number | null;
  /**
   * Суммма максимальная за результат
   * @format double
   */
  sumTo?: number | null;
}

export interface ApplicationDetailsReadModel {
  /** @format uuid */
  id: string;
  title?: string | null;
  description?: string | null;
  /**
   * Дата с которой нужно получить результат
   * @format date-time
   */
  fromDate?: string | null;
  /**
   * Дата до которой нужно получить результат
   * @format date-time
   */
  toDate?: string | null;
  /**
   * Сумма минимальная за результат
   * @format double
   */
  sumFrom?: number | null;
  /**
   * Сумма максимальная за результат
   * @format double
   */
  sumTo?: number | null;
  /** Идентификатор профиля покупателя что создал заявку */
  customerId?: string | null;
  /** @format date-time */
  created: string;
  /** @format date-time */
  updated?: string | null;
  /** @format int64 */
  number: number;
  /**
   * Идентификатор заказа в котором продали эту заявку.
   * @format uuid
   */
  selectedOrderId?: string | null;
  /**
   * Идентификатор Отклика который продали для этой заявки
   * @format uuid
   */
  selectedOfferId?: string | null;
  offers?: OfferReadModel[] | null;
  customer: UserPorfileShortInfoModel;
}

export interface ApplicationReadModel {
  /** @format uuid */
  id: string;
  title?: string | null;
  description?: string | null;
  /**
   * Дата с которой нужно получить результат
   * @format date-time
   */
  fromDate?: string | null;
  /**
   * Дата до которой нужно получить результат
   * @format date-time
   */
  toDate?: string | null;
  /**
   * Сумма минимальная за результат
   * @format double
   */
  sumFrom?: number | null;
  /**
   * Сумма максимальная за результат
   * @format double
   */
  sumTo?: number | null;
  /** Идентификатор профиля покупателя что создал заявку */
  customerId?: string | null;
  /** @format date-time */
  created: string;
  /** @format date-time */
  updated?: string | null;
  /** @format int64 */
  number: number;
  /**
   * Идентификатор заказа в котором продали эту заявку.
   * @format uuid
   */
  selectedOrderId?: string | null;
  /**
   * Идентификатор Отклика который продали для этой заявки
   * @format uuid
   */
  selectedOfferId?: string | null;
}

export interface ApplicationReadModelPageResultModel {
  /** @format int32 */
  totalCount: number;
  items?: ApplicationReadModel[] | null;
}

export interface ApplictaionShorInfoModel {
  /** @format uuid */
  id: string;
  title?: string | null;
  /** @format int64 */
  number: number;
}

export interface AuthenticationSchemeReadModel {
  name?: string | null;
  displayName?: string | null;
}

export interface CalculateDeliveryPriceReadModel {
  /**
   * Цена доставки
   * @format double
   */
  price: number;
  /**
   * Дата и время начала интервала когда курьер сможет доставить заказ
   * @format date-time
   */
  startTime: string;
  /**
   * Дата и время конца интервала когда курьер сможет доставить заказ
   * @format date-time
   */
  finishTime: string;
}

export interface CategoryCreateModel {
  /**
   * @minLength 1
   * @maxLength 255
   * @pattern ^[a-zA-z а-яА-Я]*$
   */
  title: string;
  type: CategoryType;
  /** Показывать в фильтрах на главной странице */
  showOnMainPage: boolean;
}

export interface CategoryFilterModel {
  /**
   * Номер страницы (начиная с 1)
   * @format int32
   * @min 1
   * @max 4294967295
   */
  pageNumber: number;
  /**
   * Размер страницы (максимум 1000)
   * @format int32
   * @min 1
   * @max 1000
   */
  pageSize: number;
  title?: string | null;
  /** Скрывать категории без товаров */
  hideEmpty: boolean;
}

export interface CategoryReadModel {
  /**
   * @minLength 1
   * @maxLength 255
   * @pattern ^[a-zA-z а-яА-Я]*$
   */
  title: string;
  type: CategoryType;
  /** Показывать в фильтрах на главной странице */
  showOnMainPage: boolean;
  /** @format uuid */
  id: string;
}

export interface CategoryReadModelPageResultModel {
  /** @format int32 */
  totalCount: number;
  items?: CategoryReadModel[] | null;
}

export enum CategoryType {
  Kind = 'Kind',
  Composition = 'Composition',
  Additionally = 'Additionally',
}

export interface CategoryUpdateModel {
  /**
   * @minLength 1
   * @maxLength 255
   * @pattern ^[a-zA-z а-яА-Я]*$
   */
  title: string;
  type: CategoryType;
  /** Показывать в фильтрах на главной странице */
  showOnMainPage: boolean;
  /** @format uuid */
  id: string;
}

export interface CheckVerificationCodeModel {
  /** @minLength 1 */
  phone: string;
  /** @minLength 1 */
  code: string;
}

export interface CollectionCreateModel {
  /**
   * @minLength 0
   * @maxLength 255
   */
  title: string;
  categories?: string[] | null;
}

export interface CollectionReadModel {
  /** @format uuid */
  id: string;
  title?: string | null;
  categories?: CategoryReadModel[] | null;
}

export interface CollectionUpdateModel {
  /**
   * @minLength 0
   * @maxLength 255
   */
  title: string;
  categories?: string[] | null;
  /** @format uuid */
  id: string;
}

export interface ConfigModel {
  ssoUri?: string | null;
  apiUri?: string | null;
  isTesting: boolean;
  currency: CurrencyType;
  termOfServiceFilePath?: string | null;
  partnerTermOfServiceFilePath?: string | null;
  /** @format double */
  commissionPrecentageForMerch: number;
  /** Разрешение отправлять пользователю рассылки */
  consentToMailingsFilePath?: string | null;
  /** Политика конфеденциальности */
  privacyPolicyFilePath?: string | null;
  /** Согласие на обработку персональных данных */
  consentToPersonalDataProcessingFilePath?: string | null;
  offerForBuyerFilePath?: string | null;
  termsOfUserFilePath?: string | null;
}

export enum CurrencyType {
  Rub = 'Rub',
}

export enum DayOfWeek {
  Sunday = 'Sunday',
  Monday = 'Monday',
  Tuesday = 'Tuesday',
  Wednesday = 'Wednesday',
  Thursday = 'Thursday',
  Friday = 'Friday',
  Saturday = 'Saturday',
}

export interface DeliveryIndividualPriceModel {
  /** @format uuid */
  offerId: string;
  recipientAddress: AddressCreateModel;
}

export interface DeliveryInterval {
  /** @format date-time */
  requiredStartDatetime: string;
  /** @format date-time */
  requiredFinishDatetime: string;
}

export interface DeliveryNowPriceModel {
  recipientAddress: AddressCreateModel;
  /** Идетнификатор предложения на заявку (отклик на заявку на индивидуальный заказ */
  items: OrderItemModel[];
}

export interface DeliveryOrderModel {
  /** @format uuid */
  orderId: string;
}

export interface DeliveryOrderResponseModel {
  /**
   * Цена доставки
   * @format double
   */
  deliveryPrice: number;
  fromSeller: DeliveryInterval;
  sellerDeliveryTrackingUrl?: string | null;
  toBuyer: DeliveryInterval;
  buyerDeliveryTrackingUrl?: string | null;
}

export interface DeviceCreateModel {
  /**
   * @minLength 1
   * @maxLength 255
   */
  fcmPushToken: string;
}

export interface DeviceReadModel {
  /**
   * @minLength 1
   * @maxLength 255
   */
  fcmPushToken: string;
  /** @format uuid */
  id: string;
}

export interface DeviceReadModelPageResultModel {
  /** @format int32 */
  totalCount: number;
  items?: DeviceReadModel[] | null;
}

export interface DeviceUpdateModel {
  /**
   * @minLength 1
   * @maxLength 255
   */
  fcmPushToken: string;
  /** @format uuid */
  id: string;
}

export interface DostavistaCalculateOrderRequest {
  type: DostavistaOrderType;
  /** Что везем */
  matter?: string | null;
  /**
   * Общий вес отправления
   * @format int32
   */
  totalWeightKg: number;
  points?: DostavistaPoint[] | null;
}

export interface DostavistaContact {
  /** Номер контактного лица на точке */
  phone?: string | null;
  /** Имя контактного лица */
  name?: string | null;
}

export enum DostavistaOrderType {
  Standard = 'standard',
  SameDay = 'same_day',
}

export interface DostavistaPakage {
  /** Артикул товара. */
  wareCode?: string | null;
  /** Описание товара. Максимум 1000 символов. */
  description?: string | null;
  /** Количество товаров. */
  itemsCount?: string | null;
  /** Сумма оплаты за единицу товара. */
  itemPaymentAmount?: string | null;
  nomenclatureCode?: string | null;
}

export interface DostavistaPoint {
  /** Полный адрес в формате: город, улица, дом. Максимум 350 символов. */
  address?: string | null;
  contactPerson: DostavistaContact;
  /** Номер заказа */
  clientOrderId?: string | null;
  /** Дополнительная информация курьеру */
  note?: string | null;
  /** Номер здания */
  buildingNumber?: string | null;
  /** Подъезд */
  entranceNumber?: string | null;
  /** Код домофона */
  intercomCode?: string | null;
  /** Этаж */
  floorNumber?: string | null;
  /** Квартира или офис */
  apartmentNumber?: string | null;
  packages?: DostavistaPakage[] | null;
  /** @format date-time */
  requiredStartDatetime?: string | null;
  /** @format date-time */
  requiredFinishDatetime?: string | null;
}

export interface EmailUpdateModel {
  /**
   * @format email
   * @minLength 1
   */
  newEmail: string;
}

export interface EnumInfoModel {
  /** @format int32 */
  value: number;
  name?: string | null;
  description?: string | null;
}

/** Внутрення ошибка сервера */
export interface ErrorModel {
  /** Сообщения */
  message?: string | null;
  /** Идентификатор по которому можно в логах остследить что произошло на сервере */
  traceId?: string | null;
  /**
   * Время сервера по которому можно отстелидить в логах что происходило на сервере
   * @format date-time
   */
  serverTime: string;
}

export interface FileReadModel {
  /**
   * Название
   * @minLength 2
   * @maxLength 255
   */
  title: string;
  /** @format uuid */
  id: string;
  /** @format byte */
  hash?: string | null;
  /** @format int64 */
  size: number;
  contentType?: string | null;
  /** @format date-time */
  created: string;
  /** @format date-time */
  updated?: string | null;
  concurrencyToken?: string | null;
  path?: string | null;
}

export interface FileReadModelPageResultModel {
  /** @format int32 */
  totalCount: number;
  items?: FileReadModel[] | null;
}

export interface FilterMerchandiseByPartnerDetailsModel {
  /**
   * Номер страницы (начиная с 1)
   * @format int32
   * @min 1
   * @max 4294967295
   */
  pageNumber: number;
  /**
   * Размер страницы (максимум 1000)
   * @format int32
   * @min 1
   * @max 1000
   */
  pageSize: number;
  /** @format uuid */
  partnerId: string;
}

export interface FilterMerchandiseByPartnerForPartnerDetailsModel {
  /**
   * Номер страницы (начиная с 1)
   * @format int32
   * @min 1
   * @max 4294967295
   */
  pageNumber: number;
  /**
   * Размер страницы (максимум 1000)
   * @format int32
   * @min 1
   * @max 1000
   */
  pageSize: number;
  /** @format uuid */
  partnerId: string;
  /** Показать товары которые продавец скрыл сам. */
  showHidden?: boolean | null;
  /** Показать товары которые закончились на складе (которые распродали уже) */
  showOutOfStock?: boolean | null;
  /** Показать товары которые сейчас на модерации у Администратора */
  showOnModeration?: boolean | null;
  /** Показать те товары к которым есть замечания от админа (заблокированные) */
  showBlockedByAdmin?: boolean | null;
}

export interface FilterMerchandiseDetailsModel {
  /**
   * Номер страницы (начиная с 1)
   * @format int32
   * @min 1
   * @max 4294967295
   */
  pageNumber: number;
  /**
   * Размер страницы (максимум 1000)
   * @format int32
   * @min 1
   * @max 1000
   */
  pageSize: number;
  /** Поисковой запрос. Ищет товары по названию или ФИО продавца или Логину продавца */
  findingQuery?: string | null;
  /** Список категорий (тегов) по которым нужно отфильтровать товар */
  categories?: Record<string, string[] | null>;
  /** По каким свойствам сортировать */
  orderings?: MerchOrderingPropsOrderingModel[] | null;
  /**
   * Коллекция товаров которую нужно показать.
   * @format uuid
   */
  collectionId?: string | null;
}

export interface FilterMerchandiseForAdminDetailsModel {
  /**
   * Номер страницы (начиная с 1)
   * @format int32
   * @min 1
   * @max 4294967295
   */
  pageNumber: number;
  /**
   * Размер страницы (максимум 1000)
   * @format int32
   * @min 1
   * @max 1000
   */
  pageSize: number;
  isTagsAppovedByAdmin?: boolean | null;
  /** Товары первый раз на модерацииь */
  isNew?: boolean | null;
}

export interface FiterBuyedApplicationsModel {
  /**
   * Номер страницы (начиная с 1)
   * @format int32
   * @min 1
   * @max 4294967295
   */
  pageNumber: number;
  /**
   * Размер страницы (максимум 1000)
   * @format int32
   * @min 1
   * @max 1000
   */
  pageSize: number;
  /** @format uuid */
  partnerId?: string | null;
}

export interface GlobaSettingsModel {
  /**
   * Минимальная сумма доставки
   * @format double
   * @min 0
   * @max 1000000
   */
  minDeliveryPrice: number;
  /**
   * Коэффициент комисси для доставки в процентах
   * @format double
   * @min 0
   * @max 100
   */
  deliveryCoefficient: number;
  /**
   * Минимальная сумма товаров
   * @format double
   * @min 0
   * @max 1000000
   */
  minMerchPrice: number;
  /**
   * Коэффициент коммисси для суммы товаров в процентах
   * @format double
   * @min 0
   * @max 100
   */
  merchCoefficient: number;
}

export interface GuidPageResultModel {
  /** @format int32 */
  totalCount: number;
  items?: string[] | null;
}

export interface IdentityRole {
  id?: string | null;
  name?: string | null;
  normalizedName?: string | null;
  concurrencyStamp?: string | null;
}

export interface LendingMessageModel {
  type: LendingMessageType;
  name?: string | null;
  /**
   * @format email
   * @minLength 1
   */
  email: string;
  message?: string | null;
  files?: File[] | null;
}

export enum LendingMessageType {
  Client = 'Client',
  Cook = 'Cook',
}

export interface LoginRemoveModel {
  /** @minLength 1 */
  loginProvider: string;
  /** @minLength 1 */
  providerKey: string;
}

export enum MerchOrderingProps {
  ByMerhRating = 'ByMerhRating',
  ByOrdersCount = 'ByOrdersCount',
  ByPartnerRating = 'ByPartnerRating',
}

export interface MerchOrderingPropsOrderingModel {
  /** Сортировать по возрастанию (Если false то по уменшению будет) */
  asc: boolean;
  by: MerchOrderingProps;
}

export interface MerchRatingCreateModel {
  comment?: string | null;
  /**
   * @format int32
   * @min 1
   * @max 5
   */
  rating: number;
  /** @format uuid */
  merchId: string;
}

export interface MerchRatingFilterModel {
  /**
   * Номер страницы (начиная с 1)
   * @format int32
   * @min 1
   * @max 4294967295
   */
  pageNumber: number;
  /**
   * Размер страницы (максимум 1000)
   * @format int32
   * @min 1
   * @max 1000
   */
  pageSize: number;
  /** @format uuid */
  merchId?: string | null;
}

export interface MerchRatingReadModel {
  comment?: string | null;
  /**
   * @format int32
   * @min 1
   * @max 5
   */
  rating: number;
  /** @format uuid */
  merchId: string;
  /** @format uuid */
  id: string;
  /** @format date-time */
  created: string;
  evaluatorId?: string | null;
}

export interface MerchRatingReadModelPageResultModel {
  /** @format int32 */
  totalCount: number;
  items?: MerchRatingReadModel[] | null;
}

export interface MerchandiseCreateModel {
  /**
   * Название
   * @minLength 2
   * @maxLength 255
   */
  title: string;
  /** Описание */
  description?: string | null;
  /** Еденицы измерения товаров */
  unitType: MerchandiseUnitType;
  /**
   * Цена за еденицу измерения
   * @format double
   * @min 0
   * @max 1000000
   */
  price: number;
  /** Фотки товара. Первая фотка в списке будет назначена главной. Сами фотки в /api/v1/files */
  images: string[];
  /**
   * Размер порции
   * @format double
   */
  servingSize: number;
  /** Категории (теги) товара */
  categories: string[];
  /**
   * Доступное количество товара в наличии.
   * @format double
   */
  availableQuantity: number;
  /**
   * Брутто вес порции в колограммах (вместе с упаковкой, одноразовой посудой и т. д.)
   * Нужно для курьера чтобы он знал сколько килограммнов ему тащить за раз.
   * @format double
   * @min 0.001
   * @max 1000
   */
  servingGrossWeightInKilograms: number;
}

export interface MerchandiseReadModel {
  /**
   * Название
   * @minLength 2
   * @maxLength 255
   */
  title: string;
  /** Описание */
  description?: string | null;
  /** @format uuid */
  id: string;
  /** Еденицы измерения товаров */
  unitType: MerchandiseUnitType;
  /**
   * Цена за еденицу измерения
   * @format double
   */
  price: number;
  currencyType: CurrencyType;
  /** Фотки товара. Первая фотка в списке будет назначена главной. Сами фотки в /api/v1/files */
  images?: FileReadModel[] | null;
  seller: PartnerReadModel;
  /** @format date-time */
  created: string;
  /** @format date-time */
  updated?: string | null;
  state: MerchandisesState;
  /**
   * Размер порции. (0.1  кг или 1 штука)
   * @format double
   */
  servingSize: number;
  /** Категории (теги) к которым относиться этот товар */
  categories?: CategoryReadModel[] | null;
  /**
   * Доступное количество товара в наличии.
   * @format double
   */
  availableQuantity: number;
  /**
   * Брутто вес порции в колограммах (вместе с упаковкой, одноразовой посудой и т. д.)
   * Нужно для курьера чтобы он знал сколько килограммнов ему тащить за раз.
   * @format double
   * @min 0.001
   * @max 1000
   */
  servingGrossWeightInKilograms: number;
  /**
   * Количество звезд (до 5)
   * @format double
   */
  rating?: number | null;
  /** Были ли теги этого товара подтверждены админом. */
  isTagsAppovedByAdmin: boolean;
  /** Пользователи запросивщие состав */
  compositionRequesters?: string[] | null;
  /** Комментарий почему товар заблокирован и что нужно сделать чтобы его разблокировать (картинку сменить например). */
  reasonForBlocking?: string | null;
}

export interface MerchandiseReadModelPageResultModel {
  /** @format int32 */
  totalCount: number;
  items?: MerchandiseReadModel[] | null;
}

/** Еденицы измерения товаров */
export enum MerchandiseUnitType {
  Pieces = 'Pieces',
  Kilograms = 'Kilograms',
  Liters = 'Liters',
}

export interface MerchandiseUpdateModel {
  /**
   * Название
   * @minLength 2
   * @maxLength 255
   */
  title: string;
  /** Описание */
  description?: string | null;
  /** Еденицы измерения товаров */
  unitType: MerchandiseUnitType;
  /**
   * Цена за еденицу измерения
   * @format double
   * @min 0
   * @max 1000000
   */
  price: number;
  /** Фотки товара. Первая фотка в списке будет назначена главной. Сами фотки в /api/v1/files */
  images: string[];
  /**
   * Размер порции
   * @format double
   */
  servingSize: number;
  /** Категории (теги) товара */
  categories: string[];
  /**
   * Доступное количество товара в наличии.
   * @format double
   */
  availableQuantity: number;
  /**
   * Брутто вес порции в колограммах (вместе с упаковкой, одноразовой посудой и т. д.)
   * Нужно для курьера чтобы он знал сколько килограммнов ему тащить за раз.
   * @format double
   * @min 0.001
   * @max 1000
   */
  servingGrossWeightInKilograms: number;
  /** @format uuid */
  id: string;
  state: MerchandisesState;
}

export enum MerchandisesState {
  Created = 'Created',
  Published = 'Published',
  Blocked = 'Blocked',
}

/** Коментарий к предложению */
export interface OfferCommentCreateModel {
  /**
   * Текст коментария
   * @minLength 1
   */
  text: string;
  /**
   * Идентификатор предложения на заявку к которой оставлен коментарий
   * @format uuid
   */
  offerId: string;
  /**
   * Коментарий на который является ответом данный комментарий. Как в Телеграм где можно на сообщений отвечать.
   * @format uuid
   */
  parentId?: string | null;
}

export interface OfferCommentReadModel {
  /** @format uuid */
  id: string;
  text?: string | null;
  /**
   * Идентификатор оффера на заявку к котором относиться коментарий
   * @format uuid
   */
  offerId: string;
  /** Идентификатор профиля пользователя что оставил комментарий. */
  userProfileId?: string | null;
  userProfile: UserPorfileShortInfoModel;
  /**
   * Идентификатор родительского коментария (на какой коментарий этот является ответом как в Телеграм)
   * @format uuid
   */
  parentId?: string | null;
  /**
   * Дата создания коментария
   * @format date-time
   */
  created: string;
}

export interface OfferCreateModel {
  /**
   * Идентификатор заявки на которую сделан оффер
   * @format uuid
   */
  applicationId: string;
  /**
   * Дата к которой будет готов заказ
   * @format date-time
   */
  date: string;
  /**
   * Сумма заказа
   * @format double
   */
  sum: number;
  /** Описание оффера. */
  description?: string | null;
  /**
   * Брутто вес в колограммах (вместе с упаковкой, одноразовой посудой и т. д.)
   * Нужно для курьера чтобы он знал сколько килограммнов ему тащить за раз.
   * @format double
   * @min 0.001
   * @max 1000
   */
  servingGrossWeightInKilograms: number;
  images?: string[] | null;
}

export interface OfferDetailsReamModel {
  /** @format uuid */
  id: string;
  /**
   * Идентификатор заявки на которую сделан оффер
   * @format uuid
   */
  applicationId: string;
  /**
   * Дата к которой будет готов заказ
   * @format date-time
   */
  date: string;
  /**
   * Сумма заказа
   * @format double
   */
  sum: number;
  /** Описание оффера. */
  description?: string | null;
  /**
   * Идентификатор партнера (магазина) что сделал оффер
   * @format uuid
   */
  partnerId: string;
  /** Идентификатор профиля пользователя (сотрудника магазина) что сделал оффер */
  creatorId?: string | null;
  /** @format int64 */
  number: number;
  seller: SellerShortInfoReadModel;
  /**
   * Брутто вес в колограммах (вместе с упаковкой, одноразовой посудой и т. д.)
   * Нужно для курьера чтобы он знал сколько килограммнов ему тащить за раз.
   * @format double
   * @min 0.001
   * @max 1000
   */
  servingGrossWeightInKilograms: number;
  /**
   * Идентификатор заказа в котором продали эту заявку.
   * @format uuid
   */
  selectedOrderId?: string | null;
  /** Коментарии к отклику на заявку */
  comments?: OfferCommentReadModel[] | null;
  /** Фотки примеров похожих работ что этот кондитер раньше делал. */
  images?: FileReadModel[] | null;
}

export interface OfferReadModel {
  /** @format uuid */
  id: string;
  /**
   * Идентификатор заявки на которую сделан оффер
   * @format uuid
   */
  applicationId: string;
  /**
   * Дата к которой будет готов заказ
   * @format date-time
   */
  date: string;
  /**
   * Сумма заказа
   * @format double
   */
  sum: number;
  /** Описание оффера. */
  description?: string | null;
  /**
   * Идентификатор партнера (магазина) что сделал оффер
   * @format uuid
   */
  partnerId: string;
  /** Идентификатор профиля пользователя (сотрудника магазина) что сделал оффер */
  creatorId?: string | null;
  /** @format int64 */
  number: number;
  seller: SellerShortInfoReadModel;
  /**
   * Брутто вес в колограммах (вместе с упаковкой, одноразовой посудой и т. д.)
   * Нужно для курьера чтобы он знал сколько килограммнов ему тащить за раз.
   * @format double
   * @min 0.001
   * @max 1000
   */
  servingGrossWeightInKilograms: number;
  images?: FileReadModel[] | null;
  /**
   * Идентификатор заказа в котором продали эту заявку.
   * @format uuid
   */
  selectedOrderId?: string | null;
}

export interface OfferSellerReadModel {
  /** @format uuid */
  id: string;
  /**
   * Идентификатор заявки на которую сделан оффер
   * @format uuid
   */
  applicationId: string;
  /**
   * Дата к которой будет готов заказ
   * @format date-time
   */
  date: string;
  /**
   * Сумма заказа
   * @format double
   */
  sum: number;
  /** Описание оффера. */
  description?: string | null;
  /**
   * Идентификатор партнера (магазина) что сделал оффер
   * @format uuid
   */
  partnerId: string;
  /** Идентификатор профиля пользователя (сотрудника магазина) что сделал оффер */
  creatorId?: string | null;
  /** @format int64 */
  number: number;
  seller: SellerShortInfoReadModel;
  /**
   * Брутто вес в колограммах (вместе с упаковкой, одноразовой посудой и т. д.)
   * Нужно для курьера чтобы он знал сколько килограммнов ему тащить за раз.
   * @format double
   * @min 0.001
   * @max 1000
   */
  servingGrossWeightInKilograms: number;
  images?: FileReadModel[] | null;
  /**
   * Идентификатор заказа в котором продали эту заявку.
   * @format uuid
   */
  selectedOrderId?: string | null;
  applictaion: ApplictaionShorInfoModel;
}

export interface OfferSellerReadModelPageResultModel {
  /** @format int32 */
  totalCount: number;
  items?: OfferSellerReadModel[] | null;
}

export interface OrderCreateModel {
  /** ФИО получателя */
  recipientFullName?: string | null;
  /**
   * Телефонный номер получателя
   * @format tel
   * @minLength 1
   */
  recipientPhone: string;
  /** Коментарии к заказу. Как проехать и т. п. */
  comments?: string | null;
  recipientAddress: AddressCreateModel;
  /** Идетнификатор предложения на заявку (отклик на заявку на индивидуальный заказ */
  items: OrderItemModel[];
}

export enum OrderDeliveryType {
  Now = 'Now',
  SelfDelivered = 'SelfDelivered',
}

/** Создание индивидуального заказа */
export interface OrderIndividualCreateModel {
  /** ФИО получателя */
  recipientFullName?: string | null;
  /**
   * Телефонный номер получателя
   * @format tel
   * @minLength 1
   */
  recipientPhone: string;
  /** Коментарии к заказу. Как проехать и т. п. */
  comments?: string | null;
  recipientAddress: AddressCreateModel;
  /**
   * Идетнификатор предложения на заявку (отклик на заявку на индивидуальный заказ
   * @format uuid
   */
  offerId: string;
}

/** Создание индивидуального заказа c самодоставкой (приехать в магазин за товаром) */
export interface OrderIndividualSelfDeliveredCreateModel {
  /** ФИО получателя */
  recipientFullName?: string | null;
  /**
   * Телефонный номер получателя
   * @format tel
   * @minLength 1
   */
  recipientPhone: string;
  /** Коментарии к заказу. Как проехать и т. п. */
  comments?: string | null;
  /**
   * Идетнификатор предложения на заявку (отклик на заявку на индивидуальный заказ
   * @format uuid
   */
  offerId: string;
}

export interface OrderItemModel {
  /** @format uuid */
  itemId: string;
  /**
   * @format int32
   * @min 0
   * @max 2147483647
   */
  amount: number;
}

export interface OrderItemReadModel {
  /** @format uuid */
  id: string;
  /** @format date-time */
  created: string;
  /** @format date-time */
  updated?: string | null;
  /**
   * Id заказа
   * @format uuid
   */
  orderId: string;
  /**
   * Id товара
   * @format uuid
   */
  itemId?: string | null;
  /** @format int32 */
  amount: number;
  type: OrderItemType;
  /**
   * Id заявки
   * @format uuid
   */
  offerId?: string | null;
  /** Еденицы измерения товаров */
  unitTypeType: MerchandiseUnitType;
  /**
   * Цена за еденицу измерения
   * @format double
   */
  price?: number | null;
  currencyType: CurrencyType;
  state: MerchandisesState;
  /**
   * Размер порции. (0.1  кг или 1 штука)
   * @format double
   */
  servingSize?: number | null;
  /**
   * Партнеры которым принадлежит этот товар
   * @format uuid
   */
  partnerId?: string | null;
  title?: string | null;
  normalizedTitle?: string | null;
  description?: string | null;
  /**
   * Брутто вес порции в колограммах (вместе с упаковкой, одноразовой посудой и т. д.)
   * Нужно для курьера чтобы он знал сколько килограммнов ему тащить за раз.
   * @format double
   */
  servingGrossWeightInKilograms: number;
}

export enum OrderItemType {
  Standard = 'Standard',
  Individual = 'Individual',
}

export interface OrderReadModel {
  /** @format uuid */
  id: string;
  buyer: UserProfileReadModel;
  seller: SellerShortInfoReadModel;
  state: OrderState;
  recipientPhone?: string | null;
  comments?: string | null;
  recipientAddress: AddressReadModel;
  items?: OrderItemReadModel[] | null;
  /** @format date-time */
  created: string;
  /** @format date-time */
  updated?: string | null;
  /**
   * Дата когда заказ был оплачен
   * @format date-time
   */
  paymentDate?: string | null;
  /**
   * Дата когда заказ был доставлен
   * @format date-time
   */
  deliveredDate?: string | null;
  /**
   * Сумма заказа
   * @format double
   */
  amount: number;
  deliveryType: OrderDeliveryType;
  /** @format int64 */
  number: number;
  /** Тип заказа */
  type: OrderType;
  recipientFullName?: string | null;
  deliveryInfo: DeliveryOrderResponseModel;
}

export interface OrderReadModelPageResultModel {
  /** @format int32 */
  totalCount: number;
  items?: OrderReadModel[] | null;
}

export interface OrderSerfDeliveredCreateModel {
  recipientFullName?: string | null;
  /**
   * @format tel
   * @minLength 1
   */
  recipientPhone: string;
  comments?: string | null;
  items: OrderItemModel[];
}

export enum OrderState {
  Created = 'Created',
  Paid = 'Paid',
  Delivered = 'Delivered',
  Delivering = 'Delivering',
}

/** Тип заказа */
export enum OrderType {
  Standard = 'Standard',
  Individual = 'Individual',
}

export interface PaginationModel {
  /**
   * Номер страницы (начиная с 1)
   * @format int32
   * @min 1
   * @max 4294967295
   */
  pageNumber: number;
  /**
   * Размер страницы (максимум 1000)
   * @format int32
   * @min 1
   * @max 1000
   */
  pageSize: number;
}

export interface PartnerCreateModel {
  /**
   * Краткое наименование компании или ФИО ИП/Самозанятого
   * @minLength 0
   * @maxLength 255
   */
  title: string;
  /**
   * ИНН
   * @minLength 1
   */
  inn: string;
  /**
   * Контактный телефон
   * @minLength 1
   */
  contactPhone: string;
  /**
   * Контактный email
   * @minLength 1
   */
  contactEmail: string;
  type: PartnerType;
  /**
   * Код подтверждения из СМС
   * @minLength 1
   */
  phoneComfinmationCode: string;
  /** Принял ли партнер условия лицензионного соглащения */
  acceptedTermsOfService: boolean;
  registrationAddress: AddressCreateModel;
}

export interface PartnerRatingCreateModel {
  comment?: string | null;
  /**
   * @format int32
   * @min 1
   * @max 5
   */
  rating: number;
  /** @format uuid */
  partnerId: string;
}

export interface PartnerRatingFilterModel {
  /**
   * Номер страницы (начиная с 1)
   * @format int32
   * @min 1
   * @max 4294967295
   */
  pageNumber: number;
  /**
   * Размер страницы (максимум 1000)
   * @format int32
   * @min 1
   * @max 1000
   */
  pageSize: number;
  /** @format uuid */
  partnerId?: string | null;
}

export interface PartnerRatingReadModel {
  comment?: string | null;
  /**
   * @format int32
   * @min 1
   * @max 5
   */
  rating: number;
  /** @format uuid */
  partnerId: string;
  /** @format uuid */
  id: string;
  /** @format date-time */
  created: string;
  evaluatorId?: string | null;
}

export interface PartnerRatingReadModelPageResultModel {
  /** @format int32 */
  totalCount: number;
  items?: PartnerRatingReadModel[] | null;
}

export interface PartnerReadModel {
  /** @format uuid */
  id: string;
  image: FileReadModel;
  /** Краткое название компании или ФИО ИП/Самозанятого */
  title?: string | null;
  /** Описание компании. */
  description?: string | null;
  state: PartnerState;
  type: PartnerType;
  address: AddressReadModel;
  workingTime: PeriodModel;
  /**
   * ИНН
   * @minLength 1
   */
  inn: string;
  /** Контактный телефон */
  contactPhone?: string | null;
  /**
   * Контактный email
   * @minLength 1
   */
  contactEmail: string;
  /** Дни по которым работает этот магазин */
  workingDays?: DayOfWeek[] | null;
  /** Идентификатор в платежной системе (в YKassa сейчас) */
  externalId?: string | null;
  /**
   * Количество звезд (до 5)
   * @format double
   */
  rating?: number | null;
  /** Есть ли самовывоз? */
  isPickupEnabled: boolean;
  registrationAddress: AddressReadModel;
}

export enum PartnerState {
  Created = 'Created',
  Confirmed = 'Confirmed',
  Blocked = 'Blocked',
}

export enum PartnerType {
  SelfEmployed = 'SelfEmployed',
  IndividualEntrepreneur = 'IndividualEntrepreneur',
  Company = 'Company',
}

export interface PartnerUpdateModel {
  /** @format uuid */
  id: string;
  /**
   * Краткое название компании или ФИО ИП/Самозанятого
   * @minLength 0
   * @maxLength 100
   */
  title: string;
  /** Описание компании */
  description?: string | null;
  /**
   * Логотип или фото самозанятого (из FileDto)
   * @format uuid
   */
  imageId?: string | null;
  workingTime: PeriodModel;
  /**
   * @maxItems 7
   * @minItems 1
   */
  workingDays: DayOfWeek[];
  address: AddressCreateModel;
  /**
   * ИНН
   * @minLength 1
   */
  inn: string;
  /**
   * Контактный телефон
   * @minLength 1
   */
  contactPhone: string;
  /**
   * Контактный email
   * @format email
   * @minLength 1
   */
  contactEmail: string;
  /** Код подтверждения из СМС. Нужен только если меняется номер телефона */
  phoneComfinmationCode?: string | null;
  registrationAddress: AddressCreateModel;
  /** Есть ли самовывоз? */
  isPickupEnabled: boolean;
}

export interface PasswordCreateModel {
  /**
   * @format password
   * @minLength 6
   * @maxLength 100
   */
  newPassword: string;
  /** @format password */
  confirmPassword?: string | null;
}

export interface PasswordUpdateModel {
  /**
   * @format password
   * @minLength 6
   * @maxLength 100
   */
  newPassword: string;
  /** @format password */
  confirmPassword?: string | null;
  /**
   * @format password
   * @minLength 1
   */
  oldPassword: string;
}

export interface PeriodModel {
  /** @format date-time */
  start: string;
  /** @format date-time */
  end: string;
}

export interface RatingUpdateModel {
  comment?: string | null;
  /**
   * @format int32
   * @min 1
   * @max 5
   */
  rating: number;
  /** @format uuid */
  id: string;
}

export interface SellerShortInfoReadModel {
  /** @format uuid */
  id: string;
  title?: string | null;
}

export interface SendVerificationSmsModel {
  /** @minLength 1 */
  phone: string;
}

/** Модель для создания профиля фрилансера или магазина */
export interface ShopCreateModel {
  /**
   * Краткое наименование магазина или фрилансера
   * @minLength 0
   * @maxLength 255
   */
  title: string;
  /** Описание магазина или фрилансера */
  description?: string | null;
  type: ShopType;
  address: AddressCreateModel;
  /**
   * Картинка магазина или фрилансера
   * @format uuid
   */
  imageId: string;
  /**
   * Партнер которому принадлежит этот магазин или этот профиль фрилансера.
   * @format uuid
   */
  parnerId: string;
}

export enum ShopType {
  Shop = 'Shop',
  Frilancer = 'Frilancer',
}

export interface TestRng {
  /** @format int32 */
  from: number;
  /** @format int32 */
  to: number;
  names?: number[] | null;
}

export interface UserLoginInfo {
  loginProvider?: string | null;
  providerKey?: string | null;
  providerDisplayName?: string | null;
}

export interface UserPorfileShortInfoModel {
  userName?: string | null;
  id?: string | null;
}

export interface UserProfileDetailsReadModel {
  id?: string | null;
  userName?: string | null;
  email?: string | null;
  emailConfirmed: boolean;
  lockoutEnabled: boolean;
  /** @format date-time */
  lockoutEnd?: string | null;
  /** @format int32 */
  accessFailedCount: number;
  securityStamp?: string | null;
  concurrencyStamp?: string | null;
  roles?: string[] | null;
  /** Текущие способы входа которые прикреплены к текущему пользователю (Googel, Facebook) */
  logins?: UserLoginInfo[] | null;
  hasPassword: boolean;
  /** Доступные способы входа которые можно прекркпить к текущему пользователю (Googel, Facebook) */
  otherLogins?: AuthenticationSchemeReadModel[] | null;
  showRemoveLoginButton: boolean;
  /** Пользователь принял согласие на рассылку */
  acceptedConsentToMailings: boolean;
  /** Пользователь согласился на обработку его персональных данных */
  acceptedConsentToPersonalDataProcessing: boolean;
  /** Пользователь принял политику конфеденциальности */
  acceptedPivacyPolicy: boolean;
  /** Пользователь принял условия использования */
  acceptedTermsOfUse: boolean;
  /** Пользователь принял оферту для физических лиц */
  acceptedOfferFoUser: boolean;
}

export interface UserProfileFilterModel {
  /**
   * Номер страницы (начиная с 1)
   * @format int32
   * @min 1
   * @max 4294967295
   */
  pageNumber: number;
  /**
   * Размер страницы (максимум 1000)
   * @format int32
   * @min 1
   * @max 1000
   */
  pageSize: number;
}

export interface UserProfileReadModel {
  id?: string | null;
  userName?: string | null;
}

export interface UserProfileReadModelPageResultModel {
  /** @format int32 */
  totalCount: number;
  items?: UserProfileReadModel[] | null;
}

export interface UserProfileUpdateModel {
  /**
   * @minLength 1
   * @maxLength 100
   * @pattern ^[^\s]*$
   */
  userName: string;
  /** Пользователь принял согласие на рассылку */
  acceptedConsentToMailings: boolean;
  /** Пользователь согласился на обработку его персональных данных */
  acceptedConsentToPersonalDataProcessing: boolean;
  /** Пользователь принял политику конфеденциальности */
  acceptedPivacyPolicy: boolean;
  /** Пользователь принял условия использования */
  acceptedTermsOfUse: boolean;
  /** Пользователь принял оферту для физических лиц */
  acceptedOfferFoUser: boolean;
}

export enum VerivicationStatus {
  Pending = 'Pending',
  Approved = 'Approved',
  Canceled = 'Canceled',
}

export interface WeatherForecast {
  /** @format date-time */
  date: string;
  /** @format int32 */
  temperatureC: number;
  /** @format int32 */
  temperatureF: number;
  /** @minLength 1 */
  summary: string;
}

export type QueryParamsType = Record<string | number, any>;
export type ResponseFormat = keyof Omit<Body, 'body' | 'bodyUsed'>;

export interface FullRequestParams extends Omit<RequestInit, 'body'> {
  /** set parameter to `true` for call `securityWorker` for this request */
  secure?: boolean;
  /** request path */
  path: string;
  /** content type of request body */
  type?: ContentType;
  /** query params */
  query?: QueryParamsType;
  /** format of response (i.e. response.json() -> format: "json") */
  format?: ResponseFormat;
  /** request body */
  body?: unknown;
  /** base url */
  baseUrl?: string;
  /** request cancellation token */
  cancelToken?: CancelToken;
}

export type RequestParams = Omit<FullRequestParams, 'body' | 'method' | 'query' | 'path'>;

export interface ApiConfig<SecurityDataType = unknown> {
  baseUrl?: string;
  baseApiParams?: Omit<RequestParams, 'baseUrl' | 'cancelToken' | 'signal'>;
  securityWorker?: (securityData: SecurityDataType | null) => Promise<RequestParams | void> | RequestParams | void;
  customFetch?: typeof fetch;
}

export interface HttpResponse<D extends unknown, E extends unknown = unknown> extends Response {
  data: D;
  error: E;
}

type CancelToken = Symbol | string | number;

export enum ContentType {
  Json = 'application/json',
  FormData = 'multipart/form-data',
  UrlEncoded = 'application/x-www-form-urlencoded',
  Text = 'text/plain',
}

export class HttpClient<SecurityDataType = unknown> {
  public baseUrl: string = '';
  private securityData: SecurityDataType | null = null;
  private securityWorker?: ApiConfig<SecurityDataType>['securityWorker'];
  private abortControllers = new Map<CancelToken, AbortController>();
  private customFetch = (...fetchParams: Parameters<typeof fetch>) => fetch(...fetchParams);

  private baseApiParams: RequestParams = {
    credentials: 'same-origin',
    headers: {},
    redirect: 'follow',
    referrerPolicy: 'no-referrer',
  };

  constructor(apiConfig: ApiConfig<SecurityDataType> = {}) {
    Object.assign(this, apiConfig);
  }

  public setSecurityData = (data: SecurityDataType | null) => {
    this.securityData = data;
  };

  protected encodeQueryParam(key: string, value: any) {
    const encodedKey = encodeURIComponent(key);
    return `${encodedKey}=${encodeURIComponent(typeof value === 'number' ? value : `${value}`)}`;
  }

  protected addQueryParam(query: QueryParamsType, key: string) {
    return this.encodeQueryParam(key, query[key]);
  }

  protected addArrayQueryParam(query: QueryParamsType, key: string) {
    const value = query[key];
    return value.map((v: any) => this.encodeQueryParam(key, v)).join('&');
  }

  protected toQueryString(rawQuery?: QueryParamsType): string {
    const query = rawQuery || {};
    const keys = Object.keys(query).filter((key) => 'undefined' !== typeof query[key]);
    return keys
      .map((key) => (Array.isArray(query[key]) ? this.addArrayQueryParam(query, key) : this.addQueryParam(query, key)))
      .join('&');
  }

  protected addQueryParams(rawQuery?: QueryParamsType): string {
    const queryString = this.toQueryString(rawQuery);
    return queryString ? `?${queryString}` : '';
  }

  private contentFormatters: Record<ContentType, (input: any) => any> = {
    [ContentType.Json]: (input: any) =>
      input !== null && (typeof input === 'object' || typeof input === 'string') ? JSON.stringify(input) : input,
    [ContentType.Text]: (input: any) => (input !== null && typeof input !== 'string' ? JSON.stringify(input) : input),
    [ContentType.FormData]: (input: any) =>
      Object.keys(input || {}).reduce((formData, key) => {
        const property = input[key];
        formData.append(
          key,
          property instanceof Blob
            ? property
            : typeof property === 'object' && property !== null
            ? JSON.stringify(property)
            : `${property}`,
        );
        return formData;
      }, new FormData()),
    [ContentType.UrlEncoded]: (input: any) => this.toQueryString(input),
  };

  protected mergeRequestParams(params1: RequestParams, params2?: RequestParams): RequestParams {
    return {
      ...this.baseApiParams,
      ...params1,
      ...(params2 || {}),
      headers: {
        ...(this.baseApiParams.headers || {}),
        ...(params1.headers || {}),
        ...((params2 && params2.headers) || {}),
      },
    };
  }

  protected createAbortSignal = (cancelToken: CancelToken): AbortSignal | undefined => {
    if (this.abortControllers.has(cancelToken)) {
      const abortController = this.abortControllers.get(cancelToken);
      if (abortController) {
        return abortController.signal;
      }
      return void 0;
    }

    const abortController = new AbortController();
    this.abortControllers.set(cancelToken, abortController);
    return abortController.signal;
  };

  public abortRequest = (cancelToken: CancelToken) => {
    const abortController = this.abortControllers.get(cancelToken);

    if (abortController) {
      abortController.abort();
      this.abortControllers.delete(cancelToken);
    }
  };

  public request = async <T = any, E = any>({
    body,
    secure,
    path,
    type,
    query,
    format,
    baseUrl,
    cancelToken,
    ...params
  }: FullRequestParams): Promise<HttpResponse<T, E>> => {
    const secureParams =
      ((typeof secure === 'boolean' ? secure : this.baseApiParams.secure) &&
        this.securityWorker &&
        (await this.securityWorker(this.securityData))) ||
      {};
    const requestParams = this.mergeRequestParams(params, secureParams);
    const queryString = query && this.toQueryString(query);
    const payloadFormatter = this.contentFormatters[type || ContentType.Json];
    const responseFormat = format || requestParams.format;

    return this.customFetch(`${baseUrl || this.baseUrl || ''}${path}${queryString ? `?${queryString}` : ''}`, {
      ...requestParams,
      headers: {
        ...(requestParams.headers || {}),
        ...(type && type !== ContentType.FormData ? { 'Content-Type': type } : {}),
      },
      signal: cancelToken ? this.createAbortSignal(cancelToken) : requestParams.signal,
      body: typeof body === 'undefined' || body === null ? null : payloadFormatter(body),
    }).then(async (response) => {
      const r = response as HttpResponse<T, E>;
      r.data = null as unknown as T;
      r.error = null as unknown as E;

      const data = !responseFormat
        ? r
        : await response[responseFormat]()
            .then((data) => {
              if (r.ok) {
                r.data = data;
              } else {
                r.error = data;
              }
              return r;
            })
            .catch((e) => {
              r.error = e;
              return r;
            });

      if (cancelToken) {
        this.abortControllers.delete(cancelToken);
      }

      if (!response.ok) throw data;
      return data;
    });
  };
}

/**
 * @title Hike
 * @version 6.0.0.0
 *
 * Dish&Fork api. Health check available on path /health-check. SignalR hub on path /real-time-hub
 */
export class Api<SecurityDataType extends unknown> extends HttpClient<SecurityDataType> {
  api = {
    /**
     * No description
     *
     * @tags Categories
     * @name V1AdminCategoriesCreate
     * @request POST:/api/v1/admin/categories
     * @secure
     */
    v1AdminCategoriesCreate: (data: CategoryCreateModel, params: RequestParams = {}) =>
      this.request<string, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/admin/categories`,
        method: 'POST',
        body: data,
        secure: true,
        type: ContentType.Json,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Categories
     * @name V1AdminCategoriesUpdate
     * @request PUT:/api/v1/admin/categories
     * @secure
     */
    v1AdminCategoriesUpdate: (data: CategoryUpdateModel, params: RequestParams = {}) =>
      this.request<number, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/admin/categories`,
        method: 'PUT',
        body: data,
        secure: true,
        type: ContentType.Json,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Categories
     * @name V1CategoriesDetail
     * @request GET:/api/v1/categories/{id}
     */
    v1CategoriesDetail: (id: string, params: RequestParams = {}) =>
      this.request<CategoryReadModel, AppProblemDetails | AppErrorModel | ErrorModel>({
        path: `/api/v1/categories/${id}`,
        method: 'GET',
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Categories
     * @name V1CategoriesFilterCreate
     * @request POST:/api/v1/categories/filter
     */
    v1CategoriesFilterCreate: (data: CategoryFilterModel, params: RequestParams = {}) =>
      this.request<CategoryReadModelPageResultModel, AppProblemDetails | AppErrorModel | ErrorModel>({
        path: `/api/v1/categories/filter`,
        method: 'POST',
        body: data,
        type: ContentType.Json,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Categories
     * @name V1AdminCategoriesDelete
     * @request DELETE:/api/v1/admin/categories/{id}
     * @secure
     */
    v1AdminCategoriesDelete: (id: string, params: RequestParams = {}) =>
      this.request<number, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/admin/categories/${id}`,
        method: 'DELETE',
        secure: true,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Collections
     * @name V1CollectionsDetail
     * @request GET:/api/v1/collections/{id}
     */
    v1CollectionsDetail: (id: string, params: RequestParams = {}) =>
      this.request<CollectionReadModel, AppProblemDetails | AppErrorModel | ErrorModel>({
        path: `/api/v1/collections/${id}`,
        method: 'GET',
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Collections
     * @name V1CollectionsAdminCreate
     * @summary Создать коллецкцию
     * @request POST:/api/v1/collections/admin
     * @secure
     */
    v1CollectionsAdminCreate: (data: CollectionCreateModel, params: RequestParams = {}) =>
      this.request<string, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/collections/admin`,
        method: 'POST',
        body: data,
        secure: true,
        type: ContentType.Json,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Collections
     * @name V1CollectionsAdminList
     * @summary Получить список коллекция для админа
     * @request GET:/api/v1/collections/admin
     * @secure
     */
    v1CollectionsAdminList: (params: RequestParams = {}) =>
      this.request<CollectionReadModel[], AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/collections/admin`,
        method: 'GET',
        secure: true,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Collections
     * @name V1CollectionsAdminUpdate
     * @summary Обновить коллекцию
     * @request PUT:/api/v1/collections/admin
     * @secure
     */
    v1CollectionsAdminUpdate: (data: CollectionUpdateModel, params: RequestParams = {}) =>
      this.request<any, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/collections/admin`,
        method: 'PUT',
        body: data,
        secure: true,
        type: ContentType.Json,
        ...params,
      }),

    /**
     * No description
     *
     * @tags Collections
     * @name V1CollectionsList
     * @summary Получить список коллекций для покупателя
     * @request GET:/api/v1/collections
     */
    v1CollectionsList: (params: RequestParams = {}) =>
      this.request<CollectionReadModel[], AppProblemDetails | AppErrorModel | ErrorModel>({
        path: `/api/v1/collections`,
        method: 'GET',
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Collections
     * @name V1CollectionsAdminDelete
     * @summary Удалить коллекцию
     * @request DELETE:/api/v1/collections/admin/{id}
     * @secure
     */
    v1CollectionsAdminDelete: (id: string, params: RequestParams = {}) =>
      this.request<any, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/collections/admin/${id}`,
        method: 'DELETE',
        secure: true,
        ...params,
      }),

    /**
     * No description
     *
     * @tags Config
     * @name V1ConfigAdminGlobalSettingsList
     * @summary Получить глобальные настрокий админа
     * @request GET:/api/v1/config/admin/global-settings
     * @secure
     */
    v1ConfigAdminGlobalSettingsList: (params: RequestParams = {}) =>
      this.request<any, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/config/admin/global-settings`,
        method: 'GET',
        secure: true,
        ...params,
      }),

    /**
     * No description
     *
     * @tags Config
     * @name V1ConfigAdminGlobalSettingsUpdate
     * @summary установить глобальные настройки админа
     * @request PUT:/api/v1/config/admin/global-settings
     * @secure
     */
    v1ConfigAdminGlobalSettingsUpdate: (data: GlobaSettingsModel, params: RequestParams = {}) =>
      this.request<any, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/config/admin/global-settings`,
        method: 'PUT',
        body: data,
        secure: true,
        type: ContentType.Json,
        ...params,
      }),

    /**
 * No description
 *
 * @tags Config
 * @name V1ConfigList
 * @summary Возврашает с сервера необходимые для Angular SPA настройки.
Проверить состояние и версию API можно по адресу /health-check
 * @request GET:/api/v1/config
 */
    v1ConfigList: (params: RequestParams = {}) =>
      this.request<ConfigModel, AppProblemDetails | AppErrorModel | ErrorModel>({
        path: `/api/v1/config`,
        method: 'GET',
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Config
     * @name V1ConfigEnumsList
     * @request GET:/api/v1/config/enums
     */
    v1ConfigEnumsList: (params: RequestParams = {}) =>
      this.request<Record<string, EnumInfoModel[]>, AppProblemDetails | AppErrorModel | ErrorModel>({
        path: `/api/v1/config/enums`,
        method: 'GET',
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Delivery
     * @name V1DeliveryAvailableDeliveryIntervalsList
     * @summary Получить список доступных интервалов на дату доставки. Если не указать дату то выдаст ближайщие доступные
     * @request GET:/api/v1/delivery/available-delivery-intervals
     * @secure
     */
    v1DeliveryAvailableDeliveryIntervalsList: (
      query?: {
        /**
         * Дата доставки (день.месяц.год)
         * @format date-time
         */
        date?: string;
      },
      params: RequestParams = {},
    ) =>
      this.request<DeliveryInterval[], AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/delivery/available-delivery-intervals`,
        method: 'GET',
        query: query,
        secure: true,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Delivery
     * @name V1DeliveryStandartOrderPriceCreate
     * @summary Вычисляет стоимость доставки отклика обычного заказа
     * @request POST:/api/v1/delivery/standart-order/price
     * @secure
     */
    v1DeliveryStandartOrderPriceCreate: (data: DeliveryNowPriceModel, params: RequestParams = {}) =>
      this.request<CalculateDeliveryPriceReadModel, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/delivery/standart-order/price`,
        method: 'POST',
        body: data,
        secure: true,
        type: ContentType.Json,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Delivery
     * @name V1DeliveryIndividualOrderPriceCreate
     * @summary Вычисляет стоимость доставки отклика (Offer)
     * @request POST:/api/v1/delivery/individual-order/price
     * @secure
     */
    v1DeliveryIndividualOrderPriceCreate: (data: DeliveryIndividualPriceModel, params: RequestParams = {}) =>
      this.request<CalculateDeliveryPriceReadModel, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/delivery/individual-order/price`,
        method: 'POST',
        body: data,
        secure: true,
        type: ContentType.Json,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Delivery
     * @name V1SellerDeliveryOrderCreate
     * @summary Отдает службе доставке команду на доставку данного заказа
     * @request POST:/api/v1/seller/delivery/order
     * @secure
     */
    v1SellerDeliveryOrderCreate: (data: DeliveryOrderModel, params: RequestParams = {}) =>
      this.request<DeliveryOrderResponseModel, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/seller/delivery/order`,
        method: 'POST',
        body: data,
        secure: true,
        type: ContentType.Json,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Delivery
     * @name V1DostavistaCallbackCreate
     * @request POST:/api/v1/dostavista/callback
     */
    v1DostavistaCallbackCreate: (params: RequestParams = {}) =>
      this.request<any, AppProblemDetails | AppErrorModel | ErrorModel>({
        path: `/api/v1/dostavista/callback`,
        method: 'POST',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Devices
     * @name V1DevicesCreate
     * @summary Создать устройство с указанием Puth токена для него
     * @request POST:/api/v1/devices
     * @secure
     */
    v1DevicesCreate: (data: DeviceCreateModel, params: RequestParams = {}) =>
      this.request<GuidPageResultModel, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/devices`,
        method: 'POST',
        body: data,
        secure: true,
        type: ContentType.Json,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Devices
     * @name V1DevicesUpdate
     * @summary Обновить данные устройства
     * @request PUT:/api/v1/devices
     * @secure
     */
    v1DevicesUpdate: (data: DeviceUpdateModel, params: RequestParams = {}) =>
      this.request<void, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/devices`,
        method: 'PUT',
        body: data,
        secure: true,
        type: ContentType.Json,
        ...params,
      }),

    /**
     * No description
     *
     * @tags Devices
     * @name V1DevicesMyList
     * @summary Получить устройства текущего пользователя
     * @request GET:/api/v1/devices/my
     * @secure
     */
    v1DevicesMyList: (
      query?: {
        /**
         * Номер страницы (начиная с 1)
         * @format int32
         * @min 1
         * @max 4294967295
         */
        PageNumber?: number;
        /**
         * Размер страницы (максимум 1000)
         * @format int32
         * @min 1
         * @max 1000
         */
        PageSize?: number;
      },
      params: RequestParams = {},
    ) =>
      this.request<DeviceReadModelPageResultModel, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/devices/my`,
        method: 'GET',
        query: query,
        secure: true,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Devices
     * @name V1DevicesDelete
     * @summary Удалить устройство
     * @request DELETE:/api/v1/devices/{id}
     * @secure
     */
    v1DevicesDelete: (id: string, params: RequestParams = {}) =>
      this.request<CategoryReadModelPageResultModel, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/devices/${id}`,
        method: 'DELETE',
        secure: true,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Files
     * @name V1FilesCreate
     * @summary Загружает файл на сервер. На загруженное изображение будет наложена ватермарка во имя темных богов.
     * @request POST:/api/v1/files
     * @secure
     */
    v1FilesCreate: (
      data: {
        /** @format binary */
        file?: File;
      },
      params: RequestParams = {},
    ) =>
      this.request<string, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/files`,
        method: 'POST',
        body: data,
        secure: true,
        type: ContentType.FormData,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Files
     * @name V1FilesList
     * @summary Получить список информаций о файлах на сервере. Скачать файл можно по пути укзананному в свойтве Path
     * @request GET:/api/v1/files
     */
    v1FilesList: (
      query?: {
        /**
         * Номер страницы (начиная с 1)
         * @format int32
         * @min 1
         * @max 4294967295
         */
        PageNumber?: number;
        /**
         * Размер страницы (максимум 1000)
         * @format int32
         * @min 1
         * @max 1000
         */
        PageSize?: number;
      },
      params: RequestParams = {},
    ) =>
      this.request<FileReadModelPageResultModel, AppProblemDetails | AppErrorModel | ErrorModel>({
        path: `/api/v1/files`,
        method: 'GET',
        query: query,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Files
     * @name V1FilesDetail
     * @request GET:/api/v1/files/{id}
     */
    v1FilesDetail: (id: string, params: RequestParams = {}) =>
      this.request<FileReadModel, AppProblemDetails | AppErrorModel | ErrorModel>({
        path: `/api/v1/files/${id}`,
        method: 'GET',
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags IndividualOrders
     * @name V1IndividualApplicationsCreate
     * @summary Создание заявки на индивидуальный заказ
     * @request POST:/api/v1/individual-applications
     * @secure
     */
    v1IndividualApplicationsCreate: (data: ApplicationCreateModel, params: RequestParams = {}) =>
      this.request<string, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/individual-applications`,
        method: 'POST',
        body: data,
        secure: true,
        type: ContentType.Json,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags IndividualOrders
     * @name V1IndividualApplicationsList
     * @summary Получение списка моих заявок (созданных мной как покупателем)
     * @request GET:/api/v1/individual-applications
     * @secure
     */
    v1IndividualApplicationsList: (
      query?: {
        /**
         * Номер страницы (начиная с 1)
         * @format int32
         * @min 1
         * @max 4294967295
         */
        PageNumber?: number;
        /**
         * Размер страницы (максимум 1000)
         * @format int32
         * @min 1
         * @max 1000
         */
        PageSize?: number;
      },
      params: RequestParams = {},
    ) =>
      this.request<ApplicationReadModelPageResultModel, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/individual-applications`,
        method: 'GET',
        query: query,
        secure: true,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags IndividualOrders
     * @name V1IndividualApplicationsBuyedFilterCreate
     * @summary Получение списка купленный заказов (по продовцам чтобы показывать в инфо о продавце какие индивидуальные заказы у него купили)
     * @request POST:/api/v1/individual-applications/buyed/filter
     * @secure
     */
    v1IndividualApplicationsBuyedFilterCreate: (data: FiterBuyedApplicationsModel, params: RequestParams = {}) =>
      this.request<ApplicationReadModelPageResultModel, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/individual-applications/buyed/filter`,
        method: 'POST',
        body: data,
        secure: true,
        type: ContentType.Json,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags IndividualOrders
     * @name V1IndividualApplicationsDetail
     * @summary Получить подробную информацию о заявке по ее Id
     * @request GET:/api/v1/individual-applications/{id}
     * @secure
     */
    v1IndividualApplicationsDetail: (id: string, params: RequestParams = {}) =>
      this.request<ApplicationDetailsReadModel, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/individual-applications/${id}`,
        method: 'GET',
        secure: true,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags IndividualOrders
     * @name V1IndividualApplicationsDelete
     * @summary Удалить заявку вместо со всеми ее откликами
     * @request DELETE:/api/v1/individual-applications/{id}
     * @secure
     */
    v1IndividualApplicationsDelete: (id: string, params: RequestParams = {}) =>
      this.request<number, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/individual-applications/${id}`,
        method: 'DELETE',
        secure: true,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags IndividualOrders
     * @name V1IndividualApplicationsOffersDetail
     * @summary Получение подробной информации об откиле на заказ
     * @request GET:/api/v1/individual-applications/offers/{id}
     * @secure
     */
    v1IndividualApplicationsOffersDetail: (id: string, params: RequestParams = {}) =>
      this.request<OfferDetailsReamModel, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/individual-applications/offers/${id}`,
        method: 'GET',
        secure: true,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags IndividualOrders
     * @name V1IndividualApplicationsOffersCreate
     * @summary Создать отклик на заявку
     * @request POST:/api/v1/individual-applications/offers
     * @secure
     */
    v1IndividualApplicationsOffersCreate: (data: OfferCreateModel, params: RequestParams = {}) =>
      this.request<string, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/individual-applications/offers`,
        method: 'POST',
        body: data,
        secure: true,
        type: ContentType.Json,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags IndividualOrders
     * @name V1SellerIndividualApplicationsList
     * @summary Получение списка заявок доступных для отклика
     * @request GET:/api/v1/seller/individual-applications
     * @secure
     */
    v1SellerIndividualApplicationsList: (
      query?: {
        /**
         * Номер страницы (начиная с 1)
         * @format int32
         * @min 1
         * @max 4294967295
         */
        PageNumber?: number;
        /**
         * Размер страницы (максимум 1000)
         * @format int32
         * @min 1
         * @max 1000
         */
        PageSize?: number;
      },
      params: RequestParams = {},
    ) =>
      this.request<ApplicationReadModelPageResultModel, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/seller/individual-applications`,
        method: 'GET',
        query: query,
        secure: true,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags IndividualOrders
     * @name V1IndividualApplicationsOffersCommentsCreate
     * @summary Создать коментарий к отклику
     * @request POST:/api/v1/individual-applications/offers/comments
     * @secure
     */
    v1IndividualApplicationsOffersCommentsCreate: (data: OfferCommentCreateModel, params: RequestParams = {}) =>
      this.request<string, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/individual-applications/offers/comments`,
        method: 'POST',
        body: data,
        secure: true,
        type: ContentType.Json,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags IndividualOrders
     * @name V1SellerIndividualApplicationsOffersMyFilterCreate
     * @summary Список откликов который создал я. Откикликов на индивидумальный заказ
     * @request POST:/api/v1/seller/individual-applications/offers/my/filter
     * @secure
     */
    v1SellerIndividualApplicationsOffersMyFilterCreate: (data: PaginationModel, params: RequestParams = {}) =>
      this.request<OfferSellerReadModelPageResultModel, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/seller/individual-applications/offers/my/filter`,
        method: 'POST',
        body: data,
        secure: true,
        type: ContentType.Json,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags IndividualOrders
     * @name V1SellerIndividualApplicationsOffersDelete
     * @summary Удалить отклик вместе со всеми ее коментариями
     * @request DELETE:/api/v1/seller/individual-applications/offers/{id}
     * @secure
     */
    v1SellerIndividualApplicationsOffersDelete: (id: string, params: RequestParams = {}) =>
      this.request<number, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/seller/individual-applications/offers/${id}`,
        method: 'DELETE',
        secure: true,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Merchandises
     * @name V1GoodsRequestCompositionUpdate
     * @summary Запросить состав товара
     * @request PUT:/api/v1/goods/{id}/request-composition
     * @secure
     */
    v1GoodsRequestCompositionUpdate: (id: string, params: RequestParams = {}) =>
      this.request<MerchandiseReadModelPageResultModel, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/goods/${id}/request-composition`,
        method: 'PUT',
        secure: true,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Merchandises
     * @name V1GoodsDetail
     * @summary Получить товар по идентификатору
     * @request GET:/api/v1/goods/{id}
     */
    v1GoodsDetail: (id: string, params: RequestParams = {}) =>
      this.request<MerchandiseReadModel, AppProblemDetails | AppErrorModel | ErrorModel>({
        path: `/api/v1/goods/${id}`,
        method: 'GET',
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Merchandises
     * @name V1GoodsFilterCreate
     * @summary Получить список товаров для главной страницы (где у нас витрина покупателя)
     * @request POST:/api/v1/goods/filter
     */
    v1GoodsFilterCreate: (data: FilterMerchandiseDetailsModel, params: RequestParams = {}) =>
      this.request<MerchandiseReadModelPageResultModel, AppProblemDetails | AppErrorModel | ErrorModel>({
        path: `/api/v1/goods/filter`,
        method: 'POST',
        body: data,
        type: ContentType.Json,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Merchandises
     * @name V1GoodsFilterSellerPageCreate
     * @summary Получить список товаров для таба с товарами продавца (где у нас страница продавца)
     * @request POST:/api/v1/goods/filter/seller-page
     */
    v1GoodsFilterSellerPageCreate: (data: FilterMerchandiseByPartnerDetailsModel, params: RequestParams = {}) =>
      this.request<MerchandiseReadModelPageResultModel, AppProblemDetails | AppErrorModel | ErrorModel>({
        path: `/api/v1/goods/filter/seller-page`,
        method: 'POST',
        body: data,
        type: ContentType.Json,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Merchandises
     * @name V1SellerGoodsFilterSellerGoodsPageCreate
     * @summary Получить список товаров для редактирования провдцом (страница мои товары на фронте продавца)
     * @request POST:/api/v1/seller/goods/filter/seller-goods-page
     * @secure
     */
    v1SellerGoodsFilterSellerGoodsPageCreate: (
      data: FilterMerchandiseByPartnerForPartnerDetailsModel,
      params: RequestParams = {},
    ) =>
      this.request<MerchandiseReadModelPageResultModel, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/seller/goods/filter/seller-goods-page`,
        method: 'POST',
        body: data,
        secure: true,
        type: ContentType.Json,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Merchandises
     * @name V1AdminGoodsFilterCreate
     * @summary Получить список товаров для редактирования Админом (страница управление товарами на фронте продавца)
     * @request POST:/api/v1/admin/goods/filter
     * @secure
     */
    v1AdminGoodsFilterCreate: (data: FilterMerchandiseForAdminDetailsModel, params: RequestParams = {}) =>
      this.request<MerchandiseReadModelPageResultModel, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/admin/goods/filter`,
        method: 'POST',
        body: data,
        secure: true,
        type: ContentType.Json,
        format: 'json',
        ...params,
      }),

    /**
     * @description Добавить может только пользователь с ролью "seller" Владельцем автоматически назначается пользователь что добавил товар. Для админа будет отдельный метод да и вообще пока что админим через БД напрямую. Это метод именно для зарегестрированного продавца.
     *
     * @tags Merchandises
     * @name V1SellerGoodsCreate
     * @summary Добавляет новый товар в список товаров продавца
     * @request POST:/api/v1/seller/goods
     * @secure
     */
    v1SellerGoodsCreate: (data: MerchandiseCreateModel, params: RequestParams = {}) =>
      this.request<string, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/seller/goods`,
        method: 'POST',
        body: data,
        secure: true,
        type: ContentType.Json,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Merchandises
     * @name V1SellerGoodsUpdate
     * @request PUT:/api/v1/seller/goods
     * @secure
     */
    v1SellerGoodsUpdate: (data: MerchandiseUpdateModel, params: RequestParams = {}) =>
      this.request<number, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/seller/goods`,
        method: 'PUT',
        body: data,
        secure: true,
        type: ContentType.Json,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Merchandises
     * @name V1AdminGoodsUpdate
     * @summary Обновить данные товара админом
     * @request PUT:/api/v1/admin/goods
     * @secure
     */
    v1AdminGoodsUpdate: (data: AdminMerchandiseUpdateModel, params: RequestParams = {}) =>
      this.request<number, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/admin/goods`,
        method: 'PUT',
        body: data,
        secure: true,
        type: ContentType.Json,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Merchandises
     * @name V1SellerGoodsDelete
     * @summary Удалить товар
     * @request DELETE:/api/v1/seller/goods/{id}
     * @secure
     */
    v1SellerGoodsDelete: (id: string, params: RequestParams = {}) =>
      this.request<number, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/seller/goods/${id}`,
        method: 'DELETE',
        secure: true,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Merchandises
     * @name V1SellerGoodsPublishUpdate
     * @summary Опубликовать товар
     * @request PUT:/api/v1/seller/goods/{id}/publish
     * @secure
     */
    v1SellerGoodsPublishUpdate: (id: string, params: RequestParams = {}) =>
      this.request<number, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/seller/goods/${id}/publish`,
        method: 'PUT',
        secure: true,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Merchandises
     * @name V1SellerGoodsUnpublishUpdate
     * @summary Снять товар с публикации
     * @request PUT:/api/v1/seller/goods/{id}/unpublish
     * @secure
     */
    v1SellerGoodsUnpublishUpdate: (id: string, params: RequestParams = {}) =>
      this.request<number, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/seller/goods/${id}/unpublish`,
        method: 'PUT',
        secure: true,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Messaging
     * @name V1MessagingSendFeedbackCreate
     * @request POST:/api/v1/messaging/send-feedback
     */
    v1MessagingSendFeedbackCreate: (
      data: {
        Name?: string;
        /** @format email */
        Email: string;
        Message?: string;
        Files?: File[];
      },
      params: RequestParams = {},
    ) =>
      this.request<any, AppProblemDetails | AppErrorModel | ErrorModel>({
        path: `/api/v1/messaging/send-feedback`,
        method: 'POST',
        body: data,
        type: ContentType.FormData,
        ...params,
      }),

    /**
     * No description
     *
     * @tags Messaging
     * @name V1MessagingSendFromLendingCreate
     * @request POST:/api/v1/messaging/send-from-lending
     */
    v1MessagingSendFromLendingCreate: (data: LendingMessageModel, params: RequestParams = {}) =>
      this.request<any, AppProblemDetails | AppErrorModel | ErrorModel>({
        path: `/api/v1/messaging/send-from-lending`,
        method: 'POST',
        body: data,
        type: ContentType.Json,
        ...params,
      }),

    /**
     * No description
     *
     * @tags Messaging
     * @name V1MessagingSendVerificationCodeCreate
     * @request POST:/api/v1/messaging/send-verification-code
     * @secure
     */
    v1MessagingSendVerificationCodeCreate: (data: SendVerificationSmsModel, params: RequestParams = {}) =>
      this.request<VerivicationStatus, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/messaging/send-verification-code`,
        method: 'POST',
        body: data,
        secure: true,
        type: ContentType.Json,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Messaging
     * @name V1MessagingCheckVerificationCodeCreate
     * @request POST:/api/v1/messaging/check-verification-code
     * @secure
     */
    v1MessagingCheckVerificationCodeCreate: (data: CheckVerificationCodeModel, params: RequestParams = {}) =>
      this.request<VerivicationStatus, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/messaging/check-verification-code`,
        method: 'POST',
        body: data,
        secure: true,
        type: ContentType.Json,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Orders
     * @name V1OrdersIndividualSelfDeliveredCreate
     * @summary Создать индивидуальный заказ с самовывозом
     * @request POST:/api/v1/orders/individual-self-delivered
     * @secure
     */
    v1OrdersIndividualSelfDeliveredCreate: (
      data: OrderIndividualSelfDeliveredCreateModel,
      params: RequestParams = {},
    ) =>
      this.request<string, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/orders/individual-self-delivered`,
        method: 'POST',
        body: data,
        secure: true,
        type: ContentType.Json,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Orders
     * @name V1OrdersIndividualCreate
     * @summary Создать индивидуальный заказ
     * @request POST:/api/v1/orders/individual
     * @secure
     */
    v1OrdersIndividualCreate: (data: OrderIndividualCreateModel, params: RequestParams = {}) =>
      this.request<string, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/orders/individual`,
        method: 'POST',
        body: data,
        secure: true,
        type: ContentType.Json,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Orders
     * @name V1OrdersSelfDeliveredCreate
     * @summary Создать заказ с самовывозом
     * @request POST:/api/v1/orders/self-delivered
     * @secure
     */
    v1OrdersSelfDeliveredCreate: (data: OrderSerfDeliveredCreateModel, params: RequestParams = {}) =>
      this.request<string, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/orders/self-delivered`,
        method: 'POST',
        body: data,
        secure: true,
        type: ContentType.Json,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Orders
     * @name V1OrdersCreate
     * @summary Создать базовый заказ
     * @request POST:/api/v1/orders
     * @secure
     */
    v1OrdersCreate: (data: OrderCreateModel, params: RequestParams = {}) =>
      this.request<string, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/orders`,
        method: 'POST',
        body: data,
        secure: true,
        type: ContentType.Json,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Orders
     * @name V1OrdersDetail
     * @summary Получить заказ текущего авторизованного пользователя по идентификатору заказа
     * @request GET:/api/v1/orders/{id}
     * @secure
     */
    v1OrdersDetail: (id: string, params: RequestParams = {}) =>
      this.request<OrderReadModel, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/orders/${id}`,
        method: 'GET',
        secure: true,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Orders
     * @name V1SellerOrdersDetail
     * @summary Получить заказ сделанный текущему продавцу
     * @request GET:/api/v1/seller/orders/{id}
     * @secure
     */
    v1SellerOrdersDetail: (id: string, params: RequestParams = {}) =>
      this.request<OrderReadModel, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/seller/orders/${id}`,
        method: 'GET',
        secure: true,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Orders
     * @name V1OrdersFilterCreate
     * @summary Получить список заказов текущего пользователя
     * @request POST:/api/v1/orders/filter
     * @secure
     */
    v1OrdersFilterCreate: (data: PaginationModel, params: RequestParams = {}) =>
      this.request<OrderReadModelPageResultModel, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/orders/filter`,
        method: 'POST',
        body: data,
        secure: true,
        type: ContentType.Json,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Orders
     * @name V1SellerOrdersFilterCreate
     * @summary Получить список заказов поступивших текущему продавцу
     * @request POST:/api/v1/seller/orders/filter
     * @secure
     */
    v1SellerOrdersFilterCreate: (data: PaginationModel, params: RequestParams = {}) =>
      this.request<OrderReadModelPageResultModel, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/seller/orders/filter`,
        method: 'POST',
        body: data,
        secure: true,
        type: ContentType.Json,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Partners
     * @name V1PartnersList
     * @summary Получить список партнеров для покупателя
     * @request GET:/api/v1/partners
     */
    v1PartnersList: (params: RequestParams = {}) =>
      this.request<PartnerReadModel[], AppProblemDetails | AppErrorModel | ErrorModel>({
        path: `/api/v1/partners`,
        method: 'GET',
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Partners
     * @name V1PartnersCreate
     * @summary Создать нового партнера в системе
     * @request POST:/api/v1/partners
     * @secure
     */
    v1PartnersCreate: (data: PartnerCreateModel, params: RequestParams = {}) =>
      this.request<string, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/partners`,
        method: 'POST',
        body: data,
        secure: true,
        type: ContentType.Json,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Partners
     * @name V1PartnersDetail
     * @summary Получить данные о партнере
     * @request GET:/api/v1/partners/{id}
     */
    v1PartnersDetail: (id: string, params: RequestParams = {}) =>
      this.request<PartnerReadModel, AppProblemDetails | AppErrorModel | ErrorModel>({
        path: `/api/v1/partners/${id}`,
        method: 'GET',
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Partners
     * @name V1PartnersMyList
     * @summary Получить данные о моем партнере
     * @request GET:/api/v1/partners/my
     * @secure
     */
    v1PartnersMyList: (params: RequestParams = {}) =>
      this.request<PartnerReadModel, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/partners/my`,
        method: 'GET',
        secure: true,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Partners
     * @name V1SellerPartnersUpdate
     * @summary Обновить свободные данные для партнера
     * @request PUT:/api/v1/seller/partners
     * @secure
     */
    v1SellerPartnersUpdate: (data: PartnerUpdateModel, params: RequestParams = {}) =>
      this.request<number, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/seller/partners`,
        method: 'PUT',
        body: data,
        secure: true,
        type: ContentType.Json,
        format: 'json',
        ...params,
      }),

    /**
 * No description
 *
 * @tags Partners
 * @name V1AdminPartnersConfirmCreate
 * @summary Подтвердить партнера.
Это даст ему возможность размещать товары на сайте.
К тому же это дасть директору этого магазина роль seller
 * @request POST:/api/v1/admin/partners/confirm/{id}
 * @secure
 */
    v1AdminPartnersConfirmCreate: (id: string, params: RequestParams = {}) =>
      this.request<number, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/admin/partners/confirm/${id}`,
        method: 'POST',
        secure: true,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Partners
     * @name V1AdminPartnersBlockCreate
     * @summary Запблокировать партнера
     * @request POST:/api/v1/admin/partners/{id}/block
     * @secure
     */
    v1AdminPartnersBlockCreate: (id: string, params: RequestParams = {}) =>
      this.request<number, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/admin/partners/${id}/block`,
        method: 'POST',
        secure: true,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Partners
     * @name V1AdminPartnersUnblockCreate
     * @summary Разблокировать партнера во имя Кхорна и Тзинча
     * @request POST:/api/v1/admin/partners/{id}/unblock
     * @secure
     */
    v1AdminPartnersUnblockCreate: (id: string, params: RequestParams = {}) =>
      this.request<number, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/admin/partners/${id}/unblock`,
        method: 'POST',
        secure: true,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Partners
     * @name V1AdmimPartnersList
     * @summary Получить список партнеров для админа
     * @request GET:/api/v1/admim/partners
     */
    v1AdmimPartnersList: (params: RequestParams = {}) =>
      this.request<PartnerReadModel[], AppProblemDetails | AppErrorModel | ErrorModel>({
        path: `/api/v1/admim/partners`,
        method: 'GET',
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Partners
     * @name V1AdminPartnersSetExternalIdUpdate
     * @summary Установить внешний id
     * @request PUT:/api/v1/admin/partners/{id}/set-external-id/{externalId}
     * @secure
     */
    v1AdminPartnersSetExternalIdUpdate: (id: string, externalId: string, params: RequestParams = {}) =>
      this.request<number, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/admin/partners/${id}/set-external-id/${externalId}`,
        method: 'PUT',
        secure: true,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Payments
     * @name V1PayAnyWayPayOrderDetail
     * @summary Оплатить заказ
     * @request GET:/api/v1/pay-any-way/pay-order/{id}
     */
    v1PayAnyWayPayOrderDetail: (id: string, params: RequestParams = {}) =>
      this.request<string, AppProblemDetails | AppErrorModel | ErrorModel>({
        path: `/api/v1/pay-any-way/pay-order/${id}`,
        method: 'GET',
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Payments
     * @name V1PayAnyWayDefaultDetail
     * @request GET:/api/v1/pay-any-way/default/{id}
     */
    v1PayAnyWayDefaultDetail: (id: string, params: RequestParams = {}) =>
      this.request<any, AppProblemDetails | AppErrorModel | ErrorModel>({
        path: `/api/v1/pay-any-way/default/${id}`,
        method: 'GET',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Payments
     * @name V1PayAnyWayCallbackList
     * @summary Метод обратного вызова
     * @request GET:/api/v1/pay-any-way/callback
     */
    v1PayAnyWayCallbackList: (
      query?: {
        MNT_ID?: string;
        MNT_TRANSACTION_ID?: string;
        MNT_OPERATION_ID?: string;
        MNT_AMOUNT?: string;
        MNT_CURRENCY_CODE?: string;
        MNT_SUBSCRIBER_ID?: string;
        MNT_TEST_MODE?: string;
        MNT_SIGNATURE?: string;
      },
      params: RequestParams = {},
    ) =>
      this.request<string, AppProblemDetails | AppErrorModel | ErrorModel>({
        path: `/api/v1/pay-any-way/callback`,
        method: 'GET',
        query: query,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Ratings
     * @name V1MyAssignedMerchRatingDetail
     * @summary Получить оценку которую я ставил этому товару
     * @request GET:/api/v1/my-assigned-merch-rating/{merchId}
     * @secure
     */
    v1MyAssignedMerchRatingDetail: (merchId: string, params: RequestParams = {}) =>
      this.request<any, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/my-assigned-merch-rating/${merchId}`,
        method: 'GET',
        secure: true,
        ...params,
      }),

    /**
     * No description
     *
     * @tags Ratings
     * @name V1MyAssignedPartnerRatingDetail
     * @summary Получить отзыв который я ставил этому товару
     * @request GET:/api/v1/my-assigned-partner-rating/{partnerId}
     * @secure
     */
    v1MyAssignedPartnerRatingDetail: (partnerId: string, params: RequestParams = {}) =>
      this.request<any, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/my-assigned-partner-rating/${partnerId}`,
        method: 'GET',
        secure: true,
        ...params,
      }),

    /**
     * No description
     *
     * @tags Ratings
     * @name V1CanISetRatingToMerchDetail
     * @summary Проверяет может ли текущий пользователь поставить оценку этому товару
     * @request GET:/api/v1/can-i-set-rating-to-merch/{merchId}
     * @secure
     */
    v1CanISetRatingToMerchDetail: (merchId: string, params: RequestParams = {}) =>
      this.request<any, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/can-i-set-rating-to-merch/${merchId}`,
        method: 'GET',
        secure: true,
        ...params,
      }),

    /**
     * No description
     *
     * @tags Ratings
     * @name V1CanISetRatingToPartnerDetail
     * @summary Проверяет может ли пользователь поставить оценку данному продавцу
     * @request GET:/api/v1/can-i-set-rating-to-partner/{partnerId}
     * @secure
     */
    v1CanISetRatingToPartnerDetail: (partnerId: string, params: RequestParams = {}) =>
      this.request<any, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/can-i-set-rating-to-partner/${partnerId}`,
        method: 'GET',
        secure: true,
        ...params,
      }),

    /**
     * No description
     *
     * @tags Ratings
     * @name V1MerchRatingsCreate
     * @summary ДОбавить оценку товару
     * @request POST:/api/v1/merch-ratings
     * @secure
     */
    v1MerchRatingsCreate: (data: MerchRatingCreateModel, params: RequestParams = {}) =>
      this.request<string, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/merch-ratings`,
        method: 'POST',
        body: data,
        secure: true,
        type: ContentType.Json,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Ratings
     * @name V1MerchRatingsDetail
     * @summary Прочитать данные оценки товара
     * @request GET:/api/v1/merch-ratings/{id}
     * @secure
     */
    v1MerchRatingsDetail: (id: string, params: RequestParams = {}) =>
      this.request<MerchRatingReadModel, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/merch-ratings/${id}`,
        method: 'GET',
        secure: true,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Ratings
     * @name V1MerchRatingsFilterCreate
     * @summary Получить список оценок товара
     * @request POST:/api/v1/merch-ratings/filter
     */
    v1MerchRatingsFilterCreate: (data: MerchRatingFilterModel, params: RequestParams = {}) =>
      this.request<MerchRatingReadModelPageResultModel, AppProblemDetails | AppErrorModel | ErrorModel>({
        path: `/api/v1/merch-ratings/filter`,
        method: 'POST',
        body: data,
        type: ContentType.Json,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Ratings
     * @name V1PartnerRatingsCreate
     * @summary Добавить оценку фрилансеру
     * @request POST:/api/v1/partner-ratings
     * @secure
     */
    v1PartnerRatingsCreate: (data: PartnerRatingCreateModel, params: RequestParams = {}) =>
      this.request<string, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/partner-ratings`,
        method: 'POST',
        body: data,
        secure: true,
        type: ContentType.Json,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Ratings
     * @name V1PartnerRatingsDetail
     * @summary Прочитать данные оценки
     * @request GET:/api/v1/partner-ratings/{id}
     * @secure
     */
    v1PartnerRatingsDetail: (id: string, params: RequestParams = {}) =>
      this.request<PartnerRatingReadModel, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/partner-ratings/${id}`,
        method: 'GET',
        secure: true,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Ratings
     * @name V1PartnerRatingsFilterCreate
     * @summary Получить список оценок фрилансеров
     * @request POST:/api/v1/partner-ratings/filter
     */
    v1PartnerRatingsFilterCreate: (data: PartnerRatingFilterModel, params: RequestParams = {}) =>
      this.request<PartnerRatingReadModelPageResultModel, AppProblemDetails | AppErrorModel | ErrorModel>({
        path: `/api/v1/partner-ratings/filter`,
        method: 'POST',
        body: data,
        type: ContentType.Json,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Ratings
     * @name V1RatingsUpdate
     * @summary Обновить поставленную оценку товару или продавцу (фрилансеру)
     * @request PUT:/api/v1/ratings
     * @secure
     */
    v1RatingsUpdate: (data: RatingUpdateModel, params: RequestParams = {}) =>
      this.request<void, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/ratings`,
        method: 'PUT',
        body: data,
        secure: true,
        type: ContentType.Json,
        ...params,
      }),

    /**
     * No description
     *
     * @tags Roles
     * @name V1AdminRolesList
     * @summary Получить список ролей которые есть в системе
     * @request GET:/api/v1/admin/roles
     * @secure
     */
    v1AdminRolesList: (params: RequestParams = {}) =>
      this.request<IdentityRole[], AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/admin/roles`,
        method: 'GET',
        secure: true,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Shops
     * @name V1SellerShopsCreate
     * @summary Создать магазин
     * @request POST:/api/v1/seller/shops
     * @secure
     */
    v1SellerShopsCreate: (data: ShopCreateModel, params: RequestParams = {}) =>
      this.request<string, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/seller/shops`,
        method: 'POST',
        body: data,
        secure: true,
        type: ContentType.Json,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Test
     * @name V1TestCalculateYookassaPyamentSelfDeliveredCreate
     * @request POST:/api/v1/Test/calculate-yookassa-pyament-self-delivered/{id}
     */
    v1TestCalculateYookassaPyamentSelfDeliveredCreate: (id: string, params: RequestParams = {}) =>
      this.request<any, AppProblemDetails | AppErrorModel | ErrorModel>({
        path: `/api/v1/Test/calculate-yookassa-pyament-self-delivered/${id}`,
        method: 'POST',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Test
     * @name V1TestCalculateYookassaPyamentCreate
     * @request POST:/api/v1/Test/calculate-yookassa-pyament/{id}
     */
    v1TestCalculateYookassaPyamentCreate: (id: string, params: RequestParams = {}) =>
      this.request<any, AppProblemDetails | AppErrorModel | ErrorModel>({
        path: `/api/v1/Test/calculate-yookassa-pyament/${id}`,
        method: 'POST',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Test
     * @name V1TestCalculateDostavistaOrderCreate
     * @request POST:/api/v1/Test/calculate-dostavista-order
     */
    v1TestCalculateDostavistaOrderCreate: (data: DostavistaCalculateOrderRequest, params: RequestParams = {}) =>
      this.request<any, AppProblemDetails | AppErrorModel | ErrorModel>({
        path: `/api/v1/Test/calculate-dostavista-order`,
        method: 'POST',
        body: data,
        type: ContentType.Json,
        ...params,
      }),

    /**
     * No description
     *
     * @tags Test
     * @name V1TestList
     * @request GET:/api/v1/Test
     * @secure
     */
    v1TestList: (params: RequestParams = {}) =>
      this.request<any, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/Test`,
        method: 'GET',
        secure: true,
        ...params,
      }),

    /**
     * No description
     *
     * @tags Test
     * @name V1TestCreate
     * @request POST:/api/v1/Test
     */
    v1TestCreate: (data: WeatherForecast, params: RequestParams = {}) =>
      this.request<any, AppProblemDetails | AppErrorModel | ErrorModel>({
        path: `/api/v1/Test`,
        method: 'POST',
        body: data,
        type: ContentType.Json,
        ...params,
      }),

    /**
     * No description
     *
     * @tags Test
     * @name V1TestUpdate
     * @request PUT:/api/v1/Test
     */
    v1TestUpdate: (params: RequestParams = {}) =>
      this.request<any, AppProblemDetails | AppErrorModel | ErrorModel>({
        path: `/api/v1/Test`,
        method: 'PUT',
        ...params,
      }),

    /**
     * No description
     *
     * @tags Test
     * @name V1TestGetTestList
     * @request GET:/api/v1/Test/GetTest
     */
    v1TestGetTestList: (
      query?: {
        /** @format int32 */
        Id?: number;
        Names?: string[];
        /** @format int32 */
        'TestRng.From'?: number;
        /** @format int32 */
        'TestRng.To'?: number;
        'TestRng.Names'?: number[];
        TestList?: TestRng[];
      },
      params: RequestParams = {},
    ) =>
      this.request<any, AppProblemDetails | AppErrorModel | ErrorModel>({
        path: `/api/v1/Test/GetTest`,
        method: 'GET',
        query: query,
        ...params,
      }),

    /**
     * No description
     *
     * @tags Test
     * @name V1TestGet403ErrorList
     * @request GET:/api/v1/Test/Get403Error
     */
    v1TestGet403ErrorList: (params: RequestParams = {}) =>
      this.request<any, AppProblemDetails | AppErrorModel | ErrorModel>({
        path: `/api/v1/Test/Get403Error`,
        method: 'GET',
        ...params,
      }),

    /**
     * No description
     *
     * @tags UserProfile
     * @name V1UserProfileMyList
     * @summary Получить данные текущего пользователя
     * @request GET:/api/v1/user-profile/my
     * @secure
     */
    v1UserProfileMyList: (params: RequestParams = {}) =>
      this.request<UserProfileDetailsReadModel, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/user-profile/my`,
        method: 'GET',
        secure: true,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags UserProfile
     * @name V1UserProfileMyUpdate
     * @summary Обновить профиль пользователя
     * @request PUT:/api/v1/user-profile/my
     * @secure
     */
    v1UserProfileMyUpdate: (data: UserProfileUpdateModel, params: RequestParams = {}) =>
      this.request<void, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/user-profile/my`,
        method: 'PUT',
        body: data,
        secure: true,
        type: ContentType.Json,
        ...params,
      }),

    /**
     * No description
     *
     * @tags UserProfile
     * @name V1AdminUserProfileDetail
     * @request GET:/api/v1/admin/user-profile/{id}
     * @secure
     */
    v1AdminUserProfileDetail: (id: string, params: RequestParams = {}) =>
      this.request<UserProfileDetailsReadModel, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/admin/user-profile/${id}`,
        method: 'GET',
        secure: true,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags UserProfile
     * @name V1AdminUserProfileFilterCreate
     * @request POST:/api/v1/admin/user-profile/filter
     * @secure
     */
    v1AdminUserProfileFilterCreate: (data: UserProfileFilterModel, params: RequestParams = {}) =>
      this.request<UserProfileReadModelPageResultModel, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/admin/user-profile/filter`,
        method: 'POST',
        body: data,
        secure: true,
        type: ContentType.Json,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags UserProfile
     * @name V1AdminUserProfileRoleCreate
     * @summary Добавить пользователю роль
     * @request POST:/api/v1/admin/user-profile/{id}/role/{roleId}
     * @secure
     */
    v1AdminUserProfileRoleCreate: (id: string, roleId: string, params: RequestParams = {}) =>
      this.request<number, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/admin/user-profile/${id}/role/${roleId}`,
        method: 'POST',
        secure: true,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags UserProfile
     * @name V1AdminUserProfileRoleDelete
     * @summary Забрать у пользователя роль
     * @request DELETE:/api/v1/admin/user-profile/{id}/role/{roleId}
     * @secure
     */
    v1AdminUserProfileRoleDelete: (id: string, roleId: string, params: RequestParams = {}) =>
      this.request<number, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/admin/user-profile/${id}/role/${roleId}`,
        method: 'DELETE',
        secure: true,
        format: 'json',
        ...params,
      }),

    /**
 * No description
 *
 * @tags UserProfile
 * @name V1UserProfileMyPasswordCreate
 * @summary Создать пароль для пользователя.
Это нужно когда пользователь зарегестрировался через Google или другого внешнего провайдера.
 * @request POST:/api/v1/user-profile/my/password
 * @secure
 */
    v1UserProfileMyPasswordCreate: (data: PasswordCreateModel, params: RequestParams = {}) =>
      this.request<void, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/user-profile/my/password`,
        method: 'POST',
        body: data,
        secure: true,
        type: ContentType.Json,
        ...params,
      }),

    /**
     * No description
     *
     * @tags UserProfile
     * @name V1UserProfileMyPasswordUpdate
     * @summary Сменить пароль текущего пользователя
     * @request PUT:/api/v1/user-profile/my/password
     * @secure
     */
    v1UserProfileMyPasswordUpdate: (data: PasswordUpdateModel, params: RequestParams = {}) =>
      this.request<void, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/user-profile/my/password`,
        method: 'PUT',
        body: data,
        secure: true,
        type: ContentType.Json,
        ...params,
      }),

    /**
     * No description
     *
     * @tags UserProfile
     * @name V1UserProfileMyEmailUpdate
     * @summary Сменить email текущего пользователя.
     * @request PUT:/api/v1/user-profile/my/email
     * @secure
     */
    v1UserProfileMyEmailUpdate: (data: EmailUpdateModel, params: RequestParams = {}) =>
      this.request<void, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/user-profile/my/email`,
        method: 'PUT',
        body: data,
        secure: true,
        type: ContentType.Json,
        ...params,
      }),

    /**
     * No description
     *
     * @tags UserProfile
     * @name V1UserProfileMyExternalLoginDelete
     * @summary Удалить логин
     * @request DELETE:/api/v1/user-profile/my/external-login
     * @secure
     */
    v1UserProfileMyExternalLoginDelete: (data: LoginRemoveModel, params: RequestParams = {}) =>
      this.request<void, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/user-profile/my/external-login`,
        method: 'DELETE',
        body: data,
        secure: true,
        type: ContentType.Json,
        ...params,
      }),

    /**
     * No description
     *
     * @tags UserProfile
     * @name V1UserProfileMyExternalLoginCreate
     * @summary Добавить логин
     * @request POST:/api/v1/user-profile/my/external-login
     * @secure
     */
    v1UserProfileMyExternalLoginCreate: (data: AddLoginModel, params: RequestParams = {}) =>
      this.request<string, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/user-profile/my/external-login`,
        method: 'POST',
        body: data,
        secure: true,
        type: ContentType.Json,
        format: 'json',
        ...params,
      }),

    /**
     * No description
     *
     * @tags UserProfile
     * @name V1UserProfileMyExternalLoginList
     * @summary Колбек для подтверждения добавления логина
     * @request GET:/api/v1/user-profile/my/external-login
     * @secure
     */
    v1UserProfileMyExternalLoginList: (params: RequestParams = {}) =>
      this.request<void, AppProblemDetails | void | AppErrorModel | ErrorModel>({
        path: `/api/v1/user-profile/my/external-login`,
        method: 'GET',
        secure: true,
        ...params,
      }),
  };
  configuration = {
    /**
     * No description
     *
     * @tags OidcConfiguration
     * @name ConfigurationDetail
     * @summary Получить настройки oidc клиента для oauth (Наш клиент Hike)
     * @request GET:/_configuration/{clientId}
     */
    configurationDetail: (clientId: string, params: RequestParams = {}) =>
      this.request<Record<string, string>, AppProblemDetails | AppErrorModel | ErrorModel>({
        path: `/_configuration/${clientId}`,
        method: 'GET',
        format: 'json',
        ...params,
      }),
  };
  hubs = {
    /**
     * No description
     *
     * @tags RealtimeHub
     * @name RealtimeHubSomeMethodCreate
     * @request POST:/hubs/RealtimeHub/SomeMethod
     */
    realtimeHubSomeMethodCreate: (
      query?: {
        /** @format int32 */
        value?: number;
      },
      params: RequestParams = {},
    ) =>
      this.request<any, any>({
        path: `/hubs/RealtimeHub/SomeMethod`,
        method: 'POST',
        query: query,
        ...params,
      }),

    /**
     * No description
     *
     * @tags WebSocketsClientEvents
     * @name WebSocketsClientEventsOrderStatusChangedCreate
     * @request POST:/hubs/WebSocketsClientEvents/OrderStatusChanged
     */
    webSocketsClientEventsOrderStatusChangedCreate: (
      query?: {
        state?: OrderState;
      },
      params: RequestParams = {},
    ) =>
      this.request<any, any>({
        path: `/hubs/WebSocketsClientEvents/OrderStatusChanged`,
        method: 'POST',
        query: query,
        ...params,
      }),

    /**
     * No description
     *
     * @tags WebSocketsClientEvents
     * @name WebSocketsClientEventsOfferCommentAddedCreate
     * @request POST:/hubs/WebSocketsClientEvents/OfferCommentAdded
     */
    webSocketsClientEventsOfferCommentAddedCreate: (
      query?: {
        model?: OfferCommentReadModel;
      },
      params: RequestParams = {},
    ) =>
      this.request<any, any>({
        path: `/hubs/WebSocketsClientEvents/OfferCommentAdded`,
        method: 'POST',
        query: query,
        ...params,
      }),
  };
}
