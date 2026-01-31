import { BulbOutlined, OrderedListOutlined, RiseOutlined, StarOutlined } from '@ant-design/icons';
import { RadioChangeEvent, Select } from 'antd';
import { ChangeEvent, FC, useCallback, useEffect, useMemo, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { debounce } from 'throttle-debounce';

import { useGetGoodsByFilter } from '~/api';
import { useGetCollections } from '~/api/v1/useGetCollections';
import { usePostCategoriesFilter } from '~/api/v1/usePostCategoriesByFilter';
import { LoadingSpinner } from '~/components';
import { useConfig } from '~/contexts';
import { useDownSm, usePageNumber } from '~/hooks';
import { FilterMerchandiseDetailsModel, MerchOrderingProps } from '~/types';
import { CollectionReadModel } from '~/types/collection';
import { useCategorieIdsByKind, useCategoriesByShowOnMainPage } from '~/utils/categories';
import { ProductsView } from '~/views';

import { StorefrontContext } from './Storefront.context';
import { StorefrontBanners } from './StorefrontBanners/StorefrontBanners';
import StorefrontFilters from './StorefrontFilters/StorefrontFilters';
import StorefrontHeader from './StorefrontHeader/StorefrontHeader';
import {
  FiltersContainer,
  GoodsContainer,
  StorefrontContainer,
  StyledButton,
  StyledItemsContainer,
  StyledSearch,
  StyledSorting,
  StyledSortingSelect,
  StyledSortingText,
} from './styled';

const SORTING_OPTIONS = [
  { label: 'Рейтинг', value: MerchOrderingProps.ByMerhRating, icon: <StarOutlined /> },
  { label: 'Количество заказов', value: MerchOrderingProps.ByOrdersCount, icon: <RiseOutlined /> },
  { label: 'Лучшие кондитеры', value: MerchOrderingProps.ByPartnerRating, icon: <BulbOutlined /> },
];

const Storefront: FC = () => {
  const [pageNumber] = usePageNumber();
  const { storefront } = useConfig();
  const isXs = useDownSm();
  const [search, setSearch] = useState('');
  const { pageSize } = storefront;
  const [filters, setFilters] = useState<Record<string, string[]>>({});
  const [sorting, setSorting] = useState(MerchOrderingProps.ByMerhRating);
  const [selectedCollection, updateSelectedCollection] = useState<undefined | CollectionReadModel>(undefined);

  const goodsFilters = useMemo(() => {
    const filtersResult: FilterMerchandiseDetailsModel = {
      pageNumber,
      pageSize,
      findingQuery: search,
      orderings: [
        {
          asc: false,
          by: sorting,
        },
      ],
      collectionId: selectedCollection?.id,
    };

    if (!selectedCollection) {
      filtersResult.categories = filters;
    }

    return filtersResult;
  }, [filters, pageNumber, pageSize, search, selectedCollection, sorting]);
  const goods = useGetGoodsByFilter(goodsFilters);
  const { data: collections } = useGetCollections();

  const allCategories = usePostCategoriesFilter({ pageNumber: 1, pageSize: 500, hideEmpty: true });
  const categories = useCategoriesByShowOnMainPage(allCategories.data?.items);
  const { t } = useTranslation();
  const selectedCollectionCategoriesByKind = useCategorieIdsByKind(selectedCollection?.categories ?? []);

  const onSearch = debounce(1000, (e: ChangeEvent<HTMLInputElement>) => {
    setSearch(e.target.value);
  });

  const onChangeFilter = useCallback(
    (data: Record<string, string[]>) => {
      setFilters(data);
    },
    [setFilters],
  );

  const isFilterSelected = useMemo(() => {
    let value = false;

    Object.keys(filters).forEach((key) => {
      if (key && filters) {
        if (filters[key].length) {
          value = true;
        }
      }
    });

    return value;
  }, [filters]);

  const onSortingChange = ({ target: { value } }: RadioChangeEvent) => {
    setSorting(value);
  };

  const onSortingChangeSelect = (value: unknown) => {
    setSorting(value as MerchOrderingProps);
  };

  const SearchComponent = useMemo(() => {
    return <StyledSearch size="large" placeholder={t('productions.find')} onChange={onSearch} defaultValue={search} />;
  }, [onSearch, search, t]);

  const Filters = useMemo(() => {
    return <StorefrontFilters value={filters} categories={categories} onChange={onChangeFilter} />;
  }, [categories, filters, onChangeFilter]);

  const handleDropCollection = () => {
    setFilters({});
  };

  useEffect(() => {
    if (Object.keys(selectedCollectionCategoriesByKind).length) {
      setFilters({ ...selectedCollectionCategoriesByKind });
    }
  }, [selectedCollectionCategoriesByKind, setFilters]);

  useEffect(() => {
    if (collections && filters) {
      const flatFilters = Object.keys(filters).reduce<string[]>((acc, kind) => {
        const filtersByKind = filters[kind];

        return [...acc, ...filtersByKind];
      }, []);

      flatFilters.sort();

      const normalizedCollectionsCategories = collections.reduce<Record<string, string[]>>((acc, value) => {
        acc[value.id] = value.categories.map((category) => {
          return category.id;
        });

        acc[value.id].sort();

        return acc;
      }, {});

      let selectedCollectionId: string | null = null;
      let selectedCollection;

      Object.keys(normalizedCollectionsCategories).forEach((collectionId) => {
        const collectionCategories = normalizedCollectionsCategories[collectionId];

        if (collectionCategories.toString() === flatFilters.toString()) {
          selectedCollectionId = collectionId;
        }
      });

      if (selectedCollectionId) {
        selectedCollection = collections.find((collection) => {
          return collection.id === selectedCollectionId;
        });
      }

      updateSelectedCollection(selectedCollection);
    }
  }, [collections, filters]);

  const providerMemo = useMemo(() => {
    return {
      selectedCollection,
      dropCollection: handleDropCollection,
      updateSelectedCollection,
    };
  }, [selectedCollection]);

  return (
    <StorefrontContext.Provider value={providerMemo}>
      <StorefrontContainer>
        {!isXs ? <FiltersContainer>{Filters}</FiltersContainer> : undefined}

        <GoodsContainer>
          <StyledItemsContainer>
            <StorefrontBanners />

            {isXs && categories ? (
              <StorefrontHeader preffixSlot={SearchComponent} isFilterSelected={isFilterSelected}>
                {Filters}
              </StorefrontHeader>
            ) : undefined}

            {!isXs ? SearchComponent : undefined}

            {!isXs ? (
              <StyledSorting
                onChange={onSortingChange}
                size={isXs ? 'small' : 'large'}
                value={sorting}
                optionType="button"
                buttonStyle="solid"
              >
                {SORTING_OPTIONS.map((sorting) => {
                  return (
                    <StyledButton key={sorting.value} value={sorting.value}>
                      {sorting.icon}
                      <StyledSortingText>{sorting.label}</StyledSortingText>
                    </StyledButton>
                  );
                })}
              </StyledSorting>
            ) : undefined}

            {isXs ? (
              <StyledSortingSelect
                suffixIcon={<OrderedListOutlined />}
                defaultValue={sorting}
                onChange={onSortingChangeSelect}
              >
                {SORTING_OPTIONS.map((sorting) => {
                  return (
                    <Select.Option key={sorting.value} value={sorting.value}>
                      {sorting.icon}
                      <StyledSortingText>{sorting.label}</StyledSortingText>
                    </Select.Option>
                  );
                })}
              </StyledSortingSelect>
            ) : undefined}

            {goods.isLoading ? (
              <LoadingSpinner height="auto" paddingLeft={2} width="auto" />
            ) : (
              <>
                <ProductsView {...goods} />
              </>
            )}
          </StyledItemsContainer>
        </GoodsContainer>
      </StorefrontContainer>
    </StorefrontContext.Provider>
  );
};

export default Storefront;
