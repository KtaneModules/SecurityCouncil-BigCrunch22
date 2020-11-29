using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;

public class SecurityCouncilScript : MonoBehaviour
{
	public KMAudio Audio;
    public KMBombInfo Bomb;
    public KMBombModule Module;
	public KMBossModule Boss;
	
	public AudioClip Mallet;
	public SpriteRenderer[] FlagsImages;
	public Sprite[] AllFlags, HigherUps;
	public KMSelectable[] Buttons;
	public TextMesh ModuleName, StageNumber;
	public GameObject Earth, NonEarth;
	
	private string[] IgnoredModules;
	int ActualStage = 0;
	int MaxStage;
	bool TPCorrect = false;
	
	List<string> Attacks = new List<string>();
	int ScoreJudge = 0;
	bool StillSolving = false, FinalInput = false, AbleToBeTouched = false;
	int[] Score = {0, 0, 0};
	
	
	//Logging
    static int moduleIdCounter = 1;
    int moduleId;
    private bool ModuleSolved;

	void Awake()
	{
		moduleId = moduleIdCounter++;
		for (int x = 0; x < Buttons.Length; x++)
		{
			int Numbered = x;
            Buttons[Numbered].OnInteract += delegate
            {
                Press(Numbered);
				return false;
            };
		}
	}
	
	void Start()
	{
		if (IgnoredModules == null)
				IgnoredModules = Boss.GetIgnoredModules("Security Council", new string[]{
				"14",
				"42",
				"501",
				"A>N<D",
				"Bamboozling Time Keeper",
				"Brainf---",
				"Busy Beaver",
				"Don't Touch Anything",
				"Forget Any Color",
				"Forget Enigma",
				"Forget Everything",
				"Forget It Not",
				"Forget Me Later",
				"Forget Me Not",
				"Forget Perspective",
				"Forget The Colors",
				"Forget Them All",
				"Forget This",
				"Forget Us Not",
				"Iconic",
				"Keypad Directionality",
				"Kugelblitz",
				"Multitask",
				"OmegaDestroyer",
				"OmegaForget",
				"Organization",
				"Password Destroyer",
				"Purgatory",
				"RPS Judging",
				"Security Council",
				"Simon Forgets",
				"Simon's Stages",
				"Souvenir",
				"Tallordered Keys",
				"The Time Keeper",
				"The Troll",
				"The Twin",
				"The Very Annoying Button",
				"Timing is Everything",
				"Turn The Key",
				"Ultimate Custom Night",
				"Übermodule",
				"Whiteout"
            });
		Randomize();
		Earth.SetActive(false);
		Module.OnActivate += StartUp;
	}
	
	void Press(int Numbered)
	{
		Buttons[Numbered].AddInteractionPunch(.2f);
		Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
		if (!ModuleSolved)
		{
			if (AbleToBeTouched)
			{
				if (FinalInput)
				{
					if (Numbered == 0)
					{
						if (Score[0] >= Score[1] && Score[0] >= Score[2])
						{
							Endline();
							string[] Mocha = {"establishing peace", "deliberation", "authorizing military force"};
							Debug.LogFormat("[Security Council #{0}] The selected action was {1}.", moduleId, Mocha[Numbered]);
							Debug.LogFormat("[Security Council #{0}] The council meeting has concluded.", moduleId);
						}
						
						else
						{
							Module.HandleStrike();
							Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
							string[] Mocha = {"establishing peace", "deliberation", "authorizing military force"};
							Debug.LogFormat("[Security Council #{0}] The selected action was {1}.", moduleId, Mocha[Numbered]);
							Debug.LogFormat("[Security Council #{0}] The council is coming to a conclusion, but the action being taken is incorrect.", moduleId);
						}
					}
					
					else if (Numbered == 1)
					{
						if (Score[1] >= Score[2] && Score[1] >= Score[0])
						{
							Endline();
							string[] Mocha = {"establishing peace", "deliberation", "authorizing military force"};
							Debug.LogFormat("[Security Council #{0}] The selected action was {1}.", moduleId, Mocha[Numbered]);
							Debug.LogFormat("[Security Council #{0}] The council meeting has concluded.", moduleId);
						}
						
						else
						{
							Module.HandleStrike();
							Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
							string[] Mocha = {"establishing peace", "deliberation", "authorizing military force"};
							Debug.LogFormat("[Security Council #{0}] The selected action was {1}.", moduleId, Mocha[Numbered]);
							Debug.LogFormat("[Security Council #{0}] The council is coming to a conclusion, but the action being taken is incorrect.", moduleId);
						}
					}
					
					else if (Numbered == 2)
					{
						if (Score[2] >= Score[1] && Score[2] >= Score[0])
						{
							Endline();
							string[] Mocha = {"establishing peace", "deliberation", "authorizing military force"};
							Debug.LogFormat("[Security Council #{0}] The selected action was {1}.", moduleId, Mocha[Numbered]);
							Debug.LogFormat("[Security Council #{0}] The council meeting has concluded.", moduleId);
						}
						
						else
						{
							Module.HandleStrike();
							Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
							string[] Mocha = {"establishing peace", "deliberation", "authorizing military force"};
							Debug.LogFormat("[Security Council #{0}] The selected action was {1}.", moduleId, Mocha[Numbered]);
							Debug.LogFormat("[Security Council #{0}] The council is coming to a conclusion, but the action being taken is incorrect.", moduleId);
						}
					}
				}
				
				else
				{
					if ((Numbered == 0 && ScoreJudge < 4) || ((Numbered == 1 && ScoreJudge == 4) || (Numbered == 2 && ScoreJudge > 4)))
					{
						TPCorrect = true;
						StillSolving = false;
						ModuleName.text = "";
						Score[Numbered]++;
						if (ActualStage != MaxStage)
						{
							AbleToBeTouched = false;
							string[] Mocha = {"establishing peace", "deliberation", "authorizing military force"};
							Debug.LogFormat("[Security Council #{0}] The selected action was {1}. The action selected was right.", moduleId, Mocha[Numbered]);
						}
						
						else
						{
							StageNumber.text = "";
							HigherUps.Shuffle();
							FinalInput = true;
							for (int x = 0; x < 5; x++)
							{
								FlagsImages[x].sprite = HigherUps[x];
							}
							string[] Mocha = {"establishing peace", "deliberation", "authorizing military force"};
							Debug.LogFormat("[Security Council #{0}] The selected action was {1}. The action selected was right.", moduleId, Mocha[Numbered]);
							Debug.LogFormat("[Security Council #{0}] The council meeting is reaching its conclusion.", moduleId);
							Debug.LogFormat("[Security Council #{0}] The count for each action: Establish Peace - {1}, Deliberate - {2}, Authorize Military Force - {3}", moduleId, Score[0].ToString(), Score[1].ToString(), Score[2].ToString());
						}
					}
					
					else
					{
						Module.HandleStrike();
						string[] Mocha = {"establishing peace", "deliberation", "authorizing military force"};
						Debug.LogFormat("[Security Council #{0}] The selected action was {1}. The action selected is not the right action to take based on the count of the vote.", moduleId, Mocha[Numbered]);
					}
				}
			}
			
			else
			{
				Module.HandleStrike();
				Debug.LogFormat("[Security Council #{0}] The council was alerted to an action while no information was available.", moduleId);
				Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
			}
		}
	}
	
	void Endline()
	{
		Earth.SetActive(true);
		NonEarth.SetActive(false);
		Module.HandlePass();
		Audio.PlaySoundAtTransform(Mallet.name, transform);
		ModuleSolved = true;
		
	}
	
	void StartUp()
	{
		MaxStage = Bomb.GetSolvableModuleNames().Where(x => !IgnoredModules.Contains(x)).Count();
		if (MaxStage < 1)
		{
			Endline();
			Debug.LogFormat("[Security Council #{0}] The council has no information that can be used to take action. The council meeting has concluded.", moduleId);
		}
	}
	
	void Randomize()
	{
		AllFlags.Shuffle();
		string TheOneWhoAgreed = "Non-permanent members currently sitting on the council are: ";
		for (int x = 0; x < 5; x++)
		{
			FlagsImages[x].sprite = AllFlags[x];
			TheOneWhoAgreed += x < 5 - 1 ? AllFlags[x].name + ", " : AllFlags[x].name;
		}
		Debug.LogFormat("[Security Council #{0}] {1}", moduleId, TheOneWhoAgreed);
	}
	
