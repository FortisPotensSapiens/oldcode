import { Box, Grid, Typography } from '@mui/material';
import { FC } from 'react';

import SweetLife from '~/assets/images/about/1.svg';
import ItHelping from '~/assets/images/about/2.svg';
import ForDesert from '~/assets/images/about/3.svg';
import { Breadcrumbs, Link, PageLayout, SupportButton, TelegramButtonLink } from '~/components';
import { getStorefrontPath } from '~/routing';

import { StyledPaper } from './StyledPaper';

const About: FC = () => (
  <>
    <Breadcrumbs>
      <Link key="root" color="text.secondary" sx={{ textDecoration: 'none' }} to={getStorefrontPath()}>
        Все изделия
      </Link>

      <Box key="title" color="text.primary">
        О нас
      </Box>
    </Breadcrumbs>

    <PageLayout height="auto" noHeaderTopPadding title="О нас">
      <Grid container spacing={{ md: 4, xs: 2 }}>
        <Grid item xs={12}>
          <StyledPaper>
            <Box
              component="img"
              display={{ sm: 'none', xs: 'block' }}
              marginX="auto"
              maxWidth={343}
              src={SweetLife}
              width={1}
            />

            <Grid columnSpacing={4} container>
              <Grid item sm={6} xs={12}>
                <Typography mt={{ md: 0, xs: 2 }} variant="h5">
                  Делаем жизнь слаще!
                </Typography>

                <Box mt={2}>
                  Мы, команда Dish&Fork, создали первый в России маркетплейс для кондитеров и любителей сладкого. Здесь
                  в поиске шикарного тортика Вы не наткнетесь на объявления о продажи шин для автомобиля или шлепанец, и
                  обязательно найдете то, что искали.
                </Box>

                <Box mt={2}>
                  Выбирайте из «меню» понравившегося кондитера или сформируйте собственный заказ, чтобы кондитеры сами
                  выходили с Вами на связь!
                </Box>
              </Grid>

              <Grid alignSelf="center" display={{ sm: 'block', xs: 'none' }} item sm={6} xs={12}>
                <Box pl={4} position="relative">
                  <Box component="img" marginX="auto" src={SweetLife} width={1} />
                </Box>
              </Grid>
            </Grid>
          </StyledPaper>
        </Grid>

        <Grid item md={6} xs={12}>
          <StyledPaper>
            <Box
              component="img"
              display="block"
              marginBottom={{ sm: 2 }}
              marginLeft={{ md: 7 }}
              marginRight={{ md: 0, sm: 7 }}
              marginX="auto"
              maxWidth={288}
              src={ItHelping}
              sx={{ float: { md: 'right', sm: 'left' } }}
              width={{ md: '30%', sm: '50%', xs: 1 }}
            />

            <Typography mt={{ md: 0, xs: 2 }} variant="h5">
              IT в помощь
            </Typography>

            <Box mt={2}>
              Платформа Dish&Fork работаетна уникальном ядре, разработанным нашими специалистами специально для нужд
              кондитеров и удобства желающих у заказать у них продукцию.
            </Box>

            <Box mt={2}>
              Благодаря такой системе мы существенно экономим Ваше на выбор и ожидание доставки заказа, а также способны
              в любой момент предоставить Вам информацию о его статусе.
            </Box>
          </StyledPaper>
        </Grid>

        <Grid item md={6} xs={12}>
          <StyledPaper>
            <Box
              component="img"
              display={{ sm: 'none', xs: 'block' }}
              marginX="auto"
              maxWidth={288}
              src={ForDesert}
              width={1}
            />

            <Grid columnSpacing={3} container>
              <Grid item sm={7} xs={12}>
                <Typography mt={{ md: 0, xs: 2 }} variant="h5">
                  И на десерт
                </Typography>

                <Box mt={2}>
                  Мы постоянно совершенствуем нашу платформу, поэтому уже в ближайшее время ожидайте нововведений,
                  которые сделают процесс заказа более увлекательным и обеспечат максимальное удовольствие от заказанной
                  продукции.
                </Box>
              </Grid>

              <Grid display={{ sm: 'block', xs: 'none' }} item sm={5}>
                <Box component="img" src={ForDesert} width={1} />
              </Grid>
            </Grid>
          </StyledPaper>
        </Grid>

        <Grid item xs={12}>
          <StyledPaper>
            <Typography mt={{ md: 0, xs: 2 }} variant="h5">
              Контакты
            </Typography>

            <Typography mb={3} mt={2}>
              Если у вас есть вопросы или вы испытываете проблемы с сервисом напишите нам.
            </Typography>

            <Grid columnSpacing={{ sm: 3, xs: 2 }} container>
              <Grid item sm="auto" xs={6}>
                <SupportButton />
              </Grid>

              <Grid item sm xs={6}>
                <TelegramButtonLink />
              </Grid>
            </Grid>
          </StyledPaper>
        </Grid>
      </Grid>
    </PageLayout>
  </>
);

export { About };
