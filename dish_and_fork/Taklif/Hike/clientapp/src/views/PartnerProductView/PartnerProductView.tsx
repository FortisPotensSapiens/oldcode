import EditIcon from '@mui/icons-material/Edit';
import { LoadingButton } from '@mui/lab';
import { Box, Button, Grid } from '@mui/material';
import { Alert } from 'antd';
import { FC, useEffect, useState } from 'react';
import { Controller, FormProvider, useForm } from 'react-hook-form';

import { usePostCategoriesFilter } from '~/api/v1/usePostCategoriesByFilter';
import { PageLayout } from '~/components';
import CategoriesSelect from '~/components/CategoriesSelect/CategoriesSelect';
import { useCurrencySymbol, useDownSm } from '~/hooks';
import { getPartnerProductsPath } from '~/routing';
import { MerchandiseCreateModel, MerchandiseReadModel, MerchandisesState } from '~/types';
import { useCategories } from '~/utils/categories';

import { FormTextField } from './FormTextField';
import { getImagePath } from './getImagePath';
import { Images } from './Images';
import { SelectedImage } from './SelectedImage';
import { ServingUnit } from './ServingUnit';
import { FormValues } from './types';
import { useDefaultValues } from './useDefaultValues';
import { useSubmit } from './useSubmit';

type PartnerProductViewProps = {
  merchandise?: Partial<MerchandiseReadModel>;
  onSubmit: (merchandise: MerchandiseCreateModel) => void;
  disabled?: boolean;
};

const PartnerProductView: FC<PartnerProductViewProps> = ({ disabled, merchandise, onSubmit }) => {
  const [selected, setSelected] = useState(0);
  const isXs = useDownSm();
  const currency = useCurrencySymbol();
  const submitHandler = useSubmit(onSubmit);
  const isNew = !merchandise?.id;
  const { data: allCategories } = usePostCategoriesFilter({ pageNumber: 1, pageSize: 500, hideEmpty: false });

  const [categories, compositionCategories] = useCategories(allCategories?.items);

  const defaultValues = useDefaultValues(merchandise);
  const methods = useForm<FormValues>({ defaultValues });

  const images = methods.watch('images');
  const path = getImagePath(images[selected]);

  useEffect(() => {
    methods.reset(defaultValues);
  }, [defaultValues, methods]);

  const onDeletePhoto = () => {
    const newSelected = selected - 1;

    images.splice(selected, 1);
    methods.setValue('images', images);

    setSelected(newSelected >= 0 ? newSelected : 0);
  };

  return (
    <PageLayout href={getPartnerProductsPath()} title={isNew ? 'Новый товар' : 'Редактирование товара'}>
      {merchandise?.state === MerchandisesState.Blocked ? (
        <>
          <Alert
            message={
              <>
                Необходимо изменить: <strong>{merchandise.reasonForBlocking}</strong>
              </>
            }
            type="error"
          />
        </>
      ) : undefined}

      <FormProvider {...methods}>
        <Box component="form" onSubmit={methods.handleSubmit(submitHandler)}>
          <Box padding={2}>
            <Grid container spacing={3}>
              <Grid item sm="auto" xs={12}>
                <Controller
                  name="images"
                  render={({ field: { ref, ...field } }) => (
                    <Images
                      {...field}
                      containerProps={
                        isXs
                          ? {
                              display: 'flex',
                              overflow: 'scroll',
                            }
                          : {
                              overflow: 'hidden',
                            }
                      }
                      disabled={disabled}
                      itemProps={
                        isXs
                          ? {
                              paddingRight: 2,
                            }
                          : {
                              paddingBottom: 2,
                            }
                      }
                      onSelect={setSelected}
                      selected={selected}
                    />
                  )}
                />
              </Grid>

              <Grid item sm xs={12}>
                <Box overflow="hidden">
                  <Grid alignItems="flex-start" container direction="row">
                    <Grid item md={6} sm={12} xs={12}>
                      <Box marginBottom={4}>
                        <Box borderRadius={2.5} overflow="hidden">
                          {path ? (
                            <SelectedImage path={path} width="100%" />
                          ) : (
                            <div
                              style={{
                                display: 'flex',
                                fontSize: '140%',
                                justifyContent: 'center',
                                alignItems: 'center',
                                height: '400px',
                                border: '1px solid rgb(0, 0, 0, 0.1)',
                                overflow: 'hidden',
                                borderRadius: '20px',
                                marginBottom: '20px',
                              }}
                            >
                              Загрузите фотографию товара
                            </div>
                          )}
                        </Box>

                        <Alert type="info" message="Рекомендуемое разрешение изображения 300x300" />

                        {images.length ? (
                          <Box display="flex" justifyContent="center" padding={2}>
                            <Button onClick={onDeletePhoto} startIcon={<EditIcon />}>
                              УДАЛИТЬ ФОТО
                            </Button>
                          </Box>
                        ) : undefined}
                      </Box>
                    </Grid>

                    <Box
                      component={Grid}
                      container
                      item
                      md
                      paddingLeft={{ md: 2 }}
                      paddingTop={1}
                      sm={12}
                      spacing={3}
                      xs={12}
                    >
                      <Grid item xs={12}>
                        <FormTextField
                          disabled={disabled}
                          label="Название товара"
                          name="title"
                          rules={{ required: true }}
                        />
                      </Grid>

                      <Grid item xs={12}>
                        <FormTextField
                          disabled={disabled}
                          label="Описание"
                          maxRows={4}
                          multiline
                          name="description"
                          rules={{ required: true }}
                        />
                      </Grid>

                      <Grid item md={6} xs={12}>
                        <ServingUnit disabled={disabled} />
                      </Grid>

                      <Grid item md={6} xs={12}>
                        <FormTextField
                          disabled={disabled}
                          InputProps={{ endAdornment: currency }}
                          label="Цена за единицу товара"
                          name="price"
                          rules={{ required: true }}
                        />
                      </Grid>

                      {categories.length ? (
                        <Grid item xs={12}>
                          <Controller
                            control={methods.control}
                            name="categories"
                            render={({ field }) => <CategoriesSelect categories={categories ?? []} {...field} />}
                          />
                        </Grid>
                      ) : undefined}

                      {compositionCategories ? (
                        <Grid item xs={12}>
                          <Controller
                            control={methods.control}
                            name="compositionCategories"
                            render={({ field }) => (
                              <CategoriesSelect categories={compositionCategories ?? []} {...field} title="Состав" />
                            )}
                          />
                        </Grid>
                      ) : undefined}

                      <Grid item md={12} xs={12}>
                        <FormTextField
                          valueAsNumber
                          disabled={disabled}
                          label="Готово к отправке"
                          name="availableQuantity"
                          rules={{ required: true }}
                        />
                      </Grid>

                      <Grid item md={12} xs={12}>
                        <FormTextField
                          valueAsFloat
                          disabled={disabled}
                          label="Вес порции брутто"
                          name="servingGrossWeightInKilograms"
                          rules={{ required: true }}
                          InputProps={{ endAdornment: 'г' }}
                        />
                      </Grid>

                      <Grid item xs={12}>
                        <LoadingButton
                          fullWidth={isXs}
                          loading={disabled}
                          size="large"
                          type="submit"
                          variant="contained"
                        >
                          {isNew ? 'Добавить товар' : 'Сохранить изменения'}
                        </LoadingButton>
                      </Grid>
                    </Box>
                  </Grid>
                </Box>
              </Grid>
            </Grid>
          </Box>
        </Box>
      </FormProvider>
    </PageLayout>
  );
};

export { PartnerProductView };
export type { PartnerProductViewProps };