	void SolveThis()
	{
		ModuleName.text = "";
		Debug.LogFormat("[Security Council #{0}] The council is making an action based on {1}", moduleId, Attacks[ActualStage-1]);
		List<string> GuideUs = new List<string>();
		string BaseCourse = "";
		string[] Guideline = Attacks[ActualStage-1].Split(' ');
		for (int x = 0; x < Guideline.Length; x++)
		{
			BaseCourse += Guideline[x];
			if (BaseCourse.Length <= 28)
			{
				if (x == (Guideline.Length - 1))
				{
					GuideUs.Add(BaseCourse);
				}
				
				else
				{
					BaseCourse += " ";
				}
			}
			
			else
			{	
				string Copper = "";
				string[] Splitter = BaseCourse.Split(' ');
				for (int a = 0; a < Splitter.Length; a++)
				{
					
					if (a == Splitter.Length - 2)
					{
						Copper += Splitter[a];
						GuideUs.Add(Copper);
					}
					
					else if (a == Splitter.Length - 1)
					{
						BaseCourse = Splitter[a] + " ";
					}
					
					else
					{
						Copper += Splitter[a] + " ";
					}
				}
				
				if (x == (Guideline.Length - 1))
				{
					GuideUs.Add(BaseCourse);
				}
			}
			
		}
		for (int a = 0; a < GuideUs.Count(); a++)
		{
			ModuleName.text += GuideUs[a];
			if (a != (GuideUs.Count() - 1))
			{
				ModuleName.text = ModuleName.text + "\n";
			}
		}
		StageNumber.text = (Int32.Parse(StageNumber.text) + 1).ToString().Length == 1 ? StageNumber.text = "0" + (Int32.Parse(StageNumber.text) + 1).ToString() : (Int32.Parse(StageNumber.text) + 1).ToString().Length == 3 ? "00" : (Int32.Parse(StageNumber.text) + 1).ToString();
		CouncilJudgement();
	}
	
	void CouncilJudgement()
	{
		ScoreJudge = 0;
		bool NoneApplied = true;
		List<string> Judgement = new List<string>();
		
		//China
		if (Int32.Parse(StageNumber.text) % 5 == Bomb.GetBatteryCount())
		{
			NoneApplied = false;
			ScoreJudge++;
			Judgement.Add("China");
		}
		
		//France
		int FranceValue = 0;
		char[] ToCompare = {'S', 'R', 'E'};
		for (int x = 0; x < ToCompare.Length; x++)
		{
			if (Attacks[ActualStage-1].ToUpper().ToCharArray().Count(c => c == ToCompare[x]) > 0)
			{
				FranceValue++;
				continue;
			}
		}
		
		if (FranceValue == 2)
		{
			NoneApplied = false;
			ScoreJudge++;
			Judgement.Add("France");
		}
		
		bool IrelandIsCheck = true;
		//UK
		string[] Mechanon = Attacks[ActualStage-1].Split(' ');
		if (Mechanon.Length == 2)
		{
			NoneApplied = false;
			ScoreJudge++;
			Judgement.Add("United Kingdom");
			IrelandIsCheck = false;
		}
		
		//US
		string Barcode = StageNumber.text;
		while (Barcode.Length != 1)
		{
			int Guide = 0;
			for (int y = 0; y < Barcode.Length; y++)
			{
				Guide += Int32.Parse(Barcode[y].ToString());
			}
			Barcode = Guide.ToString();
		}
		
		if (Int32.Parse(Barcode) == Bomb.GetSerialNumberNumbers().Last() || Int32.Parse(Barcode) == Bomb.GetSerialNumberNumbers().First())
		{
			NoneApplied = false;
			ScoreJudge++;
			Judgement.Add("United States of America");
		}
		
		//Russia
		if (NoneApplied)
		{
			ScoreJudge++;
			Judgement.Add("Russia");
		}
		
		string[] Alphabet = {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"};
		for (int z = 0; z < FlagsImages.Length; z++)
		{
			
			switch (FlagsImages[z].sprite.name)
			{
				case "Australia":
					if (Attacks[ActualStage-1][0].ToString().ToUpper() == Attacks[ActualStage-1][Attacks[ActualStage-1].Length - 1].ToString().ToUpper())
					{
						ScoreJudge++;
						Judgement.Add("Australia");
					}
					break;
				case "Barbados":
					string Basis = "";
					int BasisToBeAdd = 0;
					for (int x = 0; x < Attacks[ActualStage-1].Length; x++)
					{
						if (Attacks[ActualStage-1][x].ToString().ToUpper().EqualsAny(Alphabet))
						{
							Basis = Attacks[ActualStage-1][x].ToString().ToUpper();
							break;
						}
					}
					
					if (Basis != "")
					{
						BasisToBeAdd = Array.IndexOf(Alphabet, Basis) + 1;
					}
					
					if ((Int32.Parse(StageNumber.text) + BasisToBeAdd) % 4 == 0)
					{
						ScoreJudge++;
						Judgement.Add("Barbados");
					}
					break;
				case "Cote d'Ivoire":
					int TenNumbers = 0;
					for (int x = 0; x < Attacks[ActualStage-1].Length; x++)
					{
						if (Attacks[ActualStage-1][x].ToString().ToUpper().EqualsAny(Alphabet))
						{
							TenNumbers++;
						}
						
						if (TenNumbers > 10)
						{
							ScoreJudge++;
							Judgement.Add("Cote d'Ivoire");
							break;
						}
					}
					break;
				case "Canada":
					char[] ToSearch = {'Z', 'Q', 'X', 'J', 'V', 'K'};
					for (int x = 0; x < ToSearch.Length; x++)
					{
						if (Attacks[ActualStage-1].ToUpper().ToCharArray().Count(c => c == ToSearch[x]) > 0)
						{
							ScoreJudge++;
							Judgement.Add("Canada");
							break;
						}
					}
					break;
				case "Colombia":
					int NumberJudge = 0;
					for (int x = 0; x < Attacks[ActualStage-1].Length; x++)
					{
						if (Attacks[ActualStage-1][x].ToString().ToUpper().EqualsAny(Alphabet))
						{
							NumberJudge++;
						}
					}
					if (Int32.Parse(StageNumber.text) < NumberJudge)
					{
						ScoreJudge++;
						Judgement.Add("Colombia");
					}
					break;
				case "Ecuador":
					string[] ValidModules = {"Wire Sequence", "Wires", "Who's on First", "Simon Says", "Password", "Morse Code", "Memory", "Maze", "Keypad", "Complicated Wires", "The Button", "Not Wiresword", "Not Wire Sequence", "Not Who's on First", "Not Simaze", "Not Password", "Not Morse Code", "Not Memory", "Not Maze", "Not Keypad", "Not Complicated Wires", "Not the Button"};
					if (Attacks[ActualStage-1].EqualsAny(ValidModules))
					{
						ScoreJudge++;
						Judgement.Add("Ecuador");
					}
					break;
				case "Fiji":
					if (Attacks[ActualStage-1].ToUpper().ToCharArray().Count(c => c == 'A') == 0)
					{
						ScoreJudge++;
						Judgement.Add("Fiji");
					}
					break;
				case "Guinea":
					char[] Vowels = {'A', 'E', 'I', 'O', 'U'};
					for (int x = 0; x < Vowels.Length; x++)
					{
						if (Attacks[ActualStage-1].ToUpper().ToCharArray().Count(c => c == Vowels[x]) > 1)
						{
							break;
						}
						
						if (x == Vowels.Length - 1)
						{
							ScoreJudge++;
							Judgement.Add("Guinea");
						}
					}
					break;
				case "Ireland":
					if (IrelandIsCheck)
					{
						ScoreJudge++;
						Judgement.Add("Ireland");
					}
					break;
				case "Jamaica":
					if (Attacks[ActualStage-1].ToUpper().ToCharArray().Count(c => c == 'E') == 0)
					{
						ScoreJudge++;
						Judgement.Add("Jamaica");
					}
					break;
				case "Kazakhstan":
					if (IsPrime(Int32.Parse(StageNumber.text) + 10))
					{
						ScoreJudge++;
						Judgement.Add("Kazakhstan");
					}
					break;
				case "Liberia":
					string[] ToCycle = Attacks[ActualStage-1].ToUpper().Split(' ');
					for (int x = 0; x < ToCycle.Length; x++)
					{
						if (Regex.IsMatch(ToCycle[x], "SIMON") || Regex.IsMatch(ToCycle[x], "MORSE"))
						{
							ScoreJudge++;
							Judgement.Add("Liberia");
							break;
						}
					}	
					break;
				case "Malaysia":
					int CheckIn = 0;
					for (int x = 0; x < Attacks[ActualStage-1].Length; x++)
					{
						if (Attacks[ActualStage-1][x].ToString().ToUpper().EqualsAny(Alphabet))
						{
							CheckIn++;
						}
					}
					
					if (CheckIn < 9)
					{
						ScoreJudge++;
						Judgement.Add("Malaysia");
					}
					break;
				case "Mali":
					if (Int32.Parse(StageNumber.text).EqualsAny(0, 1, 2, 3, 4, 5, 8, 9, 10, 15, 16, 17, 24, 25, 26, 35, 36, 37, 48, 49, 50, 63, 64, 65, 80, 81, 82, 99))
					{
						ScoreJudge++;
						Judgement.Add("Mali");
					}
					break;
				case "New Zealand":
					if (Attacks[ActualStage-1].ToUpper().ToCharArray().Count(c => c == 'I') == 0)
					{
						ScoreJudge++;
						Judgement.Add("New Zealand");
					}
					break;
				case "Slovakia":
					if (Int32.Parse(StageNumber.text) % (Bomb.GetSolvableModuleNames().Count() + 1) == 0)
					{
						ScoreJudge++;
						Judgement.Add("Slovakia");
					}
					break;
				case "Slovenia":
					int Count = 0;
					char[] AnotherVowels = {'A', 'E', 'I', 'O', 'U'};
					for (int x = 0; x < AnotherVowels.Length; x++)
					{
						Count += Attacks[ActualStage-1].ToUpper().ToCharArray().Count(c => c == AnotherVowels[x]);
					}
					
					if (Count != 0 && Int32.Parse(StageNumber.text) % Count == 0)
					{
						ScoreJudge++;
						Judgement.Add("Slovenia");
					}
					break;
				case "Senegal":
					string[] ToHell = Attacks[ActualStage-1].ToUpper().Split(' ');
					for (int x = 0; x < ToHell.Length; x++)
					{
						if (Regex.IsMatch(ToHell[x], "BUTTON") || Regex.IsMatch(ToHell[x], "WIRE"))
						{
							ScoreJudge++;
							Judgement.Add("Senegal");
							break;
						}
					}	
					break;
				case "Seychelles":
					if (Int32.Parse(StageNumber.text) % 5 == 2)
					{
						ScoreJudge++;
						Judgement.Add("Seychelles");
					}
					break;
				case "Tunisia":
					int Checkout = 0;
					for (int x = 0; x < Attacks[ActualStage-1].Length; x++)
					{
						if (Attacks[ActualStage-1][x].ToString().ToUpper().EqualsAny(Alphabet))
						{
							Checkout++;
						}
					}
					if (IsPrime(Checkout))
					{
						ScoreJudge++;
						Judgement.Add("Tunisia");
					}
					break;
				case "Turkey":
					if (IsPrime((Int32.Parse(StageNumber.text) * 2) + 1) || IsPrime((Int32.Parse(StageNumber.text) * 2) - 1))
					{
						ScoreJudge++;
						Judgement.Add("Turkey");
					}
					break;
				case "Ukraine":
					string[] Chicken = Attacks[ActualStage-1].ToUpper().Split(' ');
					for (int x = 0; x < Chicken.Length; x++)
					{
						if (Regex.IsMatch(Chicken[x], "CIPHER"))
						{
							ScoreJudge++;
							Judgement.Add("Ukraine");
							break;
						}
					}
					break;
				case "Uruguay":
					string AppleSeed = "QWERTYUIOPASDFGHJKLZXCVBNM";
					char[] AppleCore = AppleSeed.ToCharArray();
					for (int x = 0; x < AppleCore.Length; x++)
					{
						if (Attacks[ActualStage-1].ToUpper().ToCharArray().Count(c => c == AppleCore[x]) > 2)
						{
							ScoreJudge++;
							Judgement.Add("Uruguay");
							break;
						}
					}
					break;
				case "Venezuela":
					if (Int32.Parse(StageNumber.text) % 7 == 0 || Int32.Parse(StageNumber.text) % 8 == 0  || Int32.Parse(StageNumber.text) % 9 == 0)
					{
						ScoreJudge++;
						Judgement.Add("Venezuela");
					}
					break;
				case "Zimbabwe":
					string[] Pheonix = Attacks[ActualStage-1].ToUpper().Split(' ');
					for (int x = 0; x < Pheonix.Length; x++)
					{
						if (Regex.IsMatch(Pheonix[x], "MAZE"))
						{
							ScoreJudge++;
							Judgement.Add("Zimbabwe");
							break;
						}
					}
					break;
				default:
					break;
			}
		}
		
		string TheOneWhoAgreed = "Members who pushed a vote: ";
		for (int x = 0; x < Judgement.Count(); x++)
		{
			TheOneWhoAgreed += x < Judgement.Count() - 1 ? Judgement[x] + ", " : Judgement[x];
		}
		Debug.LogFormat("[Security Council #{0}] {1}", moduleId, TheOneWhoAgreed);
		StillSolving = true;
	}
	
	bool IsPrime(int number)
	{
		if (number <= 1) return false;
		if (number == 2) return true;
		if (number % 2 == 0) return false;

		var boundary = (int)Math.Floor(Math.Sqrt(number));

		for (int i = 3; i <= boundary; i+=2)
			if (number % i == 0)
				return false;

		return true;        
	}

	
	void Update()
	{
		if (ActualStage < Bomb.GetSolvedModuleNames().Where(a => !IgnoredModules.Contains(a)).Count() && !ModuleSolved)
        {
			TPCorrect = false;
			CheckingForIt();
		}
	}
	
	void CheckingForIt()
	{
        var list1 = Bomb.GetSolvedModuleNames().ToList();
        //This part is for tracking unignored modules only.
		if (CanUpdateCounterNonBoss())
		{
			var list2 = list1.Where(a => !IgnoredModules.Contains(a)).ToList();
			if (list2.Count() != Attacks.Count())
			{
				foreach (String A in Attacks)
				{
					list2.Remove(A);
				}
			}
			Attacks.AddRange(list2);
		}
		ActualStage++;
		if (StillSolving)
		{
			Debug.LogFormat("[Security Council #{0}] The council was disturbed with an unnoticed arrival of new information.", moduleId);
			Module.HandleStrike();
		}
		if (ActualStage % 5 == 0)
		{
			Debug.LogFormat("[Security Council #{0}] A new set of non-permanent members are seating on the council!", moduleId);
			Randomize();
		}
		SolveThis();
		AbleToBeTouched = true;
	}
	
	bool CanUpdateCounterNonBoss()
    {
        var list1 = Bomb.GetSolvedModuleNames().Where(a => !IgnoredModules.Contains(a));
        return list1.Count() >= Attacks.Count();
    }
	
	//twitch plays
    #pragma warning disable 414
    private readonly string TwitchHelpMessage = @"To press a certain button on the module, use the command !{0} [peace/deliberate/military] (You can use the first letter of the command as a shortcut)";
    #pragma warning restore 414
    IEnumerator ProcessTwitchCommand(string command)
    {
		if (Regex.IsMatch(command, @"^\s*peace\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant) || Regex.IsMatch(command, @"^\s*p\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
			Buttons[0].OnInteract();
			if (TPCorrect)
			{
				yield return "awardpoints 1";
				TPCorrect = false;
			}
        }
		
		if (Regex.IsMatch(command, @"^\s*deliberate\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant) || Regex.IsMatch(command, @"^\s*d\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
			Buttons[1].OnInteract();
			if (TPCorrect)
			{
				yield return "awardpoints 1";
				TPCorrect = false;
			}
        }
		
		if (Regex.IsMatch(command, @"^\s*military\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant) || Regex.IsMatch(command, @"^\s*m\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
			Buttons[2].OnInteract();
			if (TPCorrect)
			{
				yield return "awardpoints 1";
				TPCorrect = false;
			}
        }
	}
}