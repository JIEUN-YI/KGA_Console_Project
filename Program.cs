namespace Day7_Task1
{
    using System;
    /*
    * 제출 전 마지막 에러 체크 후 코멘트
    * 1. 마을 뒷 산에서 몬스터의 공격을 받을 시 현재 플레이어의 체력이 증가하는 에러 발생
    *    -> 아마도 FightMonster() 함수에서 임의로 정해 사용한 공격과 방어 식에서
    *       문제가 발생한 것으로 추측하고 있습니다.
    *       다만, 식을 설정하는 방식에 대해 좋은 생각이 나지 않아서 그대로 일단 두었습니다...
    * 2. 모든 모험에서 round를 진행하는 동안 종종 진행 없이 그냥 넘어가는 현상 발생
    *    -> 모든 상황에서 발생하는 것도 아니고, 특정 상황에서 발생하는 것도 아닌 것 같습니다.\
    *       디버깅을 통해 한줄한줄 확인했을때에는 바로 넘어가지 않는 걸로 봐서는 콘솔에서
    *       조작의 미숙때문이 아닌지 의심하고 있지만, 다른 이유가 있을지도 모르겠습니다.
    * 3. 모험 진행 중 발생하는 1번 에러의 내용에서 올라간 체력이 최대체력(player.maxHp)를 넘어서서
    *    표기되는 에러
    *    -> 모험 중 player.nowHp가 player.maxHp를 넘어서는 경우 player.maxHp로 표기되는 문장을
    *       추가하면 사라질 것으로 보이지만 시간 관계상 수정하지 못하였습니다.
    */
    internal class Program
    {
        #region 기본 열거형 / 구조체 설정 - 완료
        enum Scene
        {
            // 게임의 기본 장면들
            // 초기선택방면, 방, 상점, 스케쥴 선택 장면 학교, 무술도장, 서예교실, 피아노 교실, 모험선택, 마을 뒷 산, 깊은 강가, 어두운 숲
            StartSelect, Room, Shop, SelectSchedule, School, Training, Calligraphy, Piano, Adventure, VillageMt, DeepRiver, DarkForest
        }
        enum Title
        {
            //모험지 정복 후 받는 칭호
            //추후 엔딩 분기점에 구현 예정
            villageMtCon, deepRiverCon, healingDarkForest
        }

        struct GameData
        {
            public Scene Scene;
            public PlayerData Player;
            public bool running;
            public int allDay;
            public int nowDay;
            public int sCount;
        }
        static GameData data;
        /// <summary>
        /// 플레이어 데이터
        /// </summary>
        struct PlayerData
        {
            public string name;
            public int money;
            public int maxHP;//최대체력
            public int nowHP;
            public int maxState;//모든 스테이터스의 최대
            public int damage;//공격력
            public int def;//방어력
            public int INT;//지력
            public int manner;//예법
            public int music;//음악
            public int mCount;//몬스터 승리 수
            public Title title; // 플레이어의 칭호 
            //public ItemInfo Item;
        }
        static PlayerData player;

        /// <summary>
        /// 몬스터 구조체
        /// </summary>
        struct MonsterData
        {
            public string name;
            public int maxHp;
            public int nowHP;
            public int damage;
            public int def;
            public int level;
        }
        #region 몬스터의 종류 선언
        static MonsterData herbMan;
        static MonsterData mildDeer;
        static MonsterData poorBoar;
        static MonsterData warnWolf;
        static MonsterData strongTiger;
        static MonsterData hugeToad;
        static MonsterData fierceHeron;
        static MonsterData weiredCrane;
        static MonsterData angryOtter;
        static MonsterData unknowMonster;
        static MonsterData pollutionTiger;
        #endregion

        static Random ran = new Random();
        #endregion

        /// <summary>
        /// 시작 함수 - 완료
        /// </summary> 
        static void Start()
        {
            #region 사용하는 몬스터 입력
            #region 마을 뒷 산 몬스터
            herbMan.name = "약초꾼";
            herbMan.maxHp = 20;
            herbMan.nowHP = herbMan.maxHp;
            herbMan.def = 0;
            herbMan.damage = 1;
            herbMan.level = 1;

            mildDeer.name = "순한 사슴";
            mildDeer.maxHp = 20;
            mildDeer.nowHP = mildDeer.maxHp;
            mildDeer.def = 5;
            mildDeer.damage = 5;
            mildDeer.level = 1;

            poorBoar.name = "둔한 멧돼지";
            poorBoar.maxHp = 30;
            poorBoar.nowHP = poorBoar.maxHp;
            poorBoar.def = 10;
            poorBoar.damage = 15;
            poorBoar.level = 2;

            warnWolf.name = "경계하는 늑대";
            warnWolf.maxHp = 50;
            warnWolf.nowHP = warnWolf.maxHp;
            warnWolf.def = 20; ;
            warnWolf.damage = 30;
            warnWolf.level = 3;

            strongTiger.name = "강인한 호랑이";
            strongTiger.maxHp = 60;
            strongTiger.nowHP = strongTiger.maxHp;
            strongTiger.def = 40;
            strongTiger.damage = 20;
            strongTiger.level = 5;
            #endregion
            #region 깊은 강가 몬스터

            hugeToad.name = "거대한 두꺼비";
            hugeToad.maxHp = 30;
            hugeToad.def = 5;
            hugeToad.damage = 20;
            hugeToad.level = 2;

            fierceHeron.name = "사나운 왜가리";
            fierceHeron.maxHp = 50;
            fierceHeron.def = 20;
            fierceHeron.damage = 30;
            fierceHeron.level = 3;

            weiredCrane.name = "어딘가 이상한 두루미";
            weiredCrane.maxHp = 50;
            weiredCrane.def = 20; ;
            weiredCrane.damage = 40;
            weiredCrane.level = 4;

            angryOtter.name = "분노한 수달";
            angryOtter.maxHp = 70;
            angryOtter.def = 45;
            angryOtter.damage = 30;
            angryOtter.level = 6;
            #endregion
            #region 어두운 숲 추가 몬스터

            unknowMonster.name = "정체를 알 수 없는 괴물";
            unknowMonster.maxHp = 70;
            unknowMonster.def = 45;
            unknowMonster.damage = 30;
            unknowMonster.level = 6;

            pollutionTiger.name = "오염된 산군";
            pollutionTiger.maxHp = 100;
            pollutionTiger.def = 60;
            pollutionTiger.damage = 60;
            pollutionTiger.level = 8;
            #endregion
            #endregion
            data.running = true; //게임의 종료 여부
            data.allDay = 10; // 게임의 총 일수 - 10일째 엔딩
            //data.allDay = 3; //확인용 - 3일 째 엔딩
            data.nowDay = 1; // 현재 진행 중인 일수
            data.sCount = 0; // 3번이 쌓이면 하루가 지남
            player.mCount = 0;
            player.maxState = 100; // 모든 스탯의 최대치
            #region 플레이어의 기본 스탯
            player.maxHP = 10;
            player.def = 5;
            player.damage = 5;
            player.INT = 0;
            player.manner = 0;
            player.music = 0;
            player.nowHP = player.maxHP;
            #endregion
            #region 콘솔 start 출력
            Console.Clear();
            Console.WriteLine(" =====================================");
            Console.WriteLine("∥                                     ∥");
            Console.WriteLine("∥                                     ∥");
            Console.WriteLine("∥       나만의!                       ∥");
            Console.WriteLine("∥                                     ∥");
            Console.WriteLine("∥          귀여운 용사 키우기         ∥");
            Console.WriteLine("∥                                     ∥");
            Console.WriteLine("∥                                     ∥");
            Console.WriteLine(" ===================================== ");
            Console.WriteLine();
            Console.WriteLine("     계속하려면 아무키나 누르세요      ");
            Console.ReadKey();
            #endregion
        }
        /// <summary>
        /// 종료 함수 - 추후 추가
        /// </summary>
        static void End()
        {
            EndingStory();
            Console.WriteLine("게임종료");
            Console.WriteLine(" ===================================== ");
            Console.WriteLine("추후 엔딩 추가 예정");
        }
        static void Run()
        {
            Console.Clear();
            //data.running = true 인 동안에
            while (data.running)
            {

                switch (data.Scene)
                {
                    //각 dataScene에 따라서 분기하고 관련 함수 출력
                    case Scene.StartSelect:
                        StartSelect();
                        break;
                    case Scene.Shop:
                        Shop();
                        break;
                    case Scene.SelectSchedule:
                        SelectSchedule();
                        break;
                    case Scene.Adventure:
                        Adventure();
                        break;
                    case Scene.Room:
                        Room();
                        break;
                    default:
                        break;
                }
            }
        }
        static void Main(string[] args)
        {
            Start();
            while (data.running)
            {
                Run();
            }
            data.running = false;
            End();
        }
        #region Sence
        static void StartSelect()
        {
            Console.Clear();
            Console.WriteLine(" ===================================== ");
            Console.Write(" 아이의 이름을 정해주세요 : ");
            player.name = Console.ReadLine();
            if (player.name == "")
            {
                Console.WriteLine(" 잘못된 입력입니다. ");
                return;
            }

            data.Scene = Scene.Room;

        }
        static void Room()
        {
            string select = "0";
            Console.Clear();
            Console.WriteLine(" ===================================== ");
            Console.WriteLine($" Day : {data.nowDay}");
            player.nowHP = player.maxHP;
            Wait(1);
            Console.WriteLine(" 아늑한 방이다.");
            Console.WriteLine($" {player.name}이(가) 좋아하는 물건들로 가득 차 있다.");
            Console.WriteLine(" 오늘 하루는 무엇을 할까?");
            Console.WriteLine(" ===================================== ");
            Wait(1);
            Console.WriteLine(" 1. 스케쥴 수행하기");
            Console.WriteLine(" 2. 상점가기");
            Console.WriteLine(" 3. 상태보기");
            Console.Write(" 선택 : ");
            select = Console.ReadLine();
            if (data.nowDay == data.allDay)
            {
                data.running = false;
                //진행중인 날짜와 전체 일수가 같으면 진행 중지 설정
            }
            switch (select)
            {
                case "1":
                    data.Scene = Scene.SelectSchedule;
                    return;
                case "2":
                    data.Scene = Scene.Shop;
                    break;
                case "3":
                    Console.Clear();
                    ShowState();
                    //상태를 보여주는 함수
                    break;
                default:
                    Console.WriteLine(" 잘못된 입력입니다.");
                    Wait(1);
                    break;
            }
        }
        /// <summary>
        /// 상점 함수 - 추후 업데이트 예정
        /// </summary>
        static void Shop()
        {
            Console.Clear();
            Console.WriteLine(" ===================================== ");
            Console.WriteLine(" 점원 : 상점에 오신 것을 환영합니다!   ");
            Wait(1);
            Console.WriteLine("        아직 오픈 준비중에 있습니다.   ");
            Wait(1);
            Console.WriteLine("        다음에 다시 방문해주세요!      ");
            Console.WriteLine(" ===================================== ");
            Wait(1);
            data.Scene = Scene.Room;//모든 것이 끝나고 Room 장면으로 돌아감
        }
        /// <summary>
        /// 스케쥴 선택 함수
        /// </summary>
        static void SelectSchedule()
        {
            data.sCount = 0; //스케쥴 진행 숫자
            string select = "0";
            while (data.sCount < 3)
            {
                switch (data.sCount)
                {
                    case 0:
                        Console.Clear();
                        Console.WriteLine(" ===================================== ");
                        Console.WriteLine(" 일정을 선택하세요");
                        Console.WriteLine(" 1. 학교");
                        Console.WriteLine(" 2. 무술훈련");
                        Console.WriteLine(" 3. 서예 교실");
                        Console.WriteLine(" 4. 음악 교실");
                        Console.WriteLine(" 5. 모험");
                        Console.WriteLine(" ===================================== ");
                        Console.Write(" 선택 : ");
                        select = Console.ReadLine();
                        switch (select)
                        {
                            case "1":
                                data.sCount++;
                                data.Scene = Scene.School;
                                School();
                                break;
                            case "2":
                                data.sCount++;
                                data.Scene = Scene.Training;
                                Training();
                                break;
                            case "3":
                                data.sCount++;
                                data.Scene = Scene.Calligraphy;
                                Calligraphy();
                                break;
                            case "4":
                                data.sCount++;
                                data.Scene = Scene.Piano;
                                Piano();
                                break;
                            case "5":
                                data.sCount += 4;
                                data.Scene = Scene.Adventure;
                                Adventure();
                                break;
                            default:
                                Console.WriteLine(" 잘못입력하셨습니다.");
                                break;
                        }
                        break;
                    case 1:
                    case 2:
                        Console.Clear();
                        Console.WriteLine(" ===================================== ");
                        Console.WriteLine(" 일정을 선택하세요");
                        Console.WriteLine(" 1. 학교");
                        Console.WriteLine(" 2. 무술훈련");
                        Console.WriteLine(" 3. 서예 교실");
                        Console.WriteLine(" 4. 음악 교실");
                        Console.WriteLine(" ===================================== ");
                        Console.Write(" 선택 : ");
                        select = Console.ReadLine();
                        switch (select)
                        {
                            case "1":
                                data.sCount++;
                                data.Scene = Scene.School;
                                School();
                                break;
                            case "2":
                                data.sCount++;
                                data.Scene = Scene.Training;
                                Training();
                                break;
                            case "3":
                                data.sCount++;
                                data.Scene = Scene.Calligraphy;
                                Calligraphy();
                                break;
                            case "4":
                                data.sCount++;
                                data.Scene = Scene.Piano;
                                Piano();
                                break;
                            default:
                                Console.WriteLine(" 잘못입력하셨습니다.");
                                break;
                        }
                        break;
                }
            }
            Console.Clear();
            Console.WriteLine(" ===================================== ");
            Console.WriteLine(" 하루가 저물고 있습니다.");
            Console.WriteLine(" ===================================== ");
            Wait(1);
            data.nowDay += 1; //모든 sCount의 진행 횟수 완료 후 하루 진행
            data.Scene = Scene.Room;
        }
        #region 수업스케쥴 - 제작 완료
        static void School()
        {
            Console.Clear();
            Console.WriteLine(" ===================================== ");
            Console.WriteLine(" \"와하하하!\" 아이들의 웃음소리가");
            Console.WriteLine(" 학교운동장을 가득 메우고 있다.");
            Console.WriteLine($" {player.name}도 친구에게로 달려간다.");
            Wait(2);
            Console.WriteLine($" {player.name} : \"얘들아 안녕!\"");
            Console.WriteLine(" ===================================== ");
            Wait(3);
            int reaction = ran.Next(3);
            switch (reaction)
            {
                case 0:
                    Console.Clear();
                    Console.WriteLine(" ===================================== ");
                    Console.WriteLine(" 선생님께서 국어과목을 수업중이시다.");
                    Console.WriteLine(" ===================================== ");
                    Console.WriteLine(" \"앗!\" 딴짓을 하고 말았다.");
                    Wait(3);
                    SchoolStatus();
                    data.Scene = Scene.SelectSchedule;
                    return;
                case 1:
                    Console.Clear();
                    Console.WriteLine(" ===================================== ");
                    Console.WriteLine(" 선생님께서 수학과목을 수업중이시다.");
                    Console.WriteLine(" ===================================== ");
                    Console.WriteLine(" \"Zzz...\" 조금 졸았다.");
                    Wait(3);
                    SchoolStatus();
                    data.Scene = Scene.SelectSchedule;
                    return;
                case 2:
                    Console.Clear();
                    Console.WriteLine(" ===================================== ");
                    Console.WriteLine(" 선생님께서 역사과목을 수업중이시다.");
                    Console.WriteLine(" ===================================== ");
                    Console.WriteLine(" \"+_+\" 열심히 집중해서 수업을 들었다");
                    Wait(3);
                    SchoolStatus();
                    data.Scene = Scene.SelectSchedule;
                    return;
            }

        }
        static void SchoolStatus()
        {
            if (player.INT <= player.maxState)
            {
                player.INT += 5;
            }
            if (player.maxHP <= player.maxState)
            {
                player.maxHP += 2;
            }
        }
        static void Training()
        {
            Console.Clear();
            Console.WriteLine(" ===================================== ");
            Console.WriteLine(" \"으라차찻!\" 지축을 뒤흔드는 고함소리에");
            Console.WriteLine($" {player.name}의 귀가 얼얼해졌다.");
            Console.WriteLine($" {player.name}도 큰 소리로 인사한다");
            Wait(2);
            Console.WriteLine($" {player.name} : \"잘부탁드립니다!\"");
            Console.WriteLine(" ===================================== ");
            Wait(3);
            int reaction = ran.Next(3);
            switch (reaction)
            {
                case 0:
                    Console.Clear();
                    Console.WriteLine(" ===================================== ");
                    Console.WriteLine(" 맨몸 격투술 훈련을 받았다.");
                    Console.WriteLine(" ===================================== ");
                    Console.WriteLine(" \"아얏!\" 부상을 입었다.");
                    Wait(3);
                    TrainingStatus();
                    data.Scene = Scene.SelectSchedule;
                    return;
                case 1:
                    Console.Clear();
                    Console.WriteLine(" ===================================== ");
                    Console.WriteLine(" 사범님과 모의 대련을 시작했다.");
                    Console.WriteLine(" ===================================== ");
                    Console.WriteLine(" \"흑...!\" 압도적인 실력차이였다.");
                    Wait(3);
                    TrainingStatus();
                    data.Scene = Scene.SelectSchedule;
                    return;
                case 2:
                    Console.Clear();
                    Console.WriteLine(" ===================================== ");
                    Console.WriteLine(" 동기와 대련을 시작했다.");
                    Console.WriteLine(" ===================================== ");
                    Console.WriteLine(" \"아싸!\" 대련에서 승리했다!");
                    Wait(3);
                    TrainingStatus();
                    data.Scene = Scene.SelectSchedule;
                    return;
            }
        }
        static void TrainingStatus()
        {
            if (player.maxHP <= player.maxState)
            {
                player.maxHP += 10;
            }
            if (player.damage <= player.maxState)
            {
                player.damage += 5;
            }
            if (player.def <= player.maxState)
            {
                player.def += 5;
            }
        }
        static void Calligraphy()
        {
            Console.Clear();
            Console.WriteLine(" ===================================== ");
            Console.WriteLine(" 마음이 평온해지는 고요함이 교실을");
            Console.WriteLine(" 가득 채우고 있다.");
            Console.WriteLine($" 엄격한 선생님이 가만히 {player.name}을 바라본다.");
            Wait(2);
            Console.WriteLine($" 긴장한 {player.name}의 몸이 뻣뻣해진다.");
            Console.WriteLine(" ===================================== ");
            Wait(3);
            int reaction = ran.Next(3);
            switch (reaction)
            {
                case 0:
                    Console.Clear();
                    Console.WriteLine(" ===================================== ");
                    Console.WriteLine(" 올바른 인사법을 알려주신다.");
                    Console.WriteLine(" ===================================== ");
                    Console.WriteLine(" \"흑!\" 엉망진창이라 혼났다.");
                    Wait(3);
                    CalligraphyStatus();
                    data.Scene = Scene.SelectSchedule;
                    return;
                case 1:
                    Console.Clear();
                    Console.WriteLine(" ===================================== ");
                    Console.WriteLine(" 새로운 글씨체를 쓰는 법을 배웠다.");
                    Console.WriteLine(" ===================================== ");
                    Console.WriteLine(" \"예쁘다...!\" 열심히 따라 쓰기를 연습했다.");
                    Wait(3);
                    CalligraphyStatus();
                    data.Scene = Scene.SelectSchedule;
                    return;
                case 2:
                    Console.Clear();
                    Console.WriteLine(" ===================================== ");
                    Console.WriteLine(" 완벽한 예법을 보여주셨다.");
                    Console.WriteLine(" ===================================== ");
                    Console.WriteLine("\"모두 선생님 덕분입니다.\" 완벽히 따라했다.");
                    Wait(3);
                    CalligraphyStatus();
                    data.Scene = Scene.SelectSchedule;
                    return;
            }

        }
        static void CalligraphyStatus()
        {
            if (player.manner <= player.maxState)
            {
                player.manner += 5;
            }
            if (player.INT <= player.maxState)
            {
                player.INT += 3;
            }
            if (player.music >= 3)
            {
                player.damage -= 3;
            }
            if (player.maxHP >= 2)
            {
                player.maxHP -= 2;
            }
        }
        static void Piano()
        {
            Console.Clear();
            Console.WriteLine(" ===================================== ");
            Console.WriteLine(" 아름다운 음악 선율이 흘러나온다.");
            Console.WriteLine($" {player.name}의 마음이 들뜨기 시작한다.");
            Console.WriteLine(" ===================================== ");
            Wait(3);
            int reaction = ran.Next(3);
            switch (reaction)
            {
                case 0:
                    Console.Clear();
                    Console.WriteLine(" ===================================== ");
                    Console.WriteLine(" 처음으로 보는 악보를 연주했다.");
                    Console.WriteLine(" ===================================== ");
                    Console.WriteLine("\"윽! 괴로워!\" 시끄러운 소음이었다.");
                    Wait(3);
                    PianoStatus();
                    data.Scene = Scene.SelectSchedule;
                    return;
                case 1:
                    Console.Clear();
                    Console.WriteLine(" ===================================== ");
                    Console.WriteLine(" 한 곡을 10번씩 연주하며 연습했다.");
                    Console.WriteLine(" ===================================== ");
                    Console.WriteLine("\"재미없어...\" 딴 짓을 했다.");
                    Wait(3);
                    PianoStatus();
                    data.Scene = Scene.SelectSchedule;
                    return;
                case 2:
                    Console.Clear();
                    Console.WriteLine(" ===================================== ");
                    Console.WriteLine(" 연습하던 곡을 선생님 앞에서 연주했다.");
                    Console.WriteLine(" ===================================== ");
                    Console.WriteLine("\"^^b\" 완벽한 연주였다.");
                    Wait(3);
                    PianoStatus();
                    data.Scene = Scene.SelectSchedule;
                    return;
            }

            static void PianoStatus()
            {
                if (player.music <= player.maxState)
                {
                    player.music += 5;
                }
                if (player.maxHP <= player.maxState)
                {
                    player.maxHP += 1;
                }
                if (player.manner <= 3)
                {
                    player.manner -= 3;
                }
            }
        }
        #endregion
        #region 모험스케쥴 - 추가중
        static void Adventure()
        {
            Console.Clear();
            Console.WriteLine(" ===================================== ");
            Console.WriteLine(" 새로운 모험을 떠날 생각에");
            Console.WriteLine($" {player.name}의 마음이 두근거린다.");
            Console.WriteLine(" ===================================== ");
            Wait(2);
            Console.WriteLine(" 어디로 모험을 떠날까?");
            Console.WriteLine(" 1. 마을 뒷 산");
            Console.WriteLine(" 2. 깊은 강가");
            Console.WriteLine(" 3. 어두운 숲");
            Console.Write("선택 : ");
            string select = Console.ReadLine();
            switch (select)
            {
                case "1":
                    data.Scene = Scene.VillageMt;
                    VillageMt();
                    break;
                case "2":
                    data.Scene = Scene.DeepRiver;
                    DeepRiver();
                    break;
                case "3":
                    data.Scene = Scene.DarkForest;
                    DarkForest();
                    break;
                default:
                    Console.WriteLine("잘못입력하셨습니다.");
                    Adventure();
                    break;
            }
        }
        #region 모험 - 마을 뒷 산
        static void VillageMt()
        {
            player.nowHP = player.maxHP;
            //원래는 1라운드부터 10라운드를 계획 중
            int round = 1; // 1라운드부터 5라운드까지 5라운드에는 보스전을 출력함
            Console.Clear();
            Console.WriteLine(" ===================================== ");
            Console.WriteLine(" 마을 사람들이 약초를 캐러 많이 오는");
            Console.WriteLine(" 마을의 작은 뒷산이다.");
            Console.WriteLine(" 조금 깊게 들어가면 야생동물들이 많아");
            Console.WriteLine(" 주의가 필요하다.");
            Console.WriteLine(" ===================================== ");
            Wait(4);


            while (round < 5)
            {
                Console.Clear();
                if (player.nowHP >= 0)
                {

                    Console.WriteLine(" ===================================== ");
                    Console.WriteLine(" 어디로 가볼까?");
                    Console.WriteLine(" 1. 앞으로");
                    Console.WriteLine(" 2. 오른쪽");
                    Console.WriteLine(" 3. 왼쪽");
                    Console.Write("선택 : ");
                    string select = Console.ReadLine();
                    Console.Clear();
                    int passage = ran.Next(3);
                    //랜덤으로 모험 문장 출력
                    switch (passage)
                    {
                        case 0:
                            Console.WriteLine(" ====마을 뒷 산======================= ");
                            Console.WriteLine(" 사람들이 많이 다닌 길을 따라서");
                            Console.WriteLine(" 천천히 산책하듯 걸어본다.");
                            Console.WriteLine(" 바람에 수풀이 약간 움직이는 것 같다.");
                            Console.WriteLine(" ===================================== ");
                            Wait(3);
                            EventVilMT();
                            round += 1;
                            break;
                        case 1:
                            Console.WriteLine(" ====마을 뒷 산======================= ");
                            Console.WriteLine(" 나무가 우거진 곳으로 가보자.");
                            Console.WriteLine(" 사람들이 자주 다니지는 않는지");
                            Console.WriteLine(" 길이 조금 험하다");
                            Console.WriteLine(" ===================================== ");
                            Wait(3);
                            EventVilMT();
                            round += 1;
                            break;
                        case 2:
                            Console.WriteLine(" ====마을 뒷 산======================= ");
                            Console.WriteLine(" 수풀을 파헤치며 새로운 길을 개쳑해보자");
                            Console.WriteLine(" 사람들이 다니지 않는 길이라서");
                            Console.WriteLine(" 풀과 나무가 무성하다.");
                            Console.WriteLine(" ===================================== ");
                            Wait(3);
                            EventVilMT();
                            round += 1;
                            break;
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine(" ===================================== ");
                    Console.WriteLine($" {player.name}의 체력이 0이다.");
                    Console.WriteLine("  집으로 돌아가자.......");
                    Console.WriteLine(" ===================================== ");

                    data.Scene = Scene.SelectSchedule;//Scene 전환 후 함수 이동
                    return;
                }

            }
            Console.Clear();
            Console.WriteLine(" ==== 마을 뒷 산 ===================== ");
            Console.WriteLine($" {strongTiger.name}가 나타났다.");
            bool title = FightMonster(strongTiger);
            Console.WriteLine(" ===================================== ");
            Wait(3);
            //title이 승리(true)이면 타이틀 획득하는 것 추가 계정
        }
        // VillageMt()에서 사용하는 마을 뒷 산 이벤트 출력 함수
        static void EventVilMT()
        {
            string select = "0";
            int eventNem;
            Console.Clear();

            /* 코멘트
             * 추후에는 단계별로 몬스터의 출몰이 1마리 2마리 3마리로 나오는 방향으로...
             */
            eventNem = ran.Next(8);
            switch (eventNem)
            {
                case 1: // 이벤트 발생
                    Console.Clear();
                    Console.WriteLine(" ==== 마을 뒷 산 ===================== ");
                    Console.WriteLine(" 이벤트 발생!");
                    Console.WriteLine(" 추후 업데이트 예정입니다...");
                    Console.WriteLine(" ===================================== ");
                    Wait(2);
                    Console.Clear();
                    break;
                case 2: //약초꾼 등장
                    Console.Clear();
                    Console.WriteLine(" ==== 마을 뒷 산 ===================== ");
                    Console.WriteLine($"{herbMan.name}이 등장했다");
                    Console.WriteLine($"{herbMan.name} : 여어! 약초를 캐려왔나?");
                    Console.WriteLine(" 추후 업데이트 예정입니다....");
                    Console.WriteLine(" ===================================== ");
                    Wait(2);
                    Console.Clear();
                    break;
                case 3: // 보물 상자
                    Console.Clear();
                    Console.WriteLine(" ==== 마을 뒷 산 ===================== ");
                    Console.WriteLine(" 보물상자를 발견했다!");
                    Console.WriteLine(" ===================================== ");
                    Wait(3);
                    Console.WriteLine(" 1. 열어본다.");
                    Console.WriteLine(" 2. 무시한다.");
                    Console.Write(" 선택 : ");
                    select = Console.ReadLine();
                    switch (select) // 열어보는 경우 소지금 +500
                    {
                        case "1":
                            int num = ran.Next(3);
                            switch (num)
                            {
                                case 0:
                                    Console.Clear();
                                    Console.WriteLine(" ===================================== ");
                                    Console.WriteLine(" 엄청난 보물이 나왔다~");
                                    Console.WriteLine(" +500G");
                                    Console.WriteLine(" ===================================== ");
                                    Wait(3);
                                    player.money += 500;
                                    break;
                                case 1:
                                    Console.Clear();
                                    Console.WriteLine(" ===================================== ");
                                    Console.WriteLine(" 회복 포션이 나왔다!");
                                    Console.WriteLine(" 체력 + 10");
                                    Console.WriteLine(" ===================================== ");
                                    Wait(3);
                                    player.nowHP += 10;
                                    if (player.nowHP >= player.maxHP)
                                    {
                                        player.nowHP = player.maxHP;
                                    }
                                    break;
                                case 2: // 능력치 증가 포션
                                    Console.Clear();
                                    Console.WriteLine(" ===================================== ");
                                    Console.WriteLine(" 정체를 알 수 없는 물약이 나왔다!");
                                    Console.WriteLine($" {player.name}은(는) 눈을 감고 한번에 마셨다!");
                                    Wait(2);
                                    Console.WriteLine($" 오오...! 무언가 변화하고 있다!");
                                    Console.WriteLine(" ===================================== ");
                                    Wait(3);
                                    num = ran.Next(3);
                                    switch (num)
                                    {
                                        case 0:
                                            player.INT += 10;
                                            if (player.INT >= player.maxState)
                                            {
                                                player.INT = player.maxState;
                                            }
                                            Console.WriteLine($" 지능이 10 올랐다.");
                                            Console.WriteLine(" ===================================== ");
                                            Wait(2);
                                            Console.Clear();
                                            break;
                                        case 1:
                                            player.manner += 10;
                                            if (player.manner >= player.maxState)
                                            {
                                                player.manner = player.maxState;
                                            }
                                            Console.WriteLine($" 예법이 10 올랐다.");
                                            Console.WriteLine(" ===================================== ");
                                            Wait(2);
                                            Console.Clear();
                                            break;
                                        case 2:
                                            player.music += 10;
                                            if (player.music >= player.maxState)
                                            {
                                                player.music = player.maxState;
                                            }
                                            Console.WriteLine($" 음악이 10 올랐다.");
                                            Console.WriteLine(" ===================================== ");
                                            Wait(2);
                                            Console.Clear();
                                            break;
                                    }
                                    break;
                            }

                            break;
                        default:
                            Console.Clear();
                            Console.WriteLine(" ===================================== ");
                            Console.WriteLine(" 혹시 위험할지 모르니 그냥 지나쳤다.");
                            Console.WriteLine(" 아무일도 일어나지 않았다.");
                            Console.WriteLine(" ===================================== ");
                            Wait(3);
                            break;
                    }
                    Console.Clear();
                    break;
                case 4: // 트랩 상자
                    Console.Clear();
                    Console.WriteLine(" ==== 마을 뒷 산 ===================== ");
                    Console.WriteLine(" 보물상자를 발견했다!");
                    Console.WriteLine(" ===================================== ");
                    Wait(2);
                    Console.WriteLine(" 1. 열어본다.");
                    Console.WriteLine(" 2. 무시한다.");
                    Console.Write(" 선택 : ");
                    select = Console.ReadLine();
                    switch (select) //열어보는 경우
                    {
                        case "1":
                            int num = ran.Next(2);
                            switch (num)
                            {
                                case 0: //체력 -5
                                    Console.Clear();
                                    Console.WriteLine(" ===================================== ");
                                    Console.WriteLine(" \"으악\"");
                                    Console.WriteLine("  정체를 알 수 없는 트랩이 튀어나왔다.");
                                    Console.WriteLine("  상처를 입었다.");
                                    Console.WriteLine("  체력 - 5");
                                    Console.WriteLine(" ===================================== ");
                                    player.nowHP -= 5;
                                    Wait(3);
                                    break;
                                case 1: // 능력치 감소 이벤트
                                    Console.Clear();
                                    Console.WriteLine(" ===================================== ");
                                    Console.WriteLine(" 정체를 알 수 없는 물약이 나왔다!");
                                    Console.WriteLine($" {player.name}은(는) 눈을 감고 한번에 마셨다!");
                                    Wait(3);
                                    Console.WriteLine($" 오오...! 무언가 변화하고 있다!");
                                    Console.WriteLine(" ===================================== ");
                                    Wait(3);
                                    switch (num)
                                    {
                                        case 0:
                                            player.INT -= 10;
                                            if (player.INT <= 0)
                                            {
                                                player.INT = 0;
                                            }
                                            Console.WriteLine($" 지능이 10 떨어졌다.");
                                            Console.WriteLine(" ===================================== ");
                                            Wait(2);
                                            Console.Clear();
                                            break;
                                        case 1:
                                            player.manner -= 10;
                                            if (player.manner <= 0)
                                            {
                                                player.manner = 0;
                                            }
                                            Console.WriteLine($" 예법이 10 떨어졌다.");
                                            Console.WriteLine(" ===================================== ");
                                            Wait(2);
                                            Console.Clear();
                                            break;
                                        case 2:
                                            player.music -= 10;
                                            if (player.music <= player.maxState)
                                            {
                                                player.music = 0;
                                            }
                                            Console.WriteLine($" 음악이 10 떨어졌다.");
                                            Console.WriteLine(" ===================================== ");
                                            Wait(2);
                                            Console.Clear();
                                            break;
                                    }
                                    break;
                            }
                            break;
                        default:
                            Console.Clear();
                            Console.WriteLine(" ===================================== ");
                            Console.WriteLine(" 혹시 위험할지 모르니 그냥 지나쳤다.");
                            Console.WriteLine(" 아무일도 일어나지 않았다.");
                            Console.WriteLine(" ===================================== ");
                            Wait(2);
                            break;
                    }
                    Console.Clear();
                    break;
                case 5: // 몬스터 랜덤 1마리 출몰
                case 6: // 출몰확률 증가를 위해서 케이스 수 조절
                case 7:
                    Console.Clear();
                    VillageMtMonsterEnter();
                    eventNem = 0;//스위치 문 종료를 위한 설정
                    break;

            }

        }
        // VillageMt()에서 사용하는 몬스터 등장 함수
        static void VillageMtMonsterEnter()
        {
            //string monsterName = "none";
            int monsterNum = ran.Next(3);
            switch (monsterNum)
            {
                case 0:
                    Console.WriteLine(" ==== 마을 뒷 산 ===================== ");
                    Console.WriteLine($" 전방에 {mildDeer.name}이(가) 나타났다.");
                    Console.WriteLine(" ===================================== ");
                    Wait(1);
                    FightMonster(mildDeer);
                    break;
                case 1:
                    Console.WriteLine(" ==== 마을 뒷 산 ===================== ");
                    Console.WriteLine($" 전방에 {poorBoar.name}이(가) 나타났다.");
                    Console.WriteLine(" ===================================== ");
                    FightMonster(poorBoar);
                    break;
                case 2:
                    Console.WriteLine(" ==== 마을 뒷 산 ===================== ");
                    Console.WriteLine($" 전방에 {warnWolf.name}이(가) 나타났다.");
                    Console.WriteLine(" ===================================== ");
                    FightMonster(warnWolf);
                    break;
                default:
                    break;
            }
        }
        #endregion
        #region 모험 - 깊은 강가 - 제작예정
        static void DeepRiver()
        {
            player.nowHP = player.maxHP;
            //원래는 1라운드부터 10라운드를 계획 중
            int round = 1; // 1라운드부터 5라운드까지 5라운드에는 보스전을 출력함
            Console.Clear();
            Console.WriteLine(" === 깊은 강가 ======================= ");
            Console.WriteLine(" 마을 사람들이 자주 이용하는 강가보다");
            Console.WriteLine(" 조금 더 깊은 곳이다.");
            Console.WriteLine(" 사람들이 잘 오지 않아서 수풀이 무성하다.");
            Console.WriteLine(" 꼭 무언가 튀어나올 것처럼 약간 어둡다.");
            Console.WriteLine(" ===================================== ");
            Wait(3);
            while (round < 5)
            {
                Console.Clear();
                if (player.nowHP >= 0)
                {
                    Console.WriteLine(" ===================================== ");
                    Console.WriteLine(" 어디로 가볼까?");
                    Console.WriteLine(" 1. 앞으로");
                    Console.WriteLine(" 2. 오른쪽");
                    Console.WriteLine(" 3. 왼쪽");
                    Console.Write("선택 : ");
                    string select = Console.ReadLine();
                    Console.Clear();
                    int passage = ran.Next(3);
                    //랜덤으로 모험 문장 출력
                    switch (passage)
                    {
                        case 0:
                            Console.WriteLine(" === 깊은 강가 ======================= ");
                            Console.WriteLine(" 사람들이 몇번 다닌 것 같은 길을");
                            Console.WriteLine(" 천천히 산책하듯 걸어본다.");
                            Console.WriteLine(" 바람에 수풀이 약간 움직이는 것 같다.");
                            Console.WriteLine(" ===================================== ");
                            Wait(3);
                            EventDeepRi();
                            round += 1;
                            break;
                        case 1:
                            Console.WriteLine(" === 깊은 강가 ======================= ");
                            Console.WriteLine(" 나무가 우거진 곳으로 가보자.");
                            Console.WriteLine(" 어두운 숲과 이어지는 곳이라 그런지");
                            Console.WriteLine(" 길이 조금 더 어두운 것 같다");
                            Console.WriteLine(" ===================================== ");
                            Wait(3);
                            EventDeepRi();
                            round += 1;
                            break;
                        case 2:
                            Console.WriteLine(" === 깊은 강가 ======================= ");
                            Console.WriteLine(" 수풀을 파헤치며 새로운 길을 개쳑해보자");
                            Console.WriteLine(" 강가 옆이라서 그런지");
                            Console.WriteLine(" 물소리가 세차게 들려온다.");
                            Console.WriteLine(" ===================================== ");
                            Wait(3);
                            EventDeepRi();
                            round += 1;
                            break;
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine(" ===================================== ");
                    Console.WriteLine($" {player.name}의 체력이 0이다.");
                    Console.WriteLine(" 집으로 돌아가자.......");
                    Console.WriteLine(" ===================================== ");
                    data.Scene = Scene.Room;//Scene 전환 후 함수 이동
                    Room();
                }

            }
            Console.WriteLine(" === 깊은 강가 ======================= ");
            Console.WriteLine($"{angryOtter.name}가 나타났다.");
            bool title = FightMonster(angryOtter);
            //title이 승리(true)이면 타이틀 획득하는 것 추가 계정
            Console.WriteLine(" ===================================== ");
            Wait(3);
            Console.Clear();
        }
        // DeepRiver()에서 사용하는 깊은 강가 이벤트 출력 함수 - 제작예정 
        static void EventDeepRi()
        {
            string select = "0";
            int eventNem;
            Console.Clear();
            /* 코멘트
             * 추후에는 단계별로 몬스터의 출몰이 1마리 2마리 3마리로 나오는 방향으로...
             */
            eventNem = ran.Next(8);
            switch (eventNem)
            {
                case 1: // 이벤트 발생
                    Console.Clear();
                    Console.WriteLine(" ==== 깊은 강가 ====================== ");
                    Console.WriteLine(" 이벤트 발생!");
                    Console.WriteLine(" 추후 업데이트 예정입니다...");
                    Console.WriteLine(" ===================================== ");
                    Wait(2);
                    Console.Clear();
                    break;
                case 2: // 보물 상자
                    Console.Clear();
                    Console.WriteLine(" ==== 깊은 강가 ====================== ");
                    Console.WriteLine(" 보물상자를 발견했다!");
                    Console.WriteLine(" ===================================== ");
                    Wait(2);
                    Console.WriteLine(" 1. 열어본다.");
                    Console.WriteLine(" 2. 무시한다.");
                    Console.Write(" 선택 : ");
                    select = Console.ReadLine();
                    switch (select) // 열어보는 경우 소지금 +500
                    {
                        case "1":
                            int num = ran.Next(3);
                            switch (num)
                            {
                                case 0:
                                    Console.Clear();
                                    Console.WriteLine(" ===================================== ");
                                    Console.WriteLine(" 엄청난 보물이 나왔다~");
                                    Console.WriteLine(" +500G");
                                    Console.WriteLine(" ===================================== ");
                                    Wait(3);
                                    player.money += 500;
                                    break;
                                case 1:
                                    Console.Clear();
                                    Console.WriteLine(" ===================================== ");
                                    Console.WriteLine(" 회복 포션이 나왔다!");
                                    Console.WriteLine(" 체력 + 10");
                                    Console.WriteLine(" ===================================== ");
                                    Wait(3);
                                    player.nowHP += 10;
                                    if (player.nowHP >= player.maxHP)
                                    {
                                        player.nowHP = player.maxHP;
                                    }
                                    break;
                                case 2: // 능력치 증가 포션
                                    Console.Clear();
                                    Console.WriteLine(" ===================================== ");
                                    Console.WriteLine(" 정체를 알 수 없는 물약이 나왔다!");
                                    Console.WriteLine($" {player.name}은(는) 눈을 감고 한번에 마셨다!");
                                    Wait(2);
                                    Console.WriteLine($" 오오...! 무언가 변화하고 있다!");
                                    Console.WriteLine(" ===================================== ");
                                    Wait(3);
                                    num = ran.Next(3);
                                    switch (num)
                                    {
                                        case 0:
                                            player.INT += 10;
                                            if (player.INT >= player.maxState)
                                            {
                                                player.INT = player.maxState;
                                            }
                                            Console.WriteLine($" 지능이 10 올랐다.");
                                            Console.WriteLine(" ===================================== ");
                                            Wait(2);
                                            Console.Clear();
                                            break;
                                        case 1:
                                            player.manner += 10;
                                            if (player.manner >= player.maxState)
                                            {
                                                player.manner = player.maxState;
                                            }
                                            Console.WriteLine($" 예법이 10 올랐다.");
                                            Console.WriteLine(" ===================================== ");
                                            Wait(2);
                                            Console.Clear();
                                            break;
                                        case 2:
                                            player.music += 10;
                                            if (player.music >= player.maxState)
                                            {
                                                player.music = player.maxState;
                                            }
                                            Console.WriteLine($" 음악이 10 올랐다.");
                                            Console.WriteLine(" ===================================== ");
                                            Wait(2);
                                            Console.Clear();
                                            break;
                                    }
                                    break;
                            }

                            break;
                        default:
                            Console.Clear();
                            Console.WriteLine(" ===================================== ");
                            Console.WriteLine(" 혹시 위험할지 모르니 그냥 지나쳤다.");
                            Console.WriteLine(" 아무일도 일어나지 않았다.");
                            Console.WriteLine(" ===================================== ");
                            Wait(3);
                            break;
                    }
                    Console.Clear();
                    break;
                case 3: //트랩상자
                    Console.Clear();
                    Console.WriteLine(" ==== 깊은 강가 ====================== ");
                    Console.WriteLine(" 보물상자를 발견했다!");
                    Console.WriteLine(" ===================================== ");
                    Wait(2);
                    Console.WriteLine(" 1. 열어본다.");
                    Console.WriteLine(" 2. 무시한다.");
                    Console.Write(" 선택 : ");
                    select = Console.ReadLine();
                    switch (select) //열어보는 경우
                    {
                        case "1":
                            int num = ran.Next(2);
                            switch (num)
                            {
                                case 0: //체력 -5
                                    Console.Clear();
                                    Console.WriteLine(" ===================================== ");
                                    Console.WriteLine(" \"으악\"");
                                    Console.WriteLine("  정체를 알 수 없는 트랩이 튀어나왔다.");
                                    Console.WriteLine("  상처를 입었다.");
                                    Console.WriteLine("  체력 - 5");
                                    Console.WriteLine(" ===================================== ");
                                    player.nowHP -= 5;
                                    Wait(1);
                                    break;
                                case 1: // 능력치 감소 이벤트
                                    Console.Clear();
                                    Console.WriteLine(" ===================================== ");
                                    Console.WriteLine(" 정체를 알 수 없는 물약이 나왔다!");
                                    Console.WriteLine($" {player.name}은(는) 눈을 감고 한번에 마셨다!");
                                    Wait(1);
                                    Console.WriteLine($" 오오...! 무언가 변화하고 있다!");
                                    Console.WriteLine(" ===================================== ");
                                    Wait(1);
                                    switch (num)
                                    {
                                        case 0:
                                            player.INT -= 10;
                                            if (player.INT <= 0)
                                            {
                                                player.INT = 0;
                                            }
                                            Console.WriteLine($" 지능이 10 떨어졌다.");
                                            Console.WriteLine(" ===================================== ");
                                            Wait(2);
                                            Console.Clear();
                                            break;
                                        case 1:
                                            player.manner -= 10;
                                            if (player.manner <= 0)
                                            {
                                                player.manner = 0;
                                            }
                                            Console.WriteLine($" 예법이 10 떨어졌다.");
                                            Console.WriteLine(" ===================================== ");
                                            Wait(2);
                                            Console.Clear();
                                            break;
                                        case 2:
                                            player.music -= 10;
                                            if (player.music <= player.maxState)
                                            {
                                                player.music = 0;
                                            }
                                            Console.WriteLine($" 음악이 10 떨어졌다.");
                                            Console.WriteLine(" ===================================== ");
                                            Wait(2);
                                            Console.Clear();
                                            break;
                                    }
                                    break;
                            }
                            break;
                        default:
                            Console.Clear();
                            Console.WriteLine(" ===================================== ");
                            Console.WriteLine(" 혹시 위험할지 모르니 그냥 지나쳤다.");
                            Console.WriteLine(" 아무일도 일어나지 않았다.");
                            Console.WriteLine(" ===================================== ");
                            Wait(1);
                            break;
                    }
                    Console.Clear();
                    break;
                case 4: // 몬스터 랜덤 1마리 출몰
                case 5: // 출몰확률 증가를 위해서 케이스 수 조절
                case 6:
                case 7:
                    Console.Clear();
                    DeepRiverMonsterEnter();
                    eventNem = 0;//스위치 문 종료를 위한 설정
                    break;

            }

        }
        // DeepRiver()에서 사용하는 깊은 강가 몬스터 등장 함수 -제작예정
        static void DeepRiverMonsterEnter()
        {
            int monsterNum = ran.Next(3);
            switch (monsterNum)
            {
                case 0:
                    Console.WriteLine(" ==== 깊은 강가 ====================== ");
                    Console.WriteLine($" 전방에 {hugeToad.name}이(가) 나타났다.");
                    Console.WriteLine(" ===================================== ");
                    Wait(3);
                    FightMonster(hugeToad);
                    break;
                case 1:
                    Console.WriteLine(" ==== 깊은 강가 ====================== ");
                    Console.WriteLine($" 전방에 {fierceHeron.name}이(가) 나타났다.");
                    Console.WriteLine(" ===================================== ");
                    Wait(3);
                    FightMonster(fierceHeron);
                    break;
                case 2:
                    Console.WriteLine(" ==== 깊은 강가 ====================== ");
                    Console.WriteLine($" 전방에 {weiredCrane.name}이(가) 나타났다.");
                    Console.WriteLine(" ===================================== ");
                    Wait(3);
                    FightMonster(weiredCrane);
                    break;
                default:
                    break;
            }
        }
        #endregion
        #region 모험 - 어두운 숲  -제작예정
        static void DarkForest()
        {
            Console.Clear();
            Console.WriteLine(" === 어두운 숲속 ===================== ");
            Console.WriteLine(" 추후 업데이트 예정입니다.");
            Console.WriteLine(" ===================================== ");
            Wait(3);
        }
        // DarkForest()에서 사용하는 어두운 숲 이벤트 출력 함수 - 제작예정 
        static void EventDarkFor()
        {

        }
        // DarkForest()에서 사용하는 어두운 숲 몬스터 등장 함수 -제작예정
        static void DarkForestMonsterEnter()
        {

        }
        #endregion
        #endregion

        #endregion

        #region 엔딩 분기점
        static void EndingStory()
        {
            Console.Clear();
            Console.WriteLine(" ===================================== ");
            Console.WriteLine($" {data.allDay}일 동안 열심히");
            Console.WriteLine($" {player.name}와(과) 함께한 당신");
            Console.WriteLine($" 과연 {player.name}의 운명은......");
            Console.WriteLine(" ===================================== ");
            Wait(2);
            if (player.damage >= 80 && player.def >= 80
                && player.maxHP >= 80 && player.mCount >= 20
                && player.INT >= 30 && player.INT < 60)//추후 타이틀 healingDarkForest 보유 조건 추가
            {
                Console.Clear();
                Console.WriteLine(" ===================================== ");
                Console.WriteLine($" 시간이 흘러... {player.name}는.....");
                Wait(2);
                Console.WriteLine($" 마을 사람 1 : {player.name}은 대단해!");
                Console.WriteLine($" 마을 사람 2 : 그 사람을 이 세계를 구한거야!");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine($" {player.name}는 \"전설의 용사\"가 되었다!");
                Console.WriteLine(" ===================================== ");
            }
            else if (player.damage >= 60 && player.damage < 80
                && player.def >= 60 && player.def < 80
                && player.maxHP >= 50 && player.maxHP < 80
                && player.mCount < 20 && player.mCount >= 10)//추후 타이틀 deepRiverCon 보유 조건 추가
            {
                Console.Clear();
                Console.WriteLine(" ===================================== ");
                Console.WriteLine($" 시간이 흘러... {player.name}는.....");
                Wait(2);
                Console.WriteLine($" 마을 사람 1 : 깊은 강가도 이제는 안전해");
                Console.WriteLine($" 마을 사람 2 : {player.name}가 안전하게");
                Console.WriteLine("               정비해주는거라며!");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine($" {player.name}는 \"역전의 용사\"가 되었다!");
                Console.WriteLine(" ===================================== ");
            }
            else if (player.damage >= 40 && player.damage < 60
                && player.def >= 40 && player.def < 60 && player.mCount >= 1
                && player.INT >= 30 && player.INT < 60
                && player.manner >= 30 && player.manner < 60)
            {
                Console.Clear();
                Console.WriteLine(" ===================================== ");
                Console.WriteLine($" 시간이 흘러... {player.name}는.....");
                Wait(2);
                Console.WriteLine($" 마을 사람 1 : {player.name}...? 어디선가...");
                Console.WriteLine($" 마을 사람 2 : {player.name}? 한 번은 들어본");
                Console.WriteLine("               친구야! 요즘 수련을 한다던데?");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine($" {player.name}는 \"평범한 무사\"가 되었다!");
                Console.WriteLine(" ===================================== ");
            }
            else if (player.music >= 80)
            {
                Console.Clear();
                Console.WriteLine(" ===================================== ");
                Console.WriteLine($" 시간이 흘러... {player.name}는.....");
                Wait(2);
                Console.WriteLine($" 마을 사람 1 : {player.name}! 이번에");
                Console.WriteLine("               새로운 노래를 부르던데!");
                Console.WriteLine($" 마을 사람 2 : {player.name}? 요즘 세상에");
                Console.WriteLine("               그 친구를 모르는 사람이 어디있나!");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine($" {player.name}는 \"음유시인\"이 되었다!");
                Console.WriteLine(" ===================================== ");
            }
            else if (player.INT >= 60 && player.music >= 60)
            {
                Console.Clear();
                Console.WriteLine(" ===================================== ");
                Console.WriteLine($" 시간이 흘러... {player.name}는.....");
                Wait(2);
                Console.WriteLine($" 학생 1 : 선생님! 이거 너무 어려워요!");
                Console.WriteLine($" 학생 2 : 선생님! 이 부분은 어떻게");
                Console.WriteLine("           연주해야 하나요? ");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine($" {player.name}는 \"음악교사\"가 되었다!");
                Console.WriteLine(" ===================================== ");
            }
            else if (player.INT >= 60 && player.manner >= 60)
            {
                Console.Clear();
                Console.WriteLine(" ===================================== ");
                Console.WriteLine($" 시간이 흘러... {player.name}는.....");
                Wait(2);
                Console.WriteLine($" 학생 1 : 안녕하세요. 선생님!");
                Console.WriteLine("           오늘도 잘 부탁드립니다.");
                Console.WriteLine($" 학생 2 : 헉! 그, 선생님 안녕하세요...!");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine($" {player.name}는 \"서예교사\"가 되었다!");
                Console.WriteLine(" ===================================== ");
            }
            else if (player.damage <= 30 || player.def <= 30
                || player.maxHP <= 30 || player.INT <= 30
                || player.manner <= 30 || player.music <= 60)
            {
                Console.Clear();
                Console.WriteLine(" ===================================== ");
                Console.WriteLine($" 시간이 흘러... {player.name}는.....");
                Wait(2);
                Console.WriteLine($" {player.name} : 아...오늘은 어쩌지...");
                Console.WriteLine($" {player.name} : 모험이라도 떠날까...?");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine($" {player.name}는 \"모험가\"가 되었다!");
                Console.WriteLine(" ===================================== ");
            }
        }
        #endregion



        /// <summary>
        /// 몬스터와의 전투진행 함수
        /// </summary>
        /// <param name="monster"></param>
        static bool FightMonster(MonsterData monster)
        {

            bool running = true; //계속 진행하는지 여부
            bool victory = false; //승리 여부
            monster.nowHP = monster.maxHp;
            float playerAttack = player.damage * 1.5f - monster.def;
            float monsterAttack = monster.damage - player.def * 1.5f;
            //공격력이 음수인 경우 0 처리
            if ((int)playerAttack < 0)
            {
                playerAttack = 0;
            }
            else if ((int)monsterAttack < 0)
            {
                monsterAttack = 0;
            }


            while (running)
            {
                Console.Clear();
                Console.WriteLine(" === 몬스터 정보 ===================== ");
                Console.WriteLine($" 이름 : {monster.name,+3}");
                Console.WriteLine($" 체력 : {monster.nowHP,+3}");
                Console.WriteLine(" ===================================== ");
                Console.WriteLine(" === 내 정보 ========================= ");
                Console.WriteLine($" 이름   : {player.name,+3}");
                Console.WriteLine($" 체력   : {player.nowHP,+3}");
                Console.WriteLine($" 공격력 : {player.damage,+3}");
                Console.WriteLine($" 방어력 : {player.def,+3}");
                Console.WriteLine(" ===================================== ");
                Console.WriteLine(" 1. 공격한다.");
                Console.WriteLine(" 2. 도망친다.");
                Console.WriteLine(" 3. 아이템");
                Console.Write("선택 : ");
                string select = Console.ReadLine();
                switch (select)
                {
                    /*코멘트
                     * 추후 공격확률 랜덤도 있으면 좋겠다...*/
                    case "1":
                        Console.Clear();
                        Console.WriteLine(" === 몬스터 정보 ===================== ");
                        Console.WriteLine($" 이름 : {monster.name,+3}");
                        Console.WriteLine($" 체력 : {monster.nowHP,+3}");
                        Console.WriteLine(" ===================================== ");
                        Wait(1);
                        Console.WriteLine($" {player.name}은(는) 힘껏 공격했다!");
                        Console.WriteLine($" {(int)playerAttack}의 데미지를 입혔다.");
                        monster.nowHP -= (int)playerAttack;
                        Wait(1);
                        Console.WriteLine(" ===================================== ");
                        if (monster.nowHP <= 0) //몬스터 체력 0
                        {
                            Console.WriteLine(" ===================================== ");
                            Console.WriteLine($" {player.name}은(는) {monster.name}을");
                            Console.WriteLine(" 쓰러트렸다!");
                            Console.WriteLine(" 경험치가 올랐다");
                            Console.WriteLine(" ===================================== ");
                            Wait(1);
                            player.mCount += 1;
                            victory = true;
                            return victory;
                        }
                        else if (monster.nowHP > 0)//몬스터의 체력과 플레이어의 체력이 남았을때
                        {
                            Console.Clear();
                            Console.WriteLine(" === 몬스터 정보 ===================== ");
                            Console.WriteLine($" 이름 : {monster.name,+3}");
                            Console.WriteLine($" 체력 : {monster.nowHP,+3}");
                            Console.WriteLine(" ===================================== ");
                            Wait(1);
                            Console.WriteLine($" {monster.name}이(가) 반격을 시도한다!");
                            Console.WriteLine($" {(int)monsterAttack}의 데미지를 입었다.");
                            player.nowHP -= (int)monsterAttack;
                            Wait(1);
                            Console.WriteLine(" ===================================== ");

                            if (player.nowHP <= 0) //플레이어 체력 0
                            {
                                Console.WriteLine(" ===================================== ");
                                Console.WriteLine($" {monster.name}은(는) {player.name}을");
                                Console.WriteLine(" 쓰러트렸다!");
                                Console.WriteLine($" {player.name}는 지고 말았다......");
                                Console.WriteLine(" ===================================== ");
                                Wait(1);
                                victory = false;
                                return victory;
                            }
                        }
                        break;
                    case "2":
                        int randomRun = ran.Next(1, 3);
                        if (randomRun == 1)
                        {
                            Console.WriteLine(" ===================================== ");
                            Console.WriteLine(" 도망에 실패했다!");
                            Console.WriteLine(" ===================================== ");
                            Console.WriteLine($" {monster.name}이(가) 공격을 시도한다!");
                            Console.WriteLine($"{(int)monsterAttack}의 데미지를 입었다.");
                            player.nowHP -= (int)monsterAttack;
                            Console.WriteLine(" ===================================== ");
                            Wait(1);
                        }
                        else
                        {
                            Console.WriteLine(" ===================================== ");
                            Console.WriteLine(" \"휴~\"");
                            Console.WriteLine(" 무사히 도망에 성공했다.");
                            Console.WriteLine(" ===================================== ");
                            Wait(1);
                            running = false;
                        }

                        Console.Clear();
                        return victory;
                    case "3":
                        Console.WriteLine(" ===================================== ");
                        Console.WriteLine(" 추후에 업데이트 될 예정입니다...");
                        Console.WriteLine(" ===================================== ");
                        Wait(1);
                        return victory;
                    default:
                        return victory;
                }
            }

            return victory;
        }
        static void ShowState()
        {
            #region 0 ~ 최대체력의 범위에 맞춰서 스탯 조정
            if (player.maxHP >= player.maxState)
            {
                player.maxHP = player.maxState;
            }
            else if (player.def >= player.maxState)
            {
                player.def = player.maxState;
            }
            else if (player.damage >= player.maxState)
            {
                player.damage = player.maxState;
            }
            else if (player.INT >= player.maxState)
            {
                player.INT = player.maxState;
            }
            else if (player.manner >= player.maxState)
            {
                player.manner = player.maxState;
            }
            else if (player.music >= player.maxState)
            {
                player.music = player.maxState;
            }
            if (player.maxHP <= 0)
            {
                player.maxHP = 0;
            }
            else if (player.def <= 0)
            {
                player.def = 0;
            }
            else if (player.damage <= 0)
            {
                player.damage = 0;
            }
            else if (player.INT <= 0)
            {
                player.INT = 0;
            }
            else if (player.manner <= 0)
            {
                player.manner = 0;
            }
            else if (player.music <= 0)
            {
                player.music = 0;
            }
            #endregion
            Console.WriteLine(" === 상태창 ========================== ");
            Console.WriteLine($" 체력   : {player.maxHP,+2}");
            Console.WriteLine($" 공격력 : {player.damage,+2}");
            Console.WriteLine($" 방어력 : {player.def,+2}");
            Console.WriteLine($" 지력   : {player.INT,+2}");
            Console.WriteLine($" 예법   : {player.manner,+2}");
            Console.WriteLine($" 감수성 : {player.music,+2}");
            Console.WriteLine($" 사냥한 몬스터 수 : {player.mCount,+2}");
            Console.WriteLine($" 소지금 : {player.money,+2}G");
            Console.WriteLine(" ===================================== ");
            Console.WriteLine("     돌아가려면 아무키나 누르세요      ");
            Console.ReadKey();
            data.Scene = Scene.Room;
        }
        static void Wait(float seconds)
        {
            //Thread.Sleep()받은 수 만큼 일시 멈춤
            //- msec 단위 주의
            Thread.Sleep((int)(seconds * 1000));
        }
    }
}