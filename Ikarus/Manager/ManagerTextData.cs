using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Ikarus
{
    public static partial class Manager
    {
        #region Data Loading

        #region Events
        public static SortedList<long, EventInformation> Events
        {
            get
            {
                if (_e == null)
                {
                    _e = new SortedList<long, EventInformation>();
                    LoadEvents();
                }

                return _e;
            }
        }
        private static SortedList<long, EventInformation> _e = null;
        public static bool _dictionaryChanged = false;
        private static void LoadEvents()
        {
            _e = new SortedList<long, EventInformation>();
            _e.Add(0x00010100, new EventInformation(0x00010100, "Sync Wait",
                "Pause the current flow of events until the set time is reached. Synchronous timers count down when they are reached in the code.",
                new string[] { "Frames" },
                new string[] { "The number of frames to wait." },
                "\\name(): frames=\\value(0)",
                new long[] { 1 }));
            _e.Add(0x00020000, new EventInformation(0x00020000, "Nop",
                "No action.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x00020100, new EventInformation(0x00020100, "Async Wait",
                "Pause the current flow of events until the set time is reached. Asynchronous Timers start counting from the beginning of the animation.",
                new string[] { "Frames" },
                new string[] { "The number of frames to wait." },
                "\\name(): frames=\\value(0)",
                new long[] { 1 }));
            _e.Add(0x00040100, new EventInformation(0x00040100, "Set Loop",
                "Set a loop for X iterations.",
                new string[] { "Iterations" },
                new string[] { "The number of times to loop." },
                "\\name() \\if(\\unhex(\\value(0)),==,-1, Infinite, \\unhex(\\value(0)) Times)",
                new long[] { 0 }));
            _e.Add(0x00050000, new EventInformation(0x00050000, "Execute Loop",
                "Execute the the previously set loop.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x00070100, new EventInformation(0x00070100, "Subroutine",
                "Enter the event routine specified and return after ending.",
                new string[] { "Offset" },
                new string[] { "The offset inside the file to jump to." },
                "\\name() \\value(0)",
                new long[] { 2 }));
            _e.Add(0x00080000, new EventInformation(0x00080000, "Return",
                "Return from a Subroutine.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x00090100, new EventInformation(0x00090100, "Goto",
                "Goto the event location specified and execute.",
                new string[] { "Offset" },
                new string[] { "The offset inside the file to jump to." },
                "\\name() \\value(0)",
                new long[] { 2 }));
            _e.Add(0x000A0100, new EventInformation(0x000A0100, "If",
                "Start an If block until an Else Or an EndIf is reached.",
                new string[] { "Requirement" },
                new string[] { "The form of requirement used in evaluation of the if statement." },
                "\\name() \\value(0):",
                new long[] { 6 }));
            _e.Add(0x000A0200, new EventInformation(0x000A0200, "If Value",
                "Start an If block until an Else Or an EndIf is reached.",
                new string[] { "Requirement", "Variable" },
                new string[] { "The form of requirement used in evaluation of the event.", "The variable applied to the requirement." },
                "If \\value(0): \\value(1)",
                new long[] { 6, 0 }));
            _e.Add(0x000A0400, new EventInformation(0x000A0400, "If Comparison",
                "Start an If block until an Else Or an EndIf is reached.",
                new string[] { "Requirement", "Variable", "Comparison Method", "Variable" },
                new string[] { "The form of requirement used in evaluation of the event.", "The first variable in the comparison requirement.", "The method used to compare the two variables. From 0 to 5: <, ≤, =, ≠, ≥, >", "The second variable in the comparison requirement." },
                "If \\value(0): \\value(1) \\cmpsign(\\value(2)) \\value(3)",
                new long[] { 6, 5, 0, 5 }));
            _e.Add(0x000E0000, new EventInformation(0x000E0000, "Else",
                "Insert an Else block inside an If block.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x000B0400, new EventInformation(0x000B0400, "And Comparison",
                "Seems to be an \"And\" to an If statement.",
                new string[] { "Requirement", "Variable", "Comparison Method", "Variable" },
                new string[] { "The form of requirement used in evaluation of the event.", "The first variable in the comparison requirement.", "The method used to compare the two variables. From 0 to 5: <, ≤, =, ≠, ≥, > ", "The second variable in the comparison requirement." },
                "And \\value(0): \\value(1) \\cmpsign(\\value(2)) \\value(3)",
                new long[] { 6, 5, 0, 5 }));
            _e.Add(0x000D0400, new EventInformation(0x000D0400, "Else If Comparison",
                "Insert an Else If block inside of an If block.",
                new string[] { "Requirement", "Variable", "Comparison Method", "Variable" },
                new string[] { "The form of requirement used in evaluation of the event.", "The first variable in the comparison requirement.", "The method used to compare the two variables. From 0 to 5: <, ≤, =, ≠, ≥, > ", "The second variable in the comparison requirement." },
                "Else If \\value(0): \\value(1) \\cmpsign(\\value(2)) \\value(3)",
                new long[] { 6, 5, 0, 5 }));
            _e.Add(0x000F0000, new EventInformation(0x000F0000, "End If",
                "End an If block.",
                new string[] { }, 
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x00100200, new EventInformation(0x00100200, "Switch",
                "Begin a multiple case Switch block.",
                new string[] { "IC-Basic Number", "Requirement Number" },
                new string[] { "Any type? Has shown to be an IC-Basic \"disguised\" as a value. For example, set as Value 0x3E9 but really uses IC-Basic[1001] (the equivalent).", "Any Type? Has shown to be a Requirement \"disguised\" as a value. For example, set as Value 2B but really uses 2B (Roll A Die)." },
                "\\name() (\\unhex(\\value(0)), \\value(1))",
                new long[] { 0, 0 }));
            _e.Add(0x00110100, new EventInformation(0x00110100, "Case",
                "Handler for if the variable in the switch statement equals the specified value.",
                new string[] { "Value" },
                new string[] { "The value applied to the argument." },
                "\\name() \\unhex(\\value(0)):",
                new long[] { 0 }));
            _e.Add(0x00120000, new EventInformation(0x00120000, "Default Case",
                "The case chosen if none of the others are executed.",
                new string[] { },
                new string[] { },
                "\\name():",
                new long[] { }));
            _e.Add(0x00130000, new EventInformation(0x00130000, "End Switch",
                "End a Switch block.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x01010000, new EventInformation(0x01010000, "Loop Rest",
                "Briefly return execution back to the system to prevent crashes during infinite loops.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x02000300, new EventInformation(0x02000300, "Change Action Status",
                "Change the current action upon the specified requirement being met (the requirement does not have to be met at the time this ID is executed - it can be used anytime after execution.)",
                new string[] { "Status ID", "Action", "Requirement" },
                new string[] { "The four-digit status ID of the change action event. Can later be disabled via 02080100 (Disable Action Status ID).", "The ID of the action that the character will execute.", "The form of requirement used in evaluation of the event." },
                "Prioritized Change Action: priority=\\value(0), action=\\unhex(\\value(1)), requirement=\\value(2)",
                new long[] { 0, 0, 6 }));
            _e.Add(0x02010200, new EventInformation(0x02010200, "Change Action",
                "Change the current action upon the specified requirement being met. (the requirement does not have to be met at the time this ID is executed - it can be used anytime after execution.)",
                new string[] { "Action", "Requirement" },
                new string[] { "The id of the action that the character will execute.", "The form of requirement used in evaluation of the event." },
                "\\name() action=\\unhex(\\value(0)), requirement=\\value(1)",
                new long[] { 0, 6 }));
            _e.Add(0x02010300, new EventInformation(0x02010300, "Change Action",
                "Change the current action upon the specified requirement being met. (the requirement does not have to be met at the time this ID is executed - it can be used anytime after execution.)",
                new string[] { "Action", "Requirement", "Variable" },
                new string[] { "The id of the action that the character will execute.", "The form of requirement used in evaluation of the event.", "The variable applied to the requirement." },
                "\\name() action=\\unhex(\\value(0)), requirement=\\value(1): \\value(2)",
                new long[] { 0, 6, 5 }));
            _e.Add(0x02010500, new EventInformation(0x02010500, "Change Action",
                "Change the current action upon the specified requirement being met. (the requirement does not have to be met at the time this ID is executed - it can be used anytime after execution.)",
                new string[] { "Action", "Requirement", "Variable", "Comparison Method", "Variable" },
                new string[] { "The id of the action that the character will execute.", "The form of requirement used in evaluation of the event.", "The first variable in the comparison requirement.", "The method used to compare the two variables. From 0 to 5: <, ≤, =, ≠, ≥, > ", "The second variable in the comparison requirement." },
                "\\name() action=\\unhex(\\value(0)), requirement=\\value(1): \\value(2) \\cmpsign(\\value(3)) \\value(4)",
                new long[] { 0, 6, 5, 0, 5 }));
            _e.Add(0x02040100, new EventInformation(0x02040100, "Additional Change Action Requirement",
                "Add an additional requirement to the preceeding Change Action statement.",
                new string[] { "Requirement" },
                new string[] { "The form of requirement used in evaluation of the event." },
                "\\name() \\value(0)",
                new long[] { 6 }));
            _e.Add(0x02040200, new EventInformation(0x02040200, "Additional Change Action Requirement Value",
                "Add an additional requirement to the preceeding Change Action statement.",
                new string[] { "Requirement", "Variable" },
                new string[] { "The form of requirement used in evaluation of the event.", "The variable applied to the requirement." },
                "\\name() \\value(0): \\value(1)",
                new long[] { 6, 5 }));
            _e.Add(0x02040400, new EventInformation(0x02040400, "Additional Change Action Requirement Comparison",
                "Add an additional requirement to the preceeding Change Action statement.",
                new string[] { "Requirement", "Variable", "Comparison Method", "Variable" },
                new string[] { "The form of requirement used in evaluation of the event.", "The first variable in the comparison requirement.", "The method used to compare the two variables. From 0 to 5: <, ≤, =, ≠, ≥, > ", "The second variable in the comparison requirement." },
                "\\name() \\value(0): \\value(1) \\cmpsign(\\value(2)) \\value(3)",
                new long[] { 6, 5, 0, 5 }));
            _e.Add(0x02060100, new EventInformation(0x02060100, "Enable Action Status ID",
                "Enables the given Status ID.",
                new string[] { "Status ID" },
                new string[] { "The Status ID to enable." },
                "\\name(): \\unhex(\\value(0))",
                new long[] { 0 }));
            _e.Add(0x04000100, new EventInformation(0x04000100, "Change Subaction",
                "Change the current subaction.",
                new string[] { "Subaction" },
                new string[] { "The ID of the subaction that the character will execute." },
                "\\name(): sub action=\\value(0)",
                new long[] { 0 }));
            _e.Add(0x04000200, new EventInformation(0x04000200, "Change Subaction",
                "Change the current subaction. Specifies whether or not to pass the current frame or start the animation over.",
                new string[] { "Subaction", "Pass Frame" },
                new string[] { "The ID of the subaction that the character will execute.", "Whether to pass the current frame of the current animation onto the new animation or not." },
                "\\name(): sub action=\\value(0), pass frame=\\value(1)",
                new long[] { 0, 3 }));
            _e.Add(0x05000000, new EventInformation(0x05000000, "Reverse Direction",
                "Reverse the direction the character is facing after the animation ends.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x06000D00, new EventInformation(0x06000D00, "Offensive Collision",
                "Generate an offensive collision bubble with the specified parameters.",
                new string[] { "Bone/Id", "Damage", "Trajectory", "Weight Knockback/Knockback Growth", "Shield Damage/Base Knockback", "Size", "X Offset", "Y Offset", "Z Offset", "Tripping Rate", "Hitlag Multiplier", "Directional Influence Multiplier", "Flags" },
                new string[] { "Value1 = The bone the collision bubble is attached to. Value2 = The id number of the collision bubble.", "The amount of damage inflicted to the target upon collision. ", "The direction in which a target gets launched.", "Value1 = The distance the target is launched proportional to weight for fixed knockback hits. Value2 = The additional distance the target is launched proportional to its damage (launch force for fixed knockback hits).", "Value1 = The amount of damage dealt to the target's shield if it is up. Value2 = The distance the target is launched regardless of its damage (zero for fixed knockback hits).", "The size of the collision bubble.", "The amount the collision bubble is transitioned relative to the currently attached bone.", "The amount the collision bubble is transitioned relative to the currently attached bone.", "The amount the collision bubble is transitioned relative to the currently attached bone.", "The percent possibility of the collision bubble inducing a trip, provided the target doesn't leave the ground from the knockback.", "A multiplier affecting the time in which both parties pause when the collision bubble connects.", "A multiplier affecting the ability for the character to maneuver themselves while suffering from the hitlag generated by this collision bubble.", "Flags for various parameters such as hit effects and sound effects." },
                "\\name(): Id=\\unhex(\\half2(\\value(0))), Bone=\\bone(\\half1(\\value(0))), Damage=\\unhex(\\value(1)), ShieldDamage=\\unhex(\\half1(\\value(4))), Direction=\\unhex(\\value(2)), BaseKnockback=\\unhex(\\half2(\\value(4))), WeightKnockback=\\unhex(\\half1(\\value(3))), KnockbackGrowth=\\unhex(\\half2(\\value(3))), Size=\\value(5), Z Offset=\\value(6), Y Offset=\\value(7), X Offset=\\value(8), TripRate=\\value(9)%, HitlagMultiplier=x\\value(10), SDIMultiplier=x\\value(11), Flags=\\hex8(\\unhex(\\value(12)))",
                new long[] { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0 }));
            _e.Add(0x06040000, new EventInformation(0x06040000, "Terminate Collisions",
                "Remove all currently present collision bubbles",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x06050100, new EventInformation(0x06050100, "Body Collision",
                "Change how the character's own collision bubbles act.",
                new string[] { "State" },
                new string[] { "0 = normal, 1 = invincible, 2 = intangible, 3 = intangible (no flashing), 4 = intangible (quick flashing)" },
                "\\name(): status=\\collision(\\value(0))",
                new long[] { 0 }));
            _e.Add(0x06080200, new EventInformation(0x06080200, "Bone Collision",
                "Sets specific bones to a type of body collision.",
                new string[] { "Bone", "State" },
                new string[] { "The bone to be affected.", "The type of body collision. 0 = normal, 1 = invincible, 2 = intangible, 3 = intangible (no flashing), 4 = intangible (quick flashing)" },
                "\\name(): bone=\\bone(\\value(0)), status=\\collision(\\value(1))",
                new long[] { }));
            _e.Add(0x06060100, new EventInformation(0x06060100, "Undo Bone Collision",
                "Sets bones to their normal collision type.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x060A0800, new EventInformation(0x060A0800, "Catch Collision",
                "Generate a grabbing collision bubble with the specified parameters",
                new string[] { "ID", "Bone", "Scale", "X offset", "Y Offset", "Z Offset", "Action", "Air/Ground" },
                new string[] { "ID of catch collision.", "The bone the grab is attached to.", "The size of the catch collision bubble.", "Transition relative to the currently attached bone.", "Transition relative to the currently attached bone.", "Transition relative to the currently attached bone.", "The Action ID that the foe executes if successfully grabbed.", "0 = grabs nothing, 1 = grabs grounded only, 2 = grabs aerial only, 3 = grabs aerial and grounded." },
                "\\name(): ID=\\value(0), Bone=\\bone(\\value(1)), Scale=\\value(2), Offset=(\\value(3), \\value(4), \\value(5)), Action=\\unhex(\\value(6)), Type=\\value(7)",
                new long[] { 0, 0, 1, 1, 1, 1, 0, 0 }));
            _e.Add(0x060D0000, new EventInformation(0x060D0000, "Terminate Catch Collisions",
                "Remove all currently present grab collision bubbles",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x060E1100, new EventInformation(0x060E1100, "Throw Specifier",
                "Specify the properties of the throw to be used when 060F0500 is executed. Used for other things as well, such as some Final Smashes.",
                new string[] { "ID", "Bone?", "Damage", "Trajectory", "Knockback Growth", "Weight Knockback", "Base Knockback", "Effect", "Undefined", "Undefined", "Undefined", "Undefined", "SFX", "Air/Ground", "Undefined", "Undefined", "Invincibility Frames?" },
                new string[] { "ID of throw data. Seemingly, a \"0\" indicates this is the throw data, while a \"1\" indicates this is used if the opponent escapes during the throw. \"2\" has also been seen (by Light Arrow).", "Possibly bone used by collision.", "The amount of damage inflicted to the target on throw.", "The direction in which the target gets launched.", "The additional distance the target is launched proportional to its damage.", "The distance the target is launched proportional to weight. Set to non-zero values only for fixed knockback throws.", "The distance the target is launched regardless of its damage. Set to zero for fixed knockback throws.", "The effect of the throw. See the [[Hitbox Flags (Brawl)#Bits 28-32 (Effect)", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Sound effect played upon throw.", "0 = Never Grabs, 1 = Only Grabs Grounded Foes, 2 = Only Grabs Aerial Foes, 3= Grabs Aerial and Grounded Foes.", "Undefined.", "Undefined.", "The number of invincibility frames the thrower gains when this command is executed?" },
                "\\name():ID=\\value(0), Bone?=\\value(1), Damage=\\unhex(\\value(2)), Direction=\\unhex(\\value(3)), KnockbackGrowth=\\unhex(\\value(4)), WeightKnockback=\\unhex(\\value(5)),BaseKnockback=\\unhex(\\value(6)), Element=\\value(7), UnknownA=\\value(8), UnknownB=\\value(9), UnknownC=\\value(10), UnknownD=\\value(11), SFX=\\value(12), Direction?=\\value(13), UnknownE=\\value(14), UnknownF=\\value(15), UnknownG=\\value(16)",
                new long[] { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 3, 3, 0 }));
            _e.Add(0x060F0500, new EventInformation(0x060F0500, "Throw Applier",
                "Throws an opponent based on data provided by 060E1100 (Throw Specifier).",
                new string[] { "ID?", "Bone", "Undefined", "Undefined", "Undefined" },
                new string[] { "Undefined.", "Appears to always be the bone the thrown character is attached to.", "Undefined.", "Undefined.", "Undefined" },
                "",
                new long[] { 0, 0, 5, 5, 5 }));
            _e.Add(0x06150F00, new EventInformation(0x06150F00, "Special Offensive Collision",
                "Generate an offensive collision bubble - is able to achieve unique effects.",
                new string[] { "Bone/Id", "Damage", "Trajectory", "Weight Knockback/Knockback Growth", "Shield Damage/Base Knockback", "Size", "X Offset", "Y Offset", "Z Offset", "Tripping Rate", "Hitlag Multiplier", "Directional Influence Multiplier", "Flags", "Rehit Rate", "Special Flags" },
                new string[] { "Value1 = The bone the collision bubble is attached to. Value2 = The id number of the collision bubble.", "The amount of damage inflicted to the target upon collision. ", "The direction in which a target gets launched.", "Value1 = The distance the target is launched proportional to weight for fixed knockback hits. Value2 = The additional distance the target is launched proportional to its damage (launch force for fixed knockback hits).", "Value1 = The amount of damage dealt to the target's shield if it is up. Value2 = The distance the target is launched regardless of its damage (zero for fixed knockback hits).", "The size of the collision bubble.", "Transition relative to the currently attached bone.", "Transition relative to the currently attached bone.", "Transition relative to the currently attached bone.", "The percent possibility of the collision bubble inducing a trip, provided the target doesn't leave the ground from the knockback.", "A multiplier affecting the time in which both parties pause when the collision bubble connects.", "A multiplier affecting the ability for the character maneuver themselves while suffering from the hitlag generated by this collision bubble.", "Flags for various parameters such as hit effects and sound effects.", "How many frames between each hitbox refresh; for example a value of 8 will cause the hitbox to hit every 9 frames. A value of 0 disables refreshing; the hitbox will only hit once.", "Flags for hitbox type and attributes such as susceptibility to reflection and absorption." },
                "\\name(): Id=\\unhex(\\half2(\\value(0))), Bone=\\bone(\\half1(\\value(0))), Damage=\\unhex(\\value(1)), ShieldDamage=\\unhex(\\half1(\\value(4))), Direction=\\unhex(\\value(2)), BaseKnockback=\\unhex(\\half2(\\value(4))), WeightKnockback=\\unhex(\\half1(\\value(3))), KnockbackGrowth=\\unhex(\\half2(\\value(3))), Size=\\value(5), Z Offset=\\value(6), Y Offset=\\value(7), X Offset=\\value(8), TripRate=\\value(9)%, HitlagMultiplier=x\\value(10), SDIMultiplier=x\\value(11), Flags=\\hex8(\\unhex(\\value(12)))",
                new long[] { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0 }));
            _e.Add(0x0C0D0F00, new EventInformation(0x0C0D0F00, "Character Specific Special Offensive Collision",
                "Generate an offensive collision bubble - is able to achieve unique effects.",
                new string[] { "Undefined", "Damage", "Trajectory", "Weight Knockback/Knockback Growth", "Shield Damage/Base Knockback", "Size", "X Offset", "Y Offset", "Z Offset", "Tripping Rate", "Hitlag Multiplier", "Directional Influence Multiplier", "Flags", "Rehit Rate", "Special Flags" },
                new string[] { "Unknown.", "The amount of damage inflicted to the target upon collision. ", "The direction in which a target gets launched.", "Value1 = The distance the target is launched proportional to weight for fixed knockback hits. Value2 = The additional distance the target is launched proportional to its damage (launch force for fixed knockback hits).", "Value1 = The amount of damage dealt to the target's shield if it is up. Value2 = The distance the target is launched regardless of its damage (zero for fixed knockback hits).", "The size of the collision bubble.", "Transition relative to the currently attached bone.", "Transition relative to the currently attached bone.", "Transition relative to the currently attached bone.", "The percent possibility of the collision bubble inducing a trip, provided the target doesn't leave the ground from the knockback.", "A multiplier affecting the time in which both parties pause when the collision bubble connects.", "A multiplier affecting the ability for the character maneuver themselves while suffering from the hitlag generated by this collision bubble.", "Flags for various parameters such as hit effects and sound effects.", "How many frames between each hitbox refresh; for example a value of 8 will cause the hitbox to hit every 9 frames. A value of 0 disables refreshing; the hitbox will only hit once.", "Flags for hitbox type and attributes such as susceptibility to reflection and absorption." },
                "\\name(): Id=\\unhex(\\half2(\\value(0))), Bone=\\bone(\\half1(\\value(0))), Damage=\\unhex(\\value(1)), ShieldDamage=\\unhex(\\half1(\\value(4))), Direction=\\unhex(\\value(2)), BaseKnockback=\\unhex(\\half2(\\value(4))), WeightKnockback=\\unhex(\\half1(\\value(3))), KnockbackGrowth=\\unhex(\\half2(\\value(3))), Size=\\value(5), Z Offset=\\value(6), Y Offset=\\value(7), X Offset=\\value(8), TripRate=\\value(9)%, HitlagMultiplier=x\\value(10), SDIMultiplier=x\\value(11), Flags=\\hex8(\\unhex(\\value(12)))",
                new long[] { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0 }));
            _e.Add(0x06170300, new EventInformation(0x06170300, "Defensive Collision",
                "Generate a defensive collision bubble.",
                new string[] { "Undefined", "Undefined", "Undefined" },
                new string[] { "Undefined.", "Undefined.", "Undefined" },
                "",
                new long[] { 0, 0, 0 }));
            _e.Add(0x06180300, new EventInformation(0x06180300, "Terminate Defensive Collision",
                "Removes defensive collisions.",
                new string[] { "Undefined", "Undefined", "Undefined" },
                new string[] { "Undefined.", "Undefined.", "Undefined" },
                "",
                new long[] { 0, 0, 0 }));
            _e.Add(0x061B0500, new EventInformation(0x061B0500, "Move Hitbox",
                "Repositions an already-existing hitbox.",
                new string[] { "Hitbox ID", "New Bone", "New X Offset", "New Y Offset", "New Z Offset" },
                new string[] { "The ID of the hitbox to modify.", "The ID of the bone to attach to.", "The new X translation.", "The new Y translation.", "The new Z translation" },
                "",
                new long[] { 0, 0, 1, 1, 1 }));
            _e.Add(0x062B0D00, new EventInformation(0x062B0D00, "Thrown Collision",
                "Generate a damage collision bubble surrounding the character being thrown.",
                new string[] { "Bone/Id", "Damage", "Trajectory", "Weight Knockback/Knockback Growth", "Shield Damage/Base Knockback", "Size", "X Offset", "Y Offset", "Z Offset", "Tripping Rate", "Hitlag Multiplier", "Directional Influence Multiplier", "Flags" },
                new string[] { "The bone the collision bubble is attached to/The id number of the collision bubble. Where XXXXYYYY is X=Bone, Y=Id.", "The amount of damage inflicted to the target upon collision.", "The direction in which a target gets launched.", "The distance the target is launched proportional to weight for fixed knockback hits/The additional distance the target is launched proportional to its damage (launch force for fixed knockback hits). XXXXYYYY is X=Weight Knockback, Y=Knockback Growth.", "The amount of damage dealt to the target's shield if it is up/The distance the target is launched regardless of its damage (zero for fixed knockback hits). XXXXYYYY is X=Shield Damage, Y=Base Knockback.", "The size of the collision bubble.", "The amount the collision bubble is transitioned relative to the currently attached bone.", "The amount the collision bubble is transitioned relative to the currently attached bone.", "The amount the collision bubble is transitioned relative to the currently attached bone.", "The percent possibility of the collision bubble inducing a trip, proving the target doesn't leave the ground from the knockback.", "A multiplier affecting the time in which both parties pause when the collision bubble connects.", "A multiplier affecting the ability for the character maneuver themselves while suffering from the hitlag generated by this collision bubble.", "Flags for various parameters such as hit effects and sound effects." },
                "",
                new long[] { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0 }));
            _e.Add(0x0A000100, new EventInformation(0x0A000100, "Sound Effect",
                "Play a specified sound effect.",
                new string[] { "Sound Effect" },
                new string[] { "The ID number for the sound effect called." },
                "\\name(): \\value(0)",
                new long[] { 0 }));
            _e.Add(0x0A010100, new EventInformation(0x0A010100, "Sound Effect 2",
                "Play a specified sound effect.",
                new string[] { "Sound Effect" },
                new string[] { "The ID number for the sound effect called." },
                "\\name(): \\value(0)",
                new long[] { 0 }));
            _e.Add(0x0A020100, new EventInformation(0x0A020100, "Sound Effect (Transient)",
                "Play a specified sound effect. The sound effect ends with the animation.",
                new string[] { "Sound Effect" },
                new string[] { "The ID number for the sound effect called." },
                "\\name(): \\value(0)",
                new long[] { 0 }));
            _e.Add(0x0A050100, new EventInformation(0x0A050100, "Sounds 05",
                "Is used during victory poses.",
                new string[] { "Undefined" },
                new string[] { "Undefined" },
                "",
                new long[] { 0 }));
            _e.Add(0x0A070100, new EventInformation(0x0A070100, "Sounds 07",
                "Undefined.",
                new string[] { "Undefined" },
                new string[] { "Undefined" },
                "",
                new long[] { 0 }));
            _e.Add(0x0A090100, new EventInformation(0x0A090100, "Other Sound Effect 1",
                "Play a specified sound effect.",
                new string[] { "Sound Effect" },
                new string[] { "The ID number of the sound effect to be called." },
                "\\name(): \\value(0)",
                new long[] { 0 }));
            _e.Add(0x0A0A0100, new EventInformation(0x0A0A0100, "Other Sound Effect 2",
                "Play a specified sound effect.",
                new string[] { "Sound Effect" },
                new string[] { "The ID number of the sound effect to be called." },
                "\\name(): \\value(0)",
                new long[] { 0 }));
            _e.Add(0x0A030100, new EventInformation(0x0A030100, "Stop Sound Effect",
                "Stops the specified sound effect immediately.",
                new string[] { "Sound Effect" },
                new string[] { "The ID number of the sound effect to be called." },
                "\\name(): \\value(0)",
                new long[] { 0 }));
            _e.Add(0x0C050000, new EventInformation(0x0C050000, "Terminate Instance",
                "Causes the acting instance to terminate (if possible). Will load secondary instance if available.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x0C0B0000, new EventInformation(0x0C0B0000, "Low Voice Clip",
                "Play a random voice clip from the selection of low voice clips.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x0C190000, new EventInformation(0x0C190000, "Damage Voice Clip",
                "Play a random voice clip from the selection of damage voice clips.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x0C1D0000, new EventInformation(0x0C1D0000, "Ottotto Voice Clip",
                "Play the voice clip for Ottotto.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x04070100, new EventInformation(0x04070100, "Frame Speed Modifier",
                "Dictates the frame speed of the subaction. Example: setting to 2 makes the animation and timers occur twice as fast.",
                new string[] { "Multiplier" },
                new string[] { "How many times faster the frames are." },
                "\\name(): Multiplier=\\value(0)x",
                new long[] { 1 }));
            _e.Add(0x0C230200, new EventInformation(0x0C230200, "Time Manipulation",
                "Change the speed of time for various parts of the environment.",
                new string[] { "Multiplier", "Frames" },
                new string[] { "How many times faster the frames are.", "How long the time is multiplied." },
                "\\name(): Amount=\\value(0), Frames=\\value(0)",
                new long[] { 0, 0 }));
            _e.Add(0x0E000100, new EventInformation(0x0E000100, "Set Air/Ground",
                "Specify whether the character is on or off the ground.",
                new string[] { "State" },
                new string[] { "The state of the character's air/ground status. 0 = In Air, 1 = On Ground" },
                "\\name(): \\airground(\\value(0))",
                new long[] { 0 }));
            _e.Add(0x08000100, new EventInformation(0x08000100, "Set Edge Slide",
                "Determines whether or not the character will slide off the edge.",
                new string[] { "Character State" },
                new string[] { "1: Can drop off side of stage.  2: Can't drop off side of stage.  5: Treated as in air; can leave stage vertically.  Other states currently unknown." },
                "\\name(): \\enum(\\value(0), 0)",
                new long[] { 0 }) { _enums = new Dictionary<int, List<string>>() { { 0, new List<string>() { "Undefined(0)", "Can drop off side of stage", "Can't drop off side of stage", "Undefined(3)", "Undefined(4)", "In Air; Can leave stage vertically", } } } });
            _e.Add(0x10000100, new EventInformation(0x10000100, "Generate Article",
                "Generate a pre-made prop effect from the prop library.",
                new string[] { "Article ID" },
                new string[] { "The id of the prop article to be called." },
                "\\name(): \\value(0)",
                new long[] { 0 }));
            _e.Add(0x10030100, new EventInformation(0x10030100, "Remove Article",
                "Removes an article.",
                new string[] { "Article" },
                new string[] { "ID of the article to be affected." },
                "\\name(): \\value(0)",
                new long[] { 0 }));
            _e.Add(0x10050200, new EventInformation(0x10050200, "Article Visibility",
                "Makes an article visible or invisible.",
                new string[] { "Article", "Visibility" },
                new string[] { "ID of the article to be affected.", "Set Boolean: True = Visible, False = Invisible" },
                "\\name(): Article ID=\\value(0), Visible=\\value(1)",
                new long[] { 0, 3 }));
            _e.Add(0x100A0000, new EventInformation(0x100A0000, "Generate Prop Effect",
                "Generate a prop effect with the specified parameters.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x11001000, new EventInformation(0x11001000, "External Graphic Effect",
                "Generate a generic graphical effect with the specified parameters.",
                new string[] { "Graphic", "Bone", "Z Offset", "Y Offset", "X Offset", "Z Rotation", "Y Rotation", "X Rotation", "Scale", "Random Z Offset", "Random Y Offset", "Random Z Offset", "Random Z Rotation", "Random Y Rotation", "Random X Rotation", "Terminate With Animation" },
                new string[] { "The file from which to call from/The graphical effect to call. Value1 = File#, Value2 = Graphic ID", "The bone to attach the graphical effect to.", "Transition from the attached bone along the Z axis.", "Transition from the attached bone along the Y axis.", "Transition from the attached bone along the X axis.", "Rotation along the Z axis.", "Rotation along the Y axis.", "Rotation along the X axis.", "The size of the graphic.", "A random value lesser than the specified value and added to the Z Offset.", "A random value lesser than the specified value and added to the Y Offset.", "A random value lesser than the specified value and added to the X Offset.", "A random value lesser than the specified value and added to the Z Rotation.", "A random value lesser than the specified value and added to the Y Rotation.", "A random value lesser than the specified value and added to the X Rotation.", "Sets whether or not this graphic effect terminates when the animation ends." },
                "\\name(): File=\\unhex(\\half1(\\value(0))), Graphic ID=\\unhex(\\half2(\\value(0))), Bone=\\bone(\\value(1)), Translation=(\\value(4), \\value(3), \\value(2)), Rotation=(\\value(7), \\value(6), \\value(5)), Scale=\\value(8), Random Translation=(\\value(11), \\value(10), \\value(9)), Random Rotation=(\\value(14), \\value(13), \\value(12)), Anchored=\\value(15)",
                new long[] { 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3 }));
            _e.Add(0x11010A00, new EventInformation(0x11010A00, "External Graphic Effect",
                "Generate a graphical effect from an external file. (usually the Ef_ file)",
                new string[] { "Graphic", "Bone", "Z Offset", "Y Offset", "X Offset", "Z Rotation", "Y Rotation", "X Rotation", "Scale", "Terminate With Animation" },
                new string[] { "The file from which to call from/The graphical effect to call. Value1 = File#, Value2 = Graphic ID", "The bone to attach the graphical effect to.", "Transition from the attached bone along the Z axis.", "Transition from the attached bone along the Y axis.", "Transition from the attached bone along the X axis.", "Rotation along the Z axis.", "Rotation along the Y axis.", "Rotation along the X axis.", "The size of the graphic.", "Sets whether or not this graphic effect terminates when the animation ends." },
                "\\name(): File=\\unhex(\\half1(\\value(0))), Graphic ID=\\unhex(\\half2(\\value(0))), Bone=\\bone(\\value(1)), Translation=(\\value(4), \\value(3), \\value(2)), Rotation=(\\value(7), \\value(6), \\value(5)), Scale=\\value(8), Anchored=\\value(9)",
                new long[] { 0, 0, 1, 1, 1, 1, 1, 1, 1, 3 }));
            _e.Add(0x11020A00, new EventInformation(0x11020A00, "External Graphic Effect",
                "Generate a graphical effect from an external file. (usually the Ef_ file)",
                new string[] { "Graphic", "Bone", "Z Offset", "Y Offset", "X Offset", "Z Rotation", "Y Rotation", "X Rotation", "Scale", "Terminate With Animation" },
                new string[] { "The file from which to call from/The graphical effect to call. Value1 = File#, Value2 = Graphic ID", "The bone to attach the graphical effect to.", "Transition from the attached bone along the Z axis.", "Transition from the attached bone along the Y axis.", "Transition from the attached bone along the X axis.", "Rotation along the Z axis.", "Rotation along the Y axis.", "Rotation along the X axis.", "The size of the graphic.", "Sets whether or not this graphic effect terminates when the animation ends." },
                "\\name(): File=\\unhex(\\half1(\\value(0))), Graphic ID=\\unhex(\\half2(\\value(0))), Bone=\\bone(\\value(1)), Translation=(\\value(4), \\value(3), \\value(2)), Rotation=(\\value(7), \\value(6), \\value(5)), Scale=\\value(8), Anchored=\\value(9)",
                new long[] { 0, 0, 1, 1, 1, 1, 1, 1, 1, 3 }));
            _e.Add(0x11170700, new EventInformation(0x11170700, "Limited Screen Tint",
                "Tint the screen to the specified color.",
                new string[] { "Transition In Time", "Red", "Green", "Blue", "Alpha", "Frame Count", "Transition Out Time" },
                new string[] { "The time taken to transition from the current color to the specified color.", "The red value.", "The green value.", "The blue value.", "The transparency.", "The amount of frames that the color lasts.", "The amount of time it takes the color to fade out." },
                "\\name(): TransInTime=\\value(0), RGBA=(\\value(1), \\value(2), \\value(3), \\value(4)), FrameCount=\\value(5), TransOutTime=\\value(6)",
                new long[] { 0, 0, 0, 0, 0, 0, 0 }));
            _e.Add(0x111A1000, new EventInformation(0x111A1000, "Graphic Effect",
                "Generate a generic graphical effect with the specified parameters.",
                new string[] { "Graphic", "Bone", "Z Offset", "Y Offset", "X Offset", "Z Rotation", "Y Rotation", "X Rotation", "Scale", "Random Z Offset", "Random Y Offset", "Random Z Offset", "Random Z Rotation", "Random Y Rotation", "Random X Rotation", "Terminate With Animation" },
                new string[] { "The graphical effect to call.", "The bone to attach the graphical effect to.", "Transition from the attached bone along the Z axis.", "Transition from the attached bone along the Y axis.", "Transition from the attached bone along the X axis.", "Rotation along the Z axis.", "Rotation along the Y axis.", "Rotation along the X axis.", "The size of the graphic.", "A random value lesser than the specified value and added to the Z Offset.", "A random value lesser than the specified value and added to the Y Offset.", "A random value lesser than the specified value and added to the X Offset.", "A random value lesser than the specified value and added to the Z Rotation.", "A random value lesser than the specified value and added to the Y Rotation.", "A random value lesser than the specified value and added to the X Rotation.", "Sets whether or not this graphic  effect terminates when the animation ends." },
                "\\name(): Graphic=\\value(0), Bone=\\bone(\\value(1)), Translation=(\\value(4), \\value(3), \\value(2)), Rotation=(\\value(7), \\value(6), \\value(5)), Scale=\\value(8), Random Translation=(\\value(11), \\value(10), \\value(9)), Random Rotation=(\\value(14), \\value(13), \\value(12)), Anchored=\\value(15)",
                new long[] { 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3 }));
            _e.Add(0x111B1000, new EventInformation(0x111B1000, "Graphic Effect",
                "Generate a generic graphical effect with the specified parameters.",
                new string[] { "Graphic", "Bone", "Z Offset", "Y Offset", "X Offset", "Z Rotation", "Y Rotation", "X Rotation", "Scale", "Random Z Offset", "Random Y Offset", "Random Z Offset", "Random Z Rotation", "Random Y Rotation", "Random X Rotation", "Terminate With Animation" },
                new string[] { "The graphical effect to call.", "The bone to attach the graphical effect to.", "Transition from the attached bone along the Z axis.", "Transition from the attached bone along the Y axis.", "Transition from the attached bone along the X axis.", "Rotation along the Z axis.", "Rotation along the Y axis.", "Rotation along the X axis.", "The size of the graphic.", "A random value lesser than the specified value and added to the Z Offset.", "A random value lesser than the specified value and added to the Y Offset.", "A random value lesser than the specified value and added to the X Offset.", "A random value lesser than the specified value and added to the Z Rotation.", "A random value lesser than the specified value and added to the Y Rotation.", "A random value lesser than the specified value and added to the X Rotation.", "Sets whether or not this graphic  effect terminates when the animation ends." },
                "\\name(): Graphic=\\value(0), Bone=\\bone(\\value(1)), Translation=(\\value(4), \\value(3), \\value(2)), Rotation=(\\value(7), \\value(6), \\value(5)), Scale=\\value(8), Random Translation=(\\value(11), \\value(10), \\value(9)), Random Rotation=(\\value(14), \\value(13), \\value(12)), Anchored=\\value(15)",
                new long[] { 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3 }));
            _e.Add(0x11031400, new EventInformation(0x11031400, "Sword Glow",
                "Creates glow of sword.  Only usable when the proper effects are loaded by their respective characters.",
                new string[] { "Color", "Blur Length", "Trail Bone #1", "X Offset", "Y Offset", "Z Offset", "Trail Bone #2", "X Offset", "Y Offset", "Z Offset", "Glow State", "Graphic ID", "Glow Bone", "X Offset", "Y Offset", "Z Offset", "X Rotation", "Y Rotation", "Z Rotation", "Glow Length" },
                new string[] { "Controls the hue of the glow.", "The length of the glow's aftershadow.", "Bone the 1st point of the sword trail follows.", "X offset of the 1st point of the sword trail.", "Y offset of the 1st point of the sword trail.", "Z offset of the 1st point of the sword trail.", "Bone the 2nd point of the  sword trail follows.", "X offset of the 2nd point of the sword trail.", "Y offset of the 2nd point of the sword trail.", "Z offset of the 2nd point of the sword trail.", "If set to true, glow/trail disappears at the end  of the subaction.", "The ID of  the External Graphic Effect referenced.", "Bone the sword glow follows.", "X offset of the sword glow.", "Y offset  of the sword glow.", "Z offset  of the sword glow.", "Rotation of the sword glow around the X axis.", "Rotation  of the sword glow around the Y axis.", "Rotation  of the sword glow around the Z axis.", "Length of the sword glow, i.e. halving this  value will make it half the  sword's length." },
                "\\name(): Hue=\\value(0), Length=\\value(1), Bone #1=\\bone(\\value(2)), Translation=(\\value(3), \\value(4), \\value(5)), Bone #2=\\bone(\\value(6)), Translation=(\\value(7), \\value(8), \\value(9)), State=\\value(10), Graphic=\\value(11), Bone #3=\\bone(\\value(12)), Translation=(\\value(13), \\value(14), \\value(15)), Rotation=(\\value(16), \\value(17), \\value(18)), Length=\\value(19)",
                new long[] { 0, 0, 0, 1, 1, 1, 0, 1, 1, 1, 3, 0, 0, 1, 1, 1, 1, 1, 1, 1 }));
            _e.Add(0x11050100, new EventInformation(0x11050100, "Terminate Sword Glow",
                "Eliminates sword glow graphics when set to 1. May have unknown applications.",
                new string[] { "Fade Time" },
                new string[] { "The time it takes for the sword glow to fade out." },
                "\\name(): Fade Time=\\value(0)",
                new long[] { 0 }));
            _e.Add(0x14070A00, new EventInformation(0x14070A00, "Aesthetic Wind Effect",
                "Moves nearby movable model parts (capes, hair, etc) with a wind specified by the parameters.",
                new string[] { "Undefined", "Undefined", "Strength", "Speed", "Size?", "Undefined", "Undefined", "Undefined" },
                new string[] { "Undefined.", "Undefined.", "The power of the wind.", "The speed of the wind.", "Perhaps the size of the wind.", "Undefined.", "Undefined.", "Undefined" },
                "",
                new long[] { 0, 1, 1, 1, 1, 1, 1, 0 }));
            _e.Add(0x12000200, new EventInformation(0x12000200, "Basic Variable Set",
                "Set a basic variable to the specified value.",
                new string[] { "Value", "Variable" },
                new string[] { "The value to place inside the specified variable.", "The Basic type variable to access." },
                "\\name(): \\value(1) = \\unhex(\\value(0))",
                new long[] { 0, 5 }));
            _e.Add(0x12010200, new EventInformation(0x12010200, "Basic Variable Add",
                "Add a specified value to a basic variable.",
                new string[] { "Value", "Variable" },
                new string[] { "The value add to the specified variable.", "The Basic type variable to access." },
                "\\name(): \\value(1) += \\unhex(\\value(0))",
                new long[] { 0, 5 }));
            _e.Add(0x12020200, new EventInformation(0x12020200, "Basic Variable Subtract",
                "Subtract a specified value from a basic variable.",
                new string[] { "Value", "Variable" },
                new string[] { "The value to subtract from the specified variable.", "The Basic type variable to access." },
                "\\name(): \\value(1) -= \\unhex(\\value(0))",
                new long[] { 0, 5 }));
            _e.Add(0x12060200, new EventInformation(0x12060200, "Float Variable Set",
                "Set a floating point variable to the specified value.",
                new string[] { "Value", "Variable" },
                new string[] { "The floating point value to place inside the specified variable.", "The Float type variable to access." },
                "\\name(): \\value(1) = \\unhex(\\value(0))",
                new long[] { 1, 5 }));
            _e.Add(0x12070200, new EventInformation(0x12070200, "Float Variable Add",
                "Add a specified value to a float variable.",
                new string[] { "Value", "Variable" },
                new string[] { "The floating point value to add to the specified variable.", "The Float type variable to access." },
                "\\name(): \\value(1) += \\unhex(\\value(0))",
                new long[] { 1, 5 }));
            _e.Add(0x12080200, new EventInformation(0x12080200, "Float Variable Subtract",
                "Subtract a specified value from a float variable.",
                new string[] { "Value", "Variable" },
                new string[] { "The floating point value to subtract from the specified variable.", "The Float type variable to access." },
                "\\name(): \\value(1) -= \\unhex(\\value(0))",
                new long[] { 1, 5 }));
            _e.Add(0x120A0100, new EventInformation(0x120A0100, "Bit Variable Set",
                "Set a bit variable to true.",
                new string[] { "Variable" },
                new string[] { "The Bit type variable to set." },
                "\\name(): \\value(0) = true",
                new long[] { 5 }));
            _e.Add(0x120B0100, new EventInformation(0x120B0100, "Bit Variable Clear",
                "Set a bit variable to false.",
                new string[] { "Variable" },
                new string[] { "The Bit type variable to clear." },
                "\\name(): \\value(0) = false",
                new long[] { 5 }));
            _e.Add(0x1A040500, new EventInformation(0x1A040500, "Camera Closeup",
                "Zoom the camera on the character.",
                new string[] { "Zoom Time", "Undefined", "Distance", "X Angle", "Y Angle" },
                new string[] { "The time it takes to zoom in on the target.", "Undefined.", "How far away the camera is from the character.", "The horizontal rotation around the character.", "The vertical rotation around the character." },
                "\\name(): Zoom Time=\\value(0), Undefined=\\value(1), Distance=\\value(2), X Rotation=\\value(3), Y Rotation=\\value(4)",
                new long[] { 0, 0, 1, 1, 1 }));
            _e.Add(0x1A080000, new EventInformation(0x1A080000, "Normal Camera",
                "Return the camera to its normal settings.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x1F000100, new EventInformation(0x1F000100, "Pickup Item",
                "Cause the character to recieve the closest item in range.",
                new string[] { "Undefined" },
                new string[] { "Undefined" },
                "",
                new long[] { 0 }));
            _e.Add(0x1F000200, new EventInformation(0x1F000200, "Pickup Item",
                "Cause the character to recieve the closest item in range.",
                new string[] { "Undefined", "Undefined" },
                new string[] { "Undefined.", "Undefined" },
                "",
                new long[] { 0, 0 }));
            _e.Add(0x1F010300, new EventInformation(0x1F010300, "Throw Item",
                "Cause the character to throw the currently held item.",
                new string[] { "Undefined", "Undefined", "Undefined" },
                new string[] { "Undefined.", "Undefined.", "Undefined" },
                "",
                new long[] { 5, 5, 5 }));
            _e.Add(0x1F020000, new EventInformation(0x1F020000, "Drop Item",
                "Cause the character to drop any currently held item.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x1F030100, new EventInformation(0x1F030100, "Consume Item",
                "Cause the character to consume the currently held item.",
                new string[] { "Undefined" },
                new string[] { "Undefined" },
                "",
                new long[] { 0 }));
            _e.Add(0x1F040200, new EventInformation(0x1F040200, "Item Property",
                "Modify a property of the currently held item.",
                new string[] { "Undefined", "Undefined" },
                new string[] { "Undefined.", "Undefined" },
                "",
                new long[] { 0, 1 }));
            _e.Add(0x1F070100, new EventInformation(0x1F070100, "Items 1F",
                "Is used when firing a cracker launcher.",
                new string[] { "Undefined" },
                new string[] { "Undefined" },
                "",
                new long[] { 5 }));
            _e.Add(0x1F080100, new EventInformation(0x1F080100, "Generate Item",
                "Generate an item in the character's hand.",
                new string[] { "Undefined" },
                new string[] { "Undefined" },
                "",
                new long[] { 0 }));
            _e.Add(0x1F0C0100, new EventInformation(0x1F0C0100, "Beam Sword Trail",
                "Creates a beam sword trail. Probably has more uses among battering weapons.",
                new string[] { "Undefined" },
                new string[] { "Undefined" },
                "",
                new long[] { 0 }));
            _e.Add(0x1F0E0500, new EventInformation(0x1F0E0500, "Throw Item",
                "Causes the character to throw the currently held item.",
                new string[] { "Undefined", "Undefined", "Undefined", "Undefined", "Undefined" },
                new string[] { "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined" },
                "",
                new long[] { 1, 1, 5, 5, 5 }));
            _e.Add(0x1F090100, new EventInformation(0x1F090100, "Item Visibility",
                "Determines visibilty of the currently held item.",
                new string[] { "Item Visibility" },
                new string[] { "Set Boolean: True = Visible, False = Invisible" },
                "",
                new long[] { 3 }));
            _e.Add(0x1F050000, new EventInformation(0x1F050000, "Fire Weapon",
                "Fires a shot from the currently held item.  (May have other unknown applications)",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x1F060100, new EventInformation(0x1F060100, "Fire Projectile",
                "Fires a projectile of the specified degree of power.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x21000000, new EventInformation(0x21000000, "Terminate Flash Effect",
                "Terminate all currently active flash effects.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x21010400, new EventInformation(0x21010400, "Flash Overlay Effect",
                "Generate a flash overlay effect over the character with the specified colors and opacity. Replaces any currently active flash effects.",
                new string[] { "R", "G", "B", "A" },
                new string[] { "The red value from 0-255.", "The green value from 0-255.", "The blue value from 0-255.", "The alpha value from 0-255 (0 = fully transparent, 255 = fully opaque)." },
                "\\name(): RGBA=(\\value(0), \\value(1), \\value(2), \\value(3))",
                new long[] { 0, 0, 0, 0 }));
            _e.Add(0x21020500, new EventInformation(0x21020500, "Change Flash Overlay Color",
                "Changes the color of the current flash overlay effect.",
                new string[] { "Transition Time", "R", "G", "B", "A" },
                new string[] { "The number of frames the colour change takes.", "The red value (0-255) of the target colour.", "The green value (0-255) of the target colour.", "The blue value (0-255) of the target colour.", "The alpha value (0-255) of the target colour." },
                "\\name(): Transition Time=\\value(0), RGBA=(\\value(1), \\value(2), \\value(3), \\value(4))",
                new long[] { 0, 0, 0, 0, 0 }));
            _e.Add(0x21050600, new EventInformation(0x21050600, "Flash Light Effect",
                "Generate a flash lighting effect over the character with the specified colors, opacity and angle.  Replaces any currently active flash effects.",
                new string[] { "R", "G", "B", "A", "Angle", "Unknown" },
                new string[] { "The red value from 0-255.", "The green value from 0-255.", "The blue value from 0-255.", "The alpha value from 0-255 (0 = fully transparent, 255 = fully opaque).", "The angle in degrees of the virtual light source.", "Possibly the distance of the virtual light source?" },
                "\\name(): RGBA=(\\value(0), \\value(1), \\value(2), \\value(3)), Light Source X=\\value(4), Light Source Y=\\value(5)",
                new long[] { 0, 0, 0, 0, 1, 1 }));
            _e.Add(0x21070500, new EventInformation(0x21070500, "Change Flash Light Color",
                "Changes the color of the current flash light effect.",
                new string[] { "Transition Time", "R", "G", "B", "A" },
                new string[] { "The number of frames the color change takes.", "The red value (0-255) of the target color.", "The green value (0-255) of the target color.", "The blue value (0-255) of the target color.", "The alpha value (0-255) of the target color." },
                "\\name(): Transition Time=\\value(0), RGBA=(\\value(1), \\value(2), \\value(3), \\value(4))",
                new long[] { 0, 0, 0, 0, 0 }));
            _e.Add(0x64000000, new EventInformation(0x64000000, "Allow Interrupt",
                "Allow the current action to be interrupted by another action.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x020A0100, new EventInformation(0x020A0100, "Allow Specific Interrupt",
                "Allows interruption only by specific commands.  See parameters for list of possible interrupts.",
                new string[] { "Interrupt ID" },
                new string[] { "List of types of commands: 1-Ground Special, 2-Ground Item, 3-Ground Catch, 4-Ground Attack, 5-Ground Escape, 6-Ground Guard, 7-Ground Jump, 8-Ground (other), 9-Air Landing, A-Grab Edge, B-Air Special, C-Air Item Throw, D-Air Lasso, E-Air Dodge, F-Air Attack, 10-Air Tread Jump, 11-Air Walljump, 12-Air Jump Aerial, 13-Fall Through Plat(only works in squat)." },
                "\\name(): \\enum(\\value(0), 0)",
                new long[] { 0 }) { _enums = new Dictionary<int, List<string>>() { { 0, new List<string>() { "Main", "Ground Special", "Ground Item", "Ground Catch", "Ground Attack", "Ground Escape", "Ground Guard", "Ground Jump", "Ground (Other)", "Air Landing", "Grab Edge", "Air Special", "Air Item Throw", "Air Lasso", "Air Dodge", "Air Attack", "Air Tread Jump", "Air Walljump", "Air Jump Aerial", "Fall Through Platform (Squat Only)" } } } });
            _e.Add(0x1A000100, new EventInformation(0x1A000100, "Screenshake",
                "Shakes the screen.",
                new string[] { "Magnitude" },
                new string[] { "The intensity of the screenshake." },
                "\\name(): Magnitude=\\value(0)",
                new long[] { 1 }));
            _e.Add(0x0B020100, new EventInformation(0x0B020100, "Visibility",
                "Changes whether the model is visible or not.",
                new string[] { "Visibility" },
                new string[] { "Set Boolean: True = Visible, False = Invisible" },
                "Visible: \\value(0)",
                new long[] { 3 }));
            _e.Add(0x07070200, new EventInformation(0x07070200, "Rumble",
                "Undefined. Affects the rumble feature of the controller.",
                new string[] { "Undefined", "Undefined" },
                new string[] { "Undefined.", "Undefined" },
                "",
                new long[] { 0, 0 }));
            _e.Add(0x0E080400, new EventInformation(0x0E080400, "Set/Add Momentum",
                "Controls the movement velocity of the object.",
                new string[] { "Horizontal Velocity", "Vertical Velocity", "Set/Add Horizontal", "Set/Add Vertical" },
                new string[] { "The speed of the object moving left/right.", "The speed of the object moving up/down.", "0 = Add, 1 = Set", "0 = Add, 1 = Set" },
                "\\name(): Horizontal=\\value(0), Vertical=\\value(1), Add/Set Horiz=\\value(2), Add/Set Vert=\\value(3)",
                new long[] { 1, 1, 0, 0 }));
            _e.Add(0x0E010200, new EventInformation(0x0E010200, "Add/Subtract Character Momentum",
                "Adds or subtracts speed to the character's current momentum.",
                new string[] { "Horizontal Velocity", "Vertical Velocity" },
                new string[] { "The speed of the character moving left/right.", "The speed of the character moving up/down." },
                "\\name(): Horizontal Speed=\\value(0), Vertical Speed=\\value(1)",
                new long[] { 1, 1 }));
            _e.Add(0x0E060100, new EventInformation(0x0E060100, "Disallow Certain Movements",
                "Does not allow the specified type of movement.",
                new string[] { "Type" },
                new string[] { "When set to 1, vertical movement is disallowed. When set to 2, horizontal movement is disallowed." },
                "\\name(): \\enum(\\value(0), 0)",
                new long[] { 0 }) { _enums = new Dictionary<int, List<string>>() { { 0, new List<string>() { "Undefined(0)", "Vertical", "Horizontal" } } } });
            _e.Add(0x0E070100, new EventInformation(0x0E070100, "Disallow Certain Movements 2",
                "This must be set to the same value as Disallow Certain Movements to work.",
                new string[] { },
                new string[] { },
                "",
                new long[] { 0 }));
            _e.Add(0x0E020100, new EventInformation(0x0E020100, "Disallow Vertical Movement",
                "When set to 1, vertical speed and acceleration are reset back to 0.",
                new string[] { },
                new string[] { },
                "\\name(): \\unhex(\\value(0))",
                new long[] { }));
            _e.Add(0x0C250100, new EventInformation(0x0C250100, "Tag Display",
                "Disables or enables tag display for the current subaction.",
                new string[] { "Tag On/Off" },
                new string[] { "True = On, False = Off" },
                "\\name(): \\value(0)",
                new long[] { 3 }));
            _e.Add(0x1E000200, new EventInformation(0x1E000200, "Super/Heavy Armor",
                "Begins super armor or heavy armor.  Set both parameters to 0 to end the armor.",
                new string[] { "Armor State", "Heavy Armor Tolerance" },
                new string[] { "0 = None, 1 = Super Armor, 2 = Knockback Based Heavy Armor, 3 = Damage Based Heavy Armor", "The minimum damage or KB that will cause the character to flinch when using heavy armor." },
                "\\name(): State=\\enum(\\value(0), 0), Tolerance=\\value(1)",
                new long[] { 0, 1 }) { _enums = new Dictionary<int, List<string>>() { { 0, new List<string>() { "None", "Super Armor", "Knockback Based Heavy Armor", "Damage Based Heavy Armor" } } } });
            _e.Add(0x1E030100, new EventInformation(0x1E030100, "Add/Subtract Damage",
                "Adds or subtracts the specified amount of damage from the character's current percentage.",
                new string[] { "Damage" },
                new string[] { "The amount of damage to add or subtract." },
                "\\name(): \\value(0)",
                new long[] { 1 }));
            _e.Add(0x06010200, new EventInformation(0x06010200, "Change Hitbox Damage",
                "Changes a specific hitbox's damage to the new amount. Only guaranteed to work on Offensive Collisions.",
                new string[] { "Hitbox", "Damage" },
                new string[] { "ID of the hitbox to be changed.", "New damage of the hitbox." },
                "\\name(): ID=\\value(0), Damage=\\value(1)",
                new long[] { 0, 0 }));
            _e.Add(0x06030100, new EventInformation(0x06030100, "Delete Hitbox",
                "Deletes a hitbox of the specified ID.  Only guaranteed to work on Offensive Collisions.",
                new string[] { "Hitbox" },
                new string[] { "ID of the hitbox to be deleted." },
                "\\name(): \\value(0)",
                new long[] { 0 }));
            _e.Add(0x0B000200, new EventInformation(0x0B000200, "Model Changer 1",
                "Changes the visibility of certain bones attached to objects. Uses bone groups and switches set in Reference 1 of the Model Visibility section.",
                new string[] { "Switch Index", "Bone Group Index" },
                new string[] { "The index of the switch group in Reference 1 to modify.", "The index of the group of bones in the switch. A value less than 0 or greater than the amount of groups will disable the visibility of all objects. All other groups will be turned off when switching this one on." },
                "\\name(): Switch=\\value(0), Group=\\value(1)",
                new long[] { 0, 0 }));
            _e.Add(0x0B010200, new EventInformation(0x0B010200, "Model Changer 2",
                "Changes the visibility of certain bones attached to objects. Uses bone groups and switches set in Reference 2 of the Model Visibility section.",
                new string[] { "Switch Index", "Bone Group Index" },
                new string[] { "The index of the switch group in Reference 2 to modify.", "The index of the group of bones in the switch. A value less than 0 or greater than the amount of groups will disable the visibility of all objects. All other groups will be turned off when switching this one on." },
                "\\name(): Switch=\\value(0), Group=\\value(1)",
                new long[] { }));
            _e.Add(0x10040100, new EventInformation(0x10040100, "Model Event 1",
                "This affects an article/model action. (This only works with characters who have articles in one of their files.",
                new string[] { "Model ID" },
                new string[] { "Model. (Only Summons the Id of the article/model to call. Only summons a FitChar##.pac. For example, Pit's bow is article/model 0)" },
                "\\name(): Model ID=\\value(0)",
                new long[] { }));
            _e.Add(0x10040200, new EventInformation(0x10040200, "Set Anchored Article SubAction",
                "Sets the specified article to execute the specified action immediately. Only works on anchored articles (Cape, FLUDD, not fireball, water).",
                new string[] { "Article ID", "Action" },
                new string[] { "The ID of the article you would like to edit.", "The action you would like the article to execute." },
                "\\name(): article=\\value(0), action=\\unhex(\\value(1))",
                new long[] { 0, 0 }));
            _e.Add(0x10040300, new EventInformation(0x10040300, "Set Anchored Article SubAction",
                "Sets the specified article to execute the specified action immediately. Only works on anchored articles (Cape, FLUDD, not fireball, water).",
                new string[] { "Article ID", "Action", "Subaction Exclusive?" },
                new string[] { "The ID of the article you would like to edit.", "The action you would like the article to execute.", "Whether or not you want the article to be automatically deleted when the subaction ends." },
                "\\name(): article=\\value(0), action=\\unhex(\\value(1)): Delete At End=\\value(2)",
                new long[] { 0, 0, 3 }));
            _e.Add(0x14040100, new EventInformation(0x14040100, "Terminate Wind Effect",
                "Ends the wind effect spawned by the \"Aesthetic Wind Effect\" event.",
                new string[] { "Undefined" },
                new string[] { "Usually set to 0." },
                "",
                new long[] { 0 }));
            _e.Add(0x070B0200, new EventInformation(0x070B0200, "Rumble Loop",
                "Creates a rumble loop on the controller.",
                new string[] { "Undefined", "Undefined" },
                new string[] { "Undefined.", "Undefined" },
                "",
                new long[] { 0, 0 }));
            _e.Add(0x18000100, new EventInformation(0x18000100, "Slope Contour Stand",
                "Moves the character's feet if on sloped ground.",
                new string[] { "Hip Bone Index" },
                new string[] { "The index of the bone that is the parent of the leg bones (Usually HipN)." },
                "\\name(): \\bone(\\unhex(\\value(0)))",
                new long[] { }));
            _e.Add(0x18010200, new EventInformation(0x18010200, "Slope Contour Full",
                "Moves entire character to match sloped ground.",
                new string[] { "HipN/TopN Bone Index", "Translation Bone Index" },
                new string[] { "The index of the HipN or TopN bone.", "The index of the bone that is used to translate the character (Usually TransN or HipN)." },
                "\\name(): \\bone(\\unhex(\\value(0))), \\bone(\\unhex(\\value(0)))",
                new long[] { }));
            _e.Add(0x10000200, new EventInformation(0x10000200, "Generate Article",
                "Generate a pre-made prop effect from the prop library.",
                new string[] { "Article ID", "Subaction Exclusive?" },
                new string[] { "The id of the prop article to be called.", "Whether or not you want the article to be automatically deleted when the subaction ends." },
                "",
                new long[] { 0, 3 }));
            _e.Add(0x10010100, new EventInformation(0x10010100, "Article Event 02",
                "Makes the article preform an animation when set to 1.",
                new string[] { "Article ID" },
                new string[] { "ID of the article." },
                "",
                new long[] { }));
            _e.Add(0x00030000, new EventInformation(0x00030000, "Flow 03",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x000B0100, new EventInformation(0x000B0100, "And",
                "Seems to be an \"and\" to an If statement.",
                new string[] { },
                new string[] { },
                "And \\value(0):",
                new long[] { }));
            _e.Add(0x000B0200, new EventInformation(0x000B0200, "And Value",
                "Seems to be an \"and\" to an If statement.",
                new string[] { "Requirement", "Variable" },
                new string[] { "The form of requirement used in evaluation of the event.", "The variable applied to the requirement." },
                "And \\value(0): \\value(1)",
                new long[] { 6, 0 }));
            _e.Add(0x000B0300, new EventInformation(0x000B0300, "And Unknown",
                "Seems to be an \"and\" to an If statement.",
                new string[] { "Requirement", "Variable", "Unknown" },
                new string[] { "The form of requirement used in evaluation of the event.", "The variable applied to the requirement.", "Unknown" },
                "And \\value(0): \\value(1); Unknown=\\value(2)",
                new long[] { 6, 0, 0 }));
            _e.Add(0x000C0100, new EventInformation(0x000C0100, "Or",
                "Insert an alternate requirement to fall back on if the above requirement(s) are not met.",
                new string[] { "Requirement" },
                new string[] { "The form of requirement used in evaluation of the event." },
                "Or \\value(0):",
                new long[] { 6 }));
            _e.Add(0x000C0200, new EventInformation(0x000C0200, "Or Value",
                "Insert an alternate requirement to fall back on if the above requirement(s) are not met.",
                new string[] { "Requirement", "Variable" },
                new string[] { "The form of requirement used in evaluation of the event.", "The variable applied to the requirement." },
                "Or \\value(0): \\value(1)",
                new long[] { 6, 0 }));
            _e.Add(0x000C0300, new EventInformation(0x000C0300, "Or Unknown",
                "Insert an alternate requirement to fall back on if the above requirement(s) are not met.",
                new string[] { "Requirement", "Variable", "Unknown" },
                new string[] { "The form of requirement used in evaluation of the event.", "The variable applied to the requirement.", "Undefined" },
                "Or \\value(0): \\value(1); Unknown=\\value(2)",
                new long[] { 6, 0, 0 }));
            _e.Add(0x000C0400, new EventInformation(0x000C0400, "Or Comparison",
                "Insert an alternate requirement to fall back on if the above requirement(s) are not met.",
                new string[] { "Requirement", "Variable", "Comparison Method", "Variable" },
                new string[] { "The form of requirement used in evaluation of the event.", "The first variable in the comparison requirement.", "The method used to compare the two variables. From 0 to 5: <, ≤, =, ≠, ≥, > ", "The second variable in the comparison requirement." },
                "Or \\value(0): \\value(1) \\cmpsign(\\value(2)) \\value(3)",
                new long[] { 6, 5, 0, 5 }));
            _e.Add(0x000D0200, new EventInformation(0x000D0200, "Else If Value",
                "Insert an Else If block inside of an If block.",
                new string[] { "Requirement", "Variable" },
                new string[] { "The form of requirement used in evaluation of the event.", "The variable applied to the requirement." },
                "Else If \\value(0): \\value(1)",
                new long[] { 6, 0 }));
            _e.Add(0x000D0300, new EventInformation(0x000D0300, "Else If Unknown",
                "Insert an Else If block inside of an If block.",
                new string[] { "Requirement", "Variable", "Unknown" },
                new string[] { "The form of requirement used in evaluation of the event.", "The variable applied to the requirement.", "Undefined" },
                "Else If \\value(0): \\value(1); Unknown=\\value(2)",
                new long[] { 6, 0, 0 }));
            _e.Add(0x00180000, new EventInformation(0x00180000, "Break",
                "Appears within Case statements; exits the switch event completely. All code located in the same case block after this event will not be executed.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x02000400, new EventInformation(0x02000400, "Change Action Status Value",
                "Change the current action upon the specified requirement being met (the requirement does not have to be met at the time this ID is executed - it can be used anytime after execution.)",
                new string[] { "Status ID", "Action", "Requirement", "Variable" },
                new string[] { "The four-digit status ID of the change action event. Can later be disabled via 02080100 (Disable Action Status ID)", "The ID of the action that the character will execute.", "The form of requirement used in evaluation of the event.", "The variable applied to the requirement." },
                "Prioritized Change Action: priority=\\unhex(\\value(0)), action=\\unhex(\\value(1)), requirement=\\value(2): \\value(3)",
                new long[] { 0, 0, 6, 0 }));
            _e.Add(0x02000500, new EventInformation(0x02000500, "Change Action Status Unknown",
                "Change the current action upon the specified requirement being met (the requirement does not have to be met at the time this ID is executed - it can be used anytime after execution.)",
                new string[] { "Status ID", "Action", "Requirement", "Variable", "Unknown" },
                new string[] { "The four-digit status ID of the change action event. Can later be disabled via 02080100 (Disable Action Status ID)", "The ID of the action that the character will execute.", "The form of requirement used in evaluation of the event.", "The variable applied to the requirement.", "Used as a specification for the requirement when necessary (for example, if the requirement is \"button pressed\", this denotes which button)." },
                "Prioritized Change Action: priority=\\unhex(\\value(0)), action=\\unhex(\\value(1)), requirement=\\value(2): \\value(3); Unknown=\\value(4)",
                new long[] { 0, 0, 6, 0, 0 }));
            _e.Add(0x02000600, new EventInformation(0x02000600, "Change Action Status Comparison",
                "Change the current action upon the specified requirement being met (the requirement does not have to be met at the time this ID is executed - it can be used anytime after execution.)",
                new string[] { "Status ID", "Action", "Requirement", "Variable", "Comparison", "Variable" },
                new string[] { "The four-digit status ID of the change action event. Can later be disabled via 02080100 (Disable Action Status ID)", "The ID of the action that the character will execute.", "The form of requirement used in evaluation of the event.", "The variable applied to the requirement.", "The first variable in the comparison requirement.", "The method used to compare the two variables. From 0 to 5: <, ≤, =, ≠, ≥, >", "The second variable in the comparison requirement." },
                "Prioritized Change Action: priority=\\unhex(\\value(0)), action=\\unhex(\\value(1)), requirement=\\value(2): \\value(3) \\cmpsign(\\value(4)) \\value(5)",
                new long[] { 0, 0, 6, 0, 0 }));
            _e.Add(0x02080100, new EventInformation(0x02080100, "Disable Action Status ID",
                "Disables the Action associated with the given Status ID.",
                new string[] { "Status ID" },
                new string[] { "The Status ID to disable. After this command, the associated Action will not activate." },
                "\\name(): \\unhex(\\value(0))",
                new long[] { 0 }));
            _e.Add(0x02090200, new EventInformation(0x02090200, "Invert Action Status ID",
                "Appears to invert (or possibly only disable) a specific Status ID's enabled/disabled status. For example, if a character can crawl, this is used to disable the ability to dash when crouched, even though naturally crouching allows dashing through 020A (Allow Specific Interrupt).",
                new string[] { "Interrupt ID?", "Status ID?" },
                new string[] { "Appears to be a Interrupt ID as used by 020A (Allow Specific Interrupt)", "Appears to be a Status ID." },
                "\\name(): Interrupt ID=\\enum(\\value(0), 0), Status ID=\\unhex(\\value(1))",
                new long[] { 0, 0 }) { _enums = new Dictionary<int, List<string>>() { { 0, new List<string>() { "Main", "Ground Special", "Ground Item", "Ground Catch", "Ground Attack", "Ground Escape", "Ground Guard", "Ground Jump", "Ground (Other)", "Air Landing", "Grab Edge", "Air Special", "Air Item Throw", "Air Lasso", "Air Dodge", "Air Attack", "Air Tread Jump", "Air Walljump", "Air Jump Aerial", "Fall Through Platform (Squat Only)" } } } });
            _e.Add(0x020B0100, new EventInformation(0x020B0100, "Disallow Specific Interrupt",
                "Closes the specific interruption window. Must be set to the same thing as the allow specific interrupt that you wish to cancel.",
                new string[] { "Interrupt ID" },
                new string[] { "List of types of commands: 1-Ground Special, 2-Ground Item, 3-Ground Catch, 4-Ground Attack, 5-Ground Escape, 6-Ground Guard, 7-Ground Jump, 8-Ground (other), 9-Air Landing, A-Grab Edge, B-Air Special, C-Air Item Throw, D-Air Lasso, E-Air Dodge, F-Air Attack, 10-Air Tread Jump, 11-Air Walljump, 12-Air Jump Aerial, 13-Fall Through Plat(only works in squat)." },
                "\\name(): \\enum(\\value(0), 0)",
                new long[] { 0 }) { _enums = new Dictionary<int, List<string>>() { { 0, new List<string>() { "Main", "Ground Special", "Ground Item", "Ground Catch", "Ground Attack", "Ground Escape", "Ground Guard", "Ground Jump", "Ground (Other)", "Air Landing", "Grab Edge", "Air Special", "Air Item Throw", "Air Lasso", "Air Dodge", "Air Attack", "Air Tread Jump", "Air Walljump", "Air Jump Aerial", "Fall Through Platform (Squat Only)" } } } });
            _e.Add(0x020C0100, new EventInformation(0x020C0100, "Unregister Interrupt?",
                "Possibly unregisters a previously created interrupt.",
                new string[] { "Interrupt ID" },
                new string[] { "Possibly the Interrupt ID to unregister." },
                "\\name(): \\enum(\\value(0), 0)",
                new long[] { 0 }) { _enums = new Dictionary<int, List<string>>() { { 0, new List<string>() { "Main", "Ground Special", "Ground Item", "Ground Catch", "Ground Attack", "Ground Escape", "Ground Guard", "Ground Jump", "Ground (Other)", "Air Landing", "Grab Edge", "Air Special", "Air Item Throw", "Air Lasso", "Air Dodge", "Air Attack", "Air Tread Jump", "Air Walljump", "Air Jump Aerial", "Fall Through Platform (Squat Only)" } } } });
            _e.Add(0x04020100, new EventInformation(0x04020100, "Additional Subaction Change Requirement",
                "",
                new string[] { "Requirement" },
                new string[] { "The form of requirement used in evaluation of the event." },
                "\\name(): \\value(0)",
                new long[] { 6 }));
            _e.Add(0x04020200, new EventInformation(0x04020200, "Additional Subaction Change Requirement Value",
                "",
                new string[] { "Requirement", "Variable" },
                new string[] { "The form of requirement used in evaluation of the event.", "The variable applied to the requirement." },
                "\\name(): \\value(0): \\value(1)",
                new long[] { 6, 5 }));
            _e.Add(0x04020300, new EventInformation(0x04020300, "Additional Subaction Change Requirement Unknown",
                "",
                new string[] { "Requirement", "Variable", "Undefined" },
                new string[] { "The form of requirement used in evaluation of the event.", "The variable applied to the requirement.", "Undefined" },
                "\\name(): \\value(0): \\value(1), \\value(2)",
                new long[] { 6, 5, 0 }));
            _e.Add(0x04020400, new EventInformation(0x04020400, "Additional Subaction Change Requirement Compare",
                "",
                new string[] { "Requirement", "Variable", "Comparison Method", "Variable" },
                new string[] { "The form of requirement used in evaluation of the event.", "The first variable in the comparison requirement.", "The method used to compare the two variables. From 0 to 5: <, ≤, =, ≠, ≥, >", "The second variable in the comparison requirement." },
                "\\name(): \\value(0): \\value(1) \\cmpsign(\\value(2)) \\value(3)",
                new long[] { 6, 5, 0, 5 }));
            _e.Add(0x04030100, new EventInformation(0x04030100, "Extra Subaction Change Requirement",
                "Seems to act as an additional requirement for Additional Subaction Change Requirement.",
                new string[] { "Requirement" },
                new string[] { "The form of requirement used in evaluation of the event." },
               "\\name(): \\value(0)",
                new long[] { 6 }));
            _e.Add(0x04030200, new EventInformation(0x04030200, "Extra Subaction Change Requirement Value",
                "",
                new string[] { "Requirement", "Variable" },
                new string[] { "The form of requirement used in evaluation of the event.", "The variable applied to the requirement." },
                "\\name(): \\value(0): \\value(1)",
                new long[] { 6, 5 }));
            _e.Add(0x04030300, new EventInformation(0x04030300, "Extra Subaction Change Requirement Unknown",
                "",
                new string[] { "Requirement", "Variable", "Undefined" },
                new string[] { "The form of requirement used in evaluation of the event.", "The variable applied to the requirement.", "Undefined" },
                "\\name(): \\value(0): \\value(1), \\value(2)",
                new long[] { 6, 5, 0 }));
            _e.Add(0x04030400, new EventInformation(0x04030400, "Extra Subaction Change Requirement Compare",
                "",
                new string[] { "Requirement", "Variable", "Comparison Method", "Variable" },
                new string[] { "The form of requirement used in evaluation of the event.", "The first variable in the comparison requirement.", "The method used to compare the two variables. From 0 to 5: <, ≤, =, ≠, ≥, > ", "The second variable in the comparison requirement." },
                "\\name(): \\value(0): \\value(1) \\cmpsign(\\value(2)) \\value(3)",
                new long[] { 6, 5, 0, 5 }));
            _e.Add(0x04060100, new EventInformation(0x04060100, "Set Animation Frame",
                "Changes the current frame of the animation. Does not change the frame of the subaction (i.e. timers and such are unaffected).",
                new string[] { "Frame" },
                new string[] { "The frame to skip to." },
                "\\name(): \\value(0)",
                new long[] { 1 }));
            _e.Add(0x040A0100, new EventInformation(0x040A0100, "Subactions 0A",
                "",
                new string[] { "Undefined" },
                new string[] { "Undefined" },
                "",
                new long[] { 0 }));
            _e.Add(0x040B0100, new EventInformation(0x040B0100, "Subactions 0B",
                "",
                new string[] { "Undefined" },
                new string[] { "Undefined" },
                "",
                new long[] { 1 }));
            _e.Add(0x040C0100, new EventInformation(0x040C0100, "Subactions 0C",
                "",
                new string[] { "Subaction" },
                new string[] { "A subaction ID." },
                "",
                new long[] { 0 }));
            _e.Add(0x040D0100, new EventInformation(0x040D0100, "Subactions 0D",
                "",
                new string[] { "Undefined" },
                new string[] { "Undefined" },
                "",
                new long[] { 0 }));
            _e.Add(0x04140100, new EventInformation(0x04140100, "Subactions 14",
                "",
                new string[] { "Undefined" },
                new string[] { "Undefined" },
                "",
                new long[] { 1 }));
            _e.Add(0x04180100, new EventInformation(0x04180100, "Subactions 18",
                "",
                new string[] { "Undefined" },
                new string[] { "Undefined" },
                "",
                new long[] { 5 }));
            _e.Add(0x05010000, new EventInformation(0x05010000, "Posture 01",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x05020000, new EventInformation(0x05020000, "Posture 02",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x05030000, new EventInformation(0x05030000, "Posture 03",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x05040000, new EventInformation(0x05040000, "Posture 04",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x05070300, new EventInformation(0x05070300, "Posture 07",
                "",
                new string[] { "Undefined", "Undefined", "Undefined" },
                new string[] { "Undefined.", "Undefined.", "Undefined" },
                "",
                new long[] { 1, 5, 1 }));
            _e.Add(0x050D0100, new EventInformation(0x050D0100, "Posture 0D",
                "",
                new string[] { "Undefined" },
                new string[] { "Undefined" },
                "",
                new long[] { 0 }));
            _e.Add(0x06020200, new EventInformation(0x06020200, "Change Hitbox Size",
                "Changes a specific hitbox's size to the new amount. Only guaranteed to work on Offensive Collisions.",
                new string[] { "Hitbox", "Size" },
                new string[] { "ID of the hitbox to be changed.", "New size of the hitbox." },
                "\\name(): ID=\\value(1), Size=\\value(1)",
                new long[] { 0, 0 }));
            _e.Add(0x060C0100, new EventInformation(0x060C0100, "Delete Catch Collision",
                "Deletes the catch collision with the specified ID.",
                new string[] { "ID" },
                new string[] { "ID of the collision to delete" },
                "\\name(): \\value(0)",
                new long[] { 0 }));
            _e.Add(0x06101100, new EventInformation(0x06101100, "Inert Collision",
                "Generates an oblivious hitbox only used to detect collision with other characters/objects.",
                new string[] { "Undefined", "Id", "Bone", "Size", "X Offset", "Y Offset", "Z Offset", "Flags", "F", "Air/Ground", "Undefined", "Undefined", "Undefined", "Undefined", "Rehit Rate?", "Affects Self?", "Undefined" },
                new string[] { "When messed with, seemed to affect the accuracy of the collision detection. Should be set to 0 to be safe.", "The ID of the hitbox", "The bone that the hitbubble is attached to.", "The size of the hitbubble.", "Transition relative to the currently attached bone.", "Transition relative to the currently attached bone.", "Transition relative to the currently attached bone.", "+02 = Hits Normally, +04=Can be reflected....", "Undefined", "1 = hits grounded, 2 = hits aerial, 3 = hits grounded and aerial", "Undefined", "Undefined", "Undefined", "Undefined", "The rehit rate of the hitbubble?", "Possibly if the uninteractive collision affects the host character.", "Undefined" },
                "\\name(): Id=\\value(1), Bone=\\bone(\\value(2)), Size=\\value(3), Z Offset=\\value(4), Y Offset=\\value(5), X Offset=\\value(6), Air/Ground=\\value(9),Self-Affliction=\\value(15)",
                new long[] { 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 3, 0, 0, 3, 0, 3, 0 }));
            _e.Add(0x062C0F00, new EventInformation(0x062C0F00, "\"Bump\" Collision?",
                "Possibly the \"bump\" collisions that occur when a character in hitstun collides with another body.",
                new string[] { "Bone/ID?", "Damage", "Trajectory", "Weight Knockback/Knockback Growth?", "Shield Damage/Base Knockback?", "Size?", "X Offset?", "Y Offset?", "Z Offset?", "Tripping Rate?", "Hitlag Multiplier?", "Directional Influence Multiplier?", "Flags 1", "Undefined", "Flags 2" },
                new string[] { "The bone the collision bubble is attached to/The ID number of the collision bubble. Where XXXXYYYY is X=Bone, Y=Id.", "The amount of damage inflicted to the target upon collision.", "The direction in which a target gets launched.", "The distance the target is launched proportional to weight for fixed knockback hits/The additional distance the target is launched proportional to its damage (launch force for fixed knockback hits). XXXXYYYY is X=Weight Knockback, Y=Knockback Growth.", "The amount of damage dealt to the target's shield if it is up/The distance the target is launched regardless of its damage (zero for fixed knockback hits). XXXXYYYY is X=Shield Damage, Y=Base Knockback.", "The size of the collision bubble.", "The amount the collision bubble is transitioned relative to the currently attached bone.", "The amount the collision bubble is transitioned relative to the currently attached bone.", "The amount the collision bubble is transitioned relative to the currently attached bone.", "The percent possibility of the collision bubble inducing a trip, provided the target doesn't leave the ground from the knockback.", "A multiplier affecting the time in which both parties pause when the collision bubble connects.", "A multiplier affecting the ability for the character maneuver themselves while suffering from the hitlag generated by this collision bubble.", "Flags for various parameters such as hit effects and sound effects.", "Undefined.", "Flags for various parameters such as hit effects and sound effects." },
                "",
                new long[] { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0 }));
            _e.Add(0x062D0000, new EventInformation(0x062D0000, "Collisions 2D",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x07000000, new EventInformation(0x07000000, "Clear Buffer?",
                "Possibly clears the controller buffer.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x07010000, new EventInformation(0x07010000, "Controller 01",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x07020000, new EventInformation(0x07020000, "Controller 02",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x07060100, new EventInformation(0x07060100, "Controller 06",
                "",
                new string[] { "Undefined" },
                new string[] { "Undefined" },
                "",
                new long[] { 3 }));
            _e.Add(0x070C0000, new EventInformation(0x070C0000, "Controller 0C",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x08010100, new EventInformation(0x08010100, "Edge Interaction 01",
                "",
                new string[] { "Undefined" },
                new string[] { "Undefined" },
                "",
                new long[] { 0 }));
            _e.Add(0x08020100, new EventInformation(0x08020100, "Edge Interaction 02",
                "",
                new string[] { "Character State?" },
                new string[] { "Appears to use similar values to \"Set Edge Slide.\"" },
                "",
                new long[] { 0 }));
            _e.Add(0x08040100, new EventInformation(0x08040100, "Edge Interaction 04",
                "",
                new string[] { "Undefined" },
                new string[] { "Undefined" },
                "",
                new long[] { 3 }));
            _e.Add(0x08070000, new EventInformation(0x08070000, "Edge Interaction 07",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x09000100, new EventInformation(0x09000100, "Module09 00",
                "",
                new string[] { "Undefined" },
                new string[] { "Undefined" },
                "",
                new long[] { 0 }));
            _e.Add(0x0C010000, new EventInformation(0x0C010000, "Character Specific 01",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x0C040000, new EventInformation(0x0C040000, "Character Specific 04",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x0C060000, new EventInformation(0x0C060000, "Enter Final Smash State",
                "Allows use of Final Smash locked articles, variables, etc. Highly unstable.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x0C070000, new EventInformation(0x0C070000, "Exit Final Smash State",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x0C080000, new EventInformation(0x0C080000, "Terminate Self",
                "Used by certain article instances to remove themselves.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x0C090100, new EventInformation(0x0C090100, "Allow/Disallow Ledgegrab",
                "Allow or disallow grabbing ledges during the current subaction.",
                new string[] { "Allow/Disallow" },
                new string[] { "0 = cannot, 1 = Only in front, 2 = Always" },
                "",
                new long[] { 0 }));
            _e.Add(0x0C0A0100, new EventInformation(0x0C0A0100, "Character Specific 0A",
                "",
                new string[] { "Undefined" },
                new string[] { "Undefined" },
                "",
                new long[] { 0 }));
            _e.Add(0x0C130000, new EventInformation(0x0C130000, "Character Specific 13",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x0C160000, new EventInformation(0x0C160000, "Character Specific 16",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x0C170100, new EventInformation(0x0C170100, "Character Specific 17",
                "Undefined. Often appears before 0C25 (Tag Display)",
                new string[] { "Undefined" },
                new string[] { "Undefined." },
                "",
                new long[] { 3 }));
            _e.Add(0x0C170200, new EventInformation(0x0C170200, "Character Specific 17 Variable",
                "Undefined. Often appears before 0C25 (Tag Display)",
                new string[] { "Undefined", "Undefined" },
                new string[] { "Undefined.", "Undefined" },
                "",
                new long[] { 3, 5 }));
            _e.Add(0x0C1A0200, new EventInformation(0x0C1A0200, "Character Specific 1A",
                "",
                new string[] { "Undefined", "Undefined" },
                new string[] { "Undefined.", "Undefined" },
                "",
                new long[] { 0, 0 }));
            _e.Add(0x0C1B0100, new EventInformation(0x0C1B0100, "Character Specific 1B",
                "",
                new string[] { "Undefined" },
                new string[] { "Undefined" },
                "",
                new long[] { 5 }));
            _e.Add(0x0C1C0200, new EventInformation(0x0C1C0200, "Character Specific 1C",
                "",
                new string[] { "Undefined", "Undefined" },
                new string[] { "Undefined.", "Undefined" },
                "",
                new long[] { 0, 0 }));
            _e.Add(0x0C1C0300, new EventInformation(0x0C1C0300, "Character Specific 1C Boolean",
                "",
                new string[] { "Undefined", "Undefined", "Undefined" },
                new string[] { "Undefined.", "Undefined.", "Undefined" },
                "",
                new long[] { 0, 0, 3 }));
            _e.Add(0x0C1F0000, new EventInformation(0x0C1F0000, "Eating Voice Clip",
                "Play a random voice clip from the selection of eating voice clips.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x0C200200, new EventInformation(0x0C200200, "Character Specific 20",
                "",
                new string[] { "Undefined", "Undefined" },
                new string[] { "Undefined.", "Undefined" },
                "",
                new long[] { 0, 3 }));
            _e.Add(0x0C240100, new EventInformation(0x0C240100, "Character Specific 24",
                "",
                new string[] { "Undefined" },
                new string[] { "Undefined" },
                "",
                new long[] { 3 }));
            _e.Add(0x0C260100, new EventInformation(0x0C260100, "Character Specific 26",
                "",
                new string[] { "Undefined" },
                new string[] { "Undefined" },
                "",
                new long[] { 3 }));
            _e.Add(0x0C270000, new EventInformation(0x0C270000, "Character Specific 27",
                "Undefined. Often appears within Switch statements.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x0C290000, new EventInformation(0x0C290000, "Character Specific 29",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x0C2B0000, new EventInformation(0x0C2B0000, "Character Specific 2B",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x0D000200, new EventInformation(0x0D000200, "Concurrent Infinite Loop",
                "Runs a subroutine once per frame for the current action.",
                new string[] { "Thread ID?", "Offset" },
                new string[] { "Possibly the thread ID of the concurrent routine. Sometimes 4, sometimes 6, sometimes 9.", "The subroutine location that contains the events that you would like to loop infinitely." },
                "\\name() (Type \\value(0)) @\\value(1)",
                new long[] { 0, 2 }));
            _e.Add(0x0D010100, new EventInformation(0x0D010100, "Terminate Concurrent Infinite Loop?",
                "Seems to stop the execution of a loop created with event 0D000200.",
                new string[] { "Thread ID?" },
                new string[] { "Possibly the thread ID of the concurrent routine. Sometimes 4, sometimes 6, sometimes 9." },
                "",
                new long[] { 0 }));
            _e.Add(0x0F030200, new EventInformation(0x0F030200, "Link 03",
                "",
                new string[] { "Undefined", "Undefined" },
                new string[] { "Undefined.", "Undefined" },
                "",
                new long[] { 0, 0 }));
            _e.Add(0x11180200, new EventInformation(0x11180200, "End Unlimited Screen Tint",
                "Terminates an unlimited screen tint with the specified ID.",
                new string[] { "ID", "Frames" },
                new string[] { "The ID of the screen tint to end.", "The amount of frames that the color will take to fade out." },
                "\\name(): ID=\\value(0), TransOutTime=\\value(1)",
                new long[] { 0, 0 }));
            _e.Add(0x12030100, new EventInformation(0x12030100, "Basic Variable Increment",
                "Variable++",
                new string[] { "Variable" },
                new string[] { "The variable to increment." },
                "\\name(): \\value(0)++",
                new long[] { 5 }));
            _e.Add(0x12040100, new EventInformation(0x12040100, "Basic Variable Decrement",
                "Variable--",
                new string[] { "Variable" },
                new string[] { "The variable to decrement." },
                "\\name(): \\value(0)--",
                new long[] { 5 }));
            _e.Add(0x120F0200, new EventInformation(0x120F0200, "Float Variable Multiply",
                "Multiply a specified value with a float variable.",
                new string[] { "Value", "Variable" },
                new string[] { "The floating point value to multiply with the specified variable.", "The Float type variable to access." },
                "\\name(): \\value(1) *= \\unhex(\\value(0))",
                new long[] { 1, 5 }));
            _e.Add(0x17010000, new EventInformation(0x17010000, "Physics 01",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x17050000, new EventInformation(0x17050000, "Physics 05",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x19010000, new EventInformation(0x19010000, "Module19 01",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x1A030400, new EventInformation(0x1A030400, "Set Camera Boundaries",
                "Changes the camera boundaries of your character. Does not reset the camera boundaries; rather, it adds to them. Reverts to normal when the animation ends.",
                new string[] { "Front Boundary", "Back Boundary", "Top Boundary", "Bottom Boundary" },
                new string[] { "The boundary in front of the character.", "The boundary behind the character.", "The boundary above the character.", "The boundary below the character." },
                "\\name(): \\value(0) x \\value(1) x \\value(2); size \\value(3)",
                new long[] { 1, 1, 1, 1 }));
            _e.Add(0x1A060100, new EventInformation(0x1A060100, "Detach/Attach Camera (Close)",
                "Causes the camera to follow or stop following a character.",
                new string[] { "Detached/Attached", "Detached/Attached" },
                new string[] { "False = detached, True = attached.", "False = detached, True = attached." },
                "",
                new long[] { 3, 3 }));
            _e.Add(0x1A090000, new EventInformation(0x1A090000, "Camera 09",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x1A0A0100, new EventInformation(0x1A0A0100, "Camera 0A",
                "",
                new string[] { "Undefined" },
                new string[] { "Undefined" },
                "",
                new long[] { 0 }));
            _e.Add(0x1A0B0000, new EventInformation(0x1A0B0000, "Force Camera Control",
                "Appears to override any other settings in favor of the character's preference.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x1A0C0000, new EventInformation(0x1A0C0000, "Camera 0C",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x1F0A0000, new EventInformation(0x1F0A0000, "Obliterate Held Item",
                "Deletes the item that the character is holding",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x20000200, new EventInformation(0x20000200, "Turn 00",
                "",
                new string[] { "Undefined", "Undefined" },
                new string[] { "Undefined.", "Undefined" },
                "",
                new long[] { 5, 3 }));
            _e.Add(0x64010000, new EventInformation(0x64010000, "Cancel 01",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x64020000, new EventInformation(0x64020000, "Cancel 02",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x14050100, new EventInformation(0x14050100, "Aesthetic Wind 05",
                "",
                new string[] { "Undefined" },
                new string[] { "Undefined" },
                "",
                new long[] { 3 }));
            _e.Add(0x10070200, new EventInformation(0x10070200, "Set Remote Article SubAction",
                "Does the same thing as Set Article Action but seems to work on articles that aren't attached to the character.",
                new string[] { "Article ID", "SubAction" },
                new string[] { "ID of the article to be affected.", "The subaction you would like the article to execute." },
                "",
                new long[] { 0, 0 }));
            _e.Add(0x111D0100, new EventInformation(0x111D0100, "Effect ID",
                "Undefined.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x1F110100, new EventInformation(0x1F110100, "Item 11",
                "Undefined.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x06130000, new EventInformation(0x06130000, "Collisions 13",
                "Undefined.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x18030200, new EventInformation(0x18030200, "Character Specific Samus",
                "Used in samus.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x1F0F0100, new EventInformation(0x1F0F0100, "Morph Model Event",
                "If false model will appear else if true model will disappear.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x01020000, new EventInformation(0x01020000, "Loop Rest 2 for Goto",
                "Often follows 01000000 (Loop Rest 1 for Goto)",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x01000000, new EventInformation(0x01000000, "Loop Rest 1 for Goto",
                "Appears to work like 01010000, except is used for loops created by 00090100 (Goto)",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x0E0B0200, new EventInformation(0x0E0B0200, "Graphic Model Specf",
                "Appears to control posture graphics.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x11150300, new EventInformation(0x11150300, "Terminate Graphic Effect",
                "Terminate all instances of a lingering graphical effect.",
                new string[] { "Graphic", "Undefined", "Undefined" },
                new string[] { "The file from which to call from/The graphical effect to call. Value1 = File#, Value2 = Graphic ID", "Undefined.", "Undefined" },
                "\\name(): File=\\unhex(\\half1(\\value(0))), Graphic ID=\\unhex(\\half2(\\value(0))), Undefined1=\\value(1), Undefined2=\\value(2)",
                new long[] { 0, 3, 3 }));
            _e.Add(0x18010300, new EventInformation(0x18010300, "Character Spef GFX 02",
                "Appears to control posture graphics.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x17000000, new EventInformation(0x17000000, "Physics Normalize",
                "Returns to normal physics.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x10080200, new EventInformation(0x10080200, "Set Remote Article SubAction",
                "Does the same thing as Set Article Action but seems to work on articles that aren't attached to the character. (Used on Snake's downB)",
                new string[] { "Article ID", "SubAction" },
                new string[] { "ID of the article to be affected.", "The subaction you would like the article to execute." },
                "",
                new long[] { 0, 0 }));
            _e.Add(0x10020100, new EventInformation(0x10020100, "ONLY Article Event",
                "Article Animation.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x060A0900, new EventInformation(0x060A0900, "Catch Collision 2",
                "Generate a grabbing collision bubble with the specified parameters",
                new string[] { "ID", "Bone", "Size", "X offset", "Y Offset", "Z Offset", "Action", "Air/Ground", "Unknown" },
                new string[] { "ID of catch collision.", "The bone the grab is attached to.", "The size of the catch collision bubble.", "Transition relative to the currently attached bone.", "Transition relative to the currently attached bone.", "Transition relative to the currently attached bone.", "The Action ID that the foe executes if successfully grabbed.", "0 = grabs nothing, 1 = grabs grounded only, 2 = grabs aerial only, 3 = grabs aerial and grounded.", "???" },
                "",
                new long[] { 0, 0, 1, 1, 1, 1, 0, 0, 0 }));
            _e.Add(0x00060000, new EventInformation(0x00060000, "Loop Break?",
                "Breaks out of the current loop?",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x000A0300, new EventInformation(0x000A0300, "If Unknown",
                "Start an If block until an Else Or an EndIf is reached.",
                new string[] { "Requirement", "Variable", "Unknown" },
                new string[] { "The form of requirement used in evaluation of the event.", "The variable applied to the requirement.", "Undefined" },
                "If \\value(0): \\value(1); Unknown=\\value(2)",
                new long[] { 6, 0, 0 }));
            _e.Add(0x000D0100, new EventInformation(0x000D0100, "Else If",
                "Insert an Else If block inside of an If block.",
                new string[] { "Requirement" },
                new string[] { "The form of requirement used in evaluation of the event." },
                "Else If \\value(0):",
                new long[] { 6 }));
            _e.Add(0x02010400, new EventInformation(0x02010400, "Change Action",
                "Change the current action upon the specified requirement being met. (the requirement does not have to be met at the time this ID is executed - it can be used anytime after execution.)",
                new string[] { "Action", "Requirement", "Variable", "Unknown" },
                new string[] { "The id of the action that the character will execute.", "The form of requirement used in evaluation of the event.", "The variable applied to the requirement.", "Used as a specification for the requirement when necessary (for example, if the requirement is \"button pressed\", this denotes which button)." },
                "\\name(): \\value(0): \\value(1); Unknown=\\value(2)",
                new long[] { 0, 6, 5, 0 }));
            _e.Add(0x02020300, new EventInformation(0x02020300, "Unknown",
                "Used in the Primid file as alternatives to calling an AI procedure.",
                new string[] { "Undefined", "Undefined", "Undefined" },
                new string[] { "In the Primid file, seems to always equal 200.", "An action? (equals 7 when current action is not 7)", "In the Primid file, seems to always be Article Exists (15)." },
                "",
                new long[] { 0, 0, 6 }));
            _e.Add(0x02040300, new EventInformation(0x02040300, "Additional Change Action Requirement Value",
                "Add an additional requirement to the preceeding Change Action statement.",
                new string[] { "Requirement", "Variable", "Undefined" },
                new string[] { "The form of requirement used in evaluation of the event.", "The variable applied to the requirement.", "Undefined" },
                "",
                new long[] { 6, 5, 0 }));
            _e.Add(0x020E0000, new EventInformation(0x020E0000, "Unknown",
                "Used when the Blast Box detonates from a flame attack just before the change to the explosion action. Could be some sort of \"halt current action immediately\" or \"disable all possible statusID-based action changes\".",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x04090100, new EventInformation(0x04090100, "Unknown",
                "Used a few times in the Primid file.",
                new string[] { "Undefined" },
                new string[] { "Undefined" },
                "",
                new long[] { 0 }));
            _e.Add(0x04100200, new EventInformation(0x04100200, "Subactions 10",
                "",
                new string[] { "Undefined", "Undefined" },
                new string[] { "Undefined.", "Undefined" },
                "",
                new long[] { 0, 0 }));
            _e.Add(0x05050100, new EventInformation(0x05050100, "Posture 05",
                "",
                new string[] { "Undefined" },
                new string[] { "Undefined" },
                "",
                new long[] { 1 }));
            _e.Add(0x06140200, new EventInformation(0x06140200, "?",
                "Used to increase the damage of the Giant Punch when not fully charged.",
                new string[] { "Id", "Source" },
                new string[] { "The ID of the hitbox to change the damage of.", "The variable to read to find out how much to change the damage." },
                "",
                new long[] { 0, 5 }));
            _e.Add(0x06192F00, new EventInformation(0x06192F00, "SSE Hitbox",
                "The type of hitboxes used for enemies in the Subspace Emissary.",
                new string[] { "ID", "Undefined", "Bone", "Damage?", "Damage ramp?", "Angle", "Knockback growth?", "Knockback growth ramp?", "Weight-based knockback?", "WBK ramp?", "Base knockback?", "Base knockback ramp?", "Size?", "Size ramp?", "X Pos?", "Y Pos?", "Z Pos?", "Effect", "Trip chance?", "Freeze frames multiplier?", "SDI multiplier?", "Undefined", "Undefined", "Undefined", "Undefined", "Undefined", "Undefined", "Undefined", "Undefined", "Undefined", "Undefined", "Undefined", "Undefined", "Undefined", "Undefined", "Undefined", "Rehit rate?", "Undefined", "Undefined", "Undefined", "Undefined", "Undefined", "Undefined", "Undefined", "Undefined", "Undefined", "Difficulty level?" },
                new string[] { "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "i.e. Electric (3), Flame (5), etc", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Aside from effect, the only value to vary between Jyk types (for stage 040201 anyway).", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined." },
                "",
                new long[] { 0, 0, 0, 0, 1, 0, 0, 1, 0, 1, 0, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 3, 3, 0, 0, 0, 0, 3, 0, 0, 3, 0, 3, 3, 3, 3, 0, 3, 3, 0, 3, 3, 3, 3, 3, 0, 5 }));
            _e.Add(0x06240F00, new EventInformation(0x06240F00, "Unknown",
                "Used a single time in the Primid file.",
                new string[] { "Undefined", "Undefined", "Undefined", "Size?", "Undefined", "Undefined", "Undefined", "Undefined", "Undefined", "Undefined", "Undefined", "Undefined", "Undefined", "Undefined", "Undefined" },
                new string[] { "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined." },
                "",
                new long[] { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 1 }));
            _e.Add(0x0E040100, new EventInformation(0x0E040100, "Disable Horizontal Gravity",
                "When set to 1, horizontal speed and decay rate are reset back to 0.",
                new string[] { },
                new string[] { },
                "\\name(): \\unhex(\\value(0))",
                new long[] { }));
            _e.Add(0x0E050100, new EventInformation(0x0E040100, "Enable Horizontal Gravity?",
                "Undefined.",
                new string[] { },
                new string[] { },
                "\\name(): \\unhex(\\value(0))",
                new long[] { }));
            _e.Add(0x0E080200, new EventInformation(0x0E080200, "Set Momentum",
                "Controls the movement velocity of the object.",
                new string[] { "Horizontal Velocity", "Vertical Velocity" },
                new string[] { "The speed of the object moving left/right.", "The speed of the object moving up/down." },
                "",
                new long[] { 1, 1 }));
            _e.Add(0x10010200, new EventInformation(0x10010200, "Set Ex-Anchored Article Action",
                "Does the same thing as Set Article Action and Set Article Action 2 but seems to work on articles that are only initially attached to the character. (Used on Lucario's Aura Sphere)",
                new string[] { "Article ID", "Action" },
                new string[] { "ID of the article to be affected.", "The action you would like the article to execute." },
                "",
                new long[] { 0, 0 }));
            _e.Add(0x0000100A, new EventInformation(0x0000100A, "Undefined",
                "Generate a prop effect with the specified parameters.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x10130100, new EventInformation(0x10130100, "Link Character and Article?",
                "Seems to be used whenever a detached article needs to change its action...",
                new string[] { "Article ID" },
                new string[] { "ID of the article to be affected." },
                "",
                new long[] { 0 }));
            _e.Add(0x110D0100, new EventInformation(0x110D0100, "Unknown",
                "",
                new string[] { "Undefined" },
                new string[] { "Undefined" },
                "",
                new long[] { 0 }));
            _e.Add(0x11170600, new EventInformation(0x11170600, "Unlimited Screen Tint",
                "Tint the screen to the specified color until terminated by 11180200 (End Screen Tint).",
                new string[] { "ID", "Transition Time", "Red", "Green", "Blue", "Alpha" },
                new string[] { "The ID of the screen tint.", "The time taken to transition from its current color to the specified color.", "The red value.", "The green value.", "The blue value.", "The transparency." },
                "\\name(): ID=\\value(0), TransitionTime=\\value(1), RGBA=(\\value(2), \\value(3), \\value(4), \\value(5))",
                new long[] { 0, 0, 0, 0, 0, 0 }));
            _e.Add(0x12100200, new EventInformation(0x12100200, "Float Variable Divide",
                "Divide a specified value with a float variable.",
                new string[] { "Value", "Variable" },
                new string[] { "The floating point value to divide with the specified variable.", "The Float type variable to access." },
                "\\name(): \\value(1) /= \\unhex(\\value(0))",
                new long[] { 1, 5 }));
            _e.Add(0x15000000, new EventInformation(0x15000000, "Unknown",
                "Used in the Goomba file in places where Req[0x11] is true.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x1B020100, new EventInformation(0x1B020100, "Procedure Call?",
                "",
                new string[] { "Target Procedure" },
                new string[] { "Undefined" },
                "",
                new long[] { 4 }));
            _e.Add(0x1E010100, new EventInformation(0x1E010100, "Set Damage Immunity?",
                "Used at the start of Withdraw; might have something to do with Squirtle's immunity to damage during the move.",
                new string[] { "Undefined" },
                new string[] { "Undefined" },
                "",
                new long[] { 3 }));
            _e.Add(0x1F000400, new EventInformation(0x1F000400, "Pickup Item",
                "Cause the character to receive the closest item in range.",
                new string[] { "Undefined", "Undefined", "Undefined", "Undefined" },
                new string[] { "Undefined.", "Undefined.", "Undefined.", "Undefined" },
                "",
                new long[] { 0, 0, 0, 0 }));
            _e.Add(0x22000100, new EventInformation(0x22000100, "Set Team?",
                "Used with a parameter of -1 for a few explosives just before they go off (possibly to remove team allegiance and therefore hit the user).",
                new string[] { "Undefined" },
                new string[] { "Undefined" },
                "",
                new long[] { 0 }));
            _e.Add(0x22020100, new EventInformation(0x22020100, "Unknown",
                "",
                new string[] { "Undefined" },
                new string[] { "Undefined" },
                "",
                new long[] { 0 }));
            _e.Add(0x65000000, new EventInformation(0x65000000, "Item Self-Delete?",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x65030200, new EventInformation(0x65030200, "Unknown",
                "Something with rotation on item spawning.",
                new string[] { "Undefined", "Undefined" },
                new string[] { "Undefined.", "Undefined" },
                "",
                new long[] { 1, 1 }));
            _e.Add(0x65040100, new EventInformation(0x65040100, "Deletion Timer?",
                "Sets how many frames an item has to live? (Also used in enemy files.)",
                new string[] { "Lifetime (frames)?" },
                new string[] { "Undefined" },
                "",
                new long[] { 0 }));
            _e.Add(0x65050100, new EventInformation(0x65050100, "Unknown",
                "Unknown. Appears to be another timer.",
                new string[] { "Undefined" },
                new string[] { "Undefined" },
                "",
                new long[] { 0 }));
            _e.Add(0x65070200, new EventInformation(0x65070200, "Unknown",
                "Unknown. Appears to affect float variables. Used twice in the Jyk file.",
                new string[] { "Undefined", "Undefined" },
                new string[] { "Undefined.", "Undefined" },
                "",
                new long[] { 5, 5 }));
            _e.Add(0x65090200, new EventInformation(0x65090200, "Unknown",
                "Unknown. Appears to affect float variables.",
                new string[] { "Undefined", "Undefined" },
                new string[] { "Undefined.", "Undefined" },
                "",
                new long[] { 5, 5 }));
            _e.Add(0x650F0200, new EventInformation(0x650F0200, "Unknown",
                "",
                new string[] { "Undefined", "Undefined" },
                new string[] { "Undefined.", "Undefined" },
                "",
                new long[] { 5, 0 }));
            _e.Add(0x65130900, new EventInformation(0x65130900, "Generate Ratio-Based Random Number",
                "Generates a random number from 0 to [number of arguments-2], with the arguments starting at 1 being based on ratios.",
                new string[] { "Result Variable", "Ratio 1", "Ratio 2", "Ratio 3", "Ratio 4", "Ratio 5", "Ratio 6", "Ratio 7", "Ratio 8" },
                new string[] { "The result of the function is put in this variable.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined.", "Undefined" },
                "",
                new long[] { 5, 1, 1, 1, 1, 1, 1, 1, 1 }));
            _e.Add(0x65170200, new EventInformation(0x65170200, "Unknown",
                "Has something to do with sounds?",
                new string[] { "Undefined", "Undefined" },
                new string[] { "Undefined.", "Undefined" },
                "",
                new long[] { 0, 0 }));
            _e.Add(0x651B0100, new EventInformation(0x651B0100, "Activate slow motion?",
                "Used in the Dragoon.",
                new string[] { "Duration?" },
                new string[] { "Undefined" },
                "",
                new long[] { 0 }));
            _e.Add(0x651C0000, new EventInformation(0x651C0000, "Deactivate slow motion?",
                "Used in the Dragoon.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x651F0100, new EventInformation(0x651F0100, "Unknown",
                "Unknown. Used in the bumper item at least.",
                new string[] { "Undefined" },
                new string[] { "Undefined" },
                "",
                new long[] { 0 }));
            _e.Add(0x652C0000, new EventInformation(0x652C0000, "Unknown",
                "Unknown. In charizard's sideB subaction. If you nop it, he will not make rock shards or play the rock break sfx. wtf.",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x66000200, new EventInformation(0x66000200, "Unknown",
                "",
                new string[] { "Undefined", "Undefined" },
                new string[] { "Undefined.", "Undefined" },
                "",
                new long[] { 0, 0 }));
            _e.Add(0x66000400, new EventInformation(0x66000400, "Unknown",
                "Unknown. Used five times in the Jyk file with the values increasing somewhat constantly for each one (difficulty-related?).",
                new string[] { "Undefined", "Undefined", "Undefined", "Undefined" },
                new string[] { "Undefined.", "Undefined.", "Undefined.", "Undefined" },
                "",
                new long[] { 0, 0, 1, 0 }));
            _e.Add(0x66060100, new EventInformation(0x66060100, "Unknown",
                "Unknown. Used in action C of bumper at least.",
                new string[] { "Undefined" },
                new string[] { "Undefined" },
                "",
                new long[] { 0 }));
            _e.Add(0x66070100, new EventInformation(0x66070100, "Unknown",
                "",
                new string[] { "Undefined" },
                new string[] { "Undefined" },
                "",
                new long[] { 0 }));
            _e.Add(0x66090200, new EventInformation(0x66090200, "Unknown",
                "",
                new string[] { "Undefined", "Undefined" },
                new string[] { "Undefined.", "Undefined" },
                "",
                new long[] { 0, 0 }));
            _e.Add(0x660A0200, new EventInformation(0x660A0200, "Unknown",
                "",
                new string[] { "Undefined", "Undefined" },
                new string[] { "Undefined.", "Undefined" },
                "",
                new long[] { 0, 0 }));
            _e.Add(0x660B0200, new EventInformation(0x660B0200, "Unknown",
                "",
                new string[] { "Undefined", "Undefined" },
                new string[] { "Undefined.", "Undefined" },
                "",
                new long[] { 0, 0 }));
            _e.Add(0x660B0300, new EventInformation(0x660B0300, "Unknown",
                "Unknown. Something with spawn rotation.",
                new string[] { "Undefined", "Undefined", "Undefined" },
                new string[] { "Undefined.", "Undefined.", "Undefined" },
                "",
                new long[] { 1, 1, 1 }));
            _e.Add(0x69000100, new EventInformation(0x69000100, "Unknown",
                "Only known to be used in cases where \"Req[0x12], 1, \" is true.",
                new string[] { "Undefined" },
                new string[] { "Undefined" },
                "",
                new long[] { 5 }));
            _e.Add(0x6A000100, new EventInformation(0x6A000100, "Unknown",
                "",
                new string[] { "Undefined" },
                new string[] { "Undefined" },
                "",
                new long[] { 0 }));
            _e.Add(0x6A000200, new EventInformation(0x6A000200, "Unknown",
                "",
                new string[] { "Undefined", "Undefined" },
                new string[] { "Undefined.", "Undefined" },
                "",
                new long[] { 0, 0 }));
            _e.Add(0x6A000400, new EventInformation(0x6A000400, "Unknown",
                "",
                new string[] { "Undefined", "Undefined", "Undefined", "Undefined" },
                new string[] { "Undefined.", "Undefined.", "Undefined.", "Undefined" },
                "",
                new long[] { 0, 0, 0, 0 }));
            _e.Add(0x6A010100, new EventInformation(0x6A010100, "Unknown",
                "",
                new string[] { "Undefined" },
                new string[] { "Undefined" },
                "",
                new long[] { 0 }));
            _e.Add(0x6A020000, new EventInformation(0x6A020000, "Unknown",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x6A030000, new EventInformation(0x6A030000, "Unknown",
                "Undefined",
                new string[] { },
                new string[] { },
                "",
                new long[] { }));
            _e.Add(0x6B020300, new EventInformation(0x6B020300, "Unknown",
                "",
                new string[] { "Undefined", "Undefined", "Undefined" },
                new string[] { "Undefined.", "Undefined.", "Undefined." },
                "",
                new long[] { 3, 5, 5 }));
            _e.Add(0x6E000200, new EventInformation(0x6E000200, "Unknown",
                "",
                new string[] { "Undefined", "Undefined" },
                new string[] { "Undefined.", "Undefined" },
                "",
                new long[] { 0, 0 }));
            _e.Add(0x000A0500, new EventInformation(0x000A0500, "Unknown",
                "",
                new string[] { },
                new string[] { },
                "If \\value(0): \\value(1); Unknown=\\value(2), \\value(3), & \\value(4)",
                new long[] { }));
            _e.Add(0x000B0500, new EventInformation(0x000B0500, "Unknown",
                "",
                new string[] { },
                new string[] { },
                "Or \\value(0): \\value(1); Unknown=\\value(2), \\value(3), & \\value(4)",
                new long[] { }));
            _e.Add(0x000D0500, new EventInformation(0x000D0500, "Unknown",
                "",
                new string[] { },
                new string[] { },
                "Else If \\value(0): \\value(1); Unknown=\\value(2), \\value(3), & \\value(4)",
                new long[] { }));
            _e.Add(0x02050600, new EventInformation(02050600, "Actions 05 Compare",
                "Undefined.",
                new string[] { "Interrupt ID?", "Status ID?", "Requirement", "Variable", "Comparison", "Variable" },
                new string[] { "Undefined.", "Undefined.", "Undefined.", "First variable to compare.", "From 0 to 5: <, ≤, =, ≠, ≥, >.", "Second variable to compare." },
                "\\name(): Interrupt=\\unhex(\\value(0)), Status ID=\\unhex(\\value(1)), Requirement=\\value(2): \\value(3) \\cmpsign(\\value(4)) \\value(5)",
                new long[] { }));
            _e.Add(0x02050400, new EventInformation(02050400, "Actions 05 Value",
                "Undefined.",
                new string[] { "Interrupt ID?", "Status ID?", "Requirement", "Value" },
                new string[] { "Undefined.", "Undefined.", "Undefined.", "Value applied to the requirement.", },
                "\\name(): Interrupt=\\unhex(\\value(0)), Status ID=\\unhex(\\value(1)), Requirement=\\value(2): \\value(3)",
                new long[] { }));

            //Now add on to events with user inputted data

            StreamReader sr = null;
            long idNumber = 0;
            string loc, id;

            //Read known events and their descriptions.
            loc = Application.StartupPath + "/MovesetData/Events.txt";
            if (File.Exists(loc))
                using (sr = new StreamReader(loc))
                    for (int i = 0; !sr.EndOfStream; i++)
                    {
                        id = sr.ReadLine();
                        idNumber = Convert.ToInt32(id, 16);

                        if (!_e.Keys.Contains(idNumber))
                            _e.Add(idNumber, new EventInformation());

                        _e[idNumber]._id = idNumber;
                        _e[idNumber]._name = sr.ReadLine();
                        _e[idNumber]._description = sr.ReadLine();
                        _e[idNumber].SetDefaults(sr.ReadLine());
                        sr.ReadLine();
                    }

            //Read known parameters and their descriptions.
            loc = Application.StartupPath + "/MovesetData/Parameters.txt";
            if (File.Exists(loc))
                using (sr = new StreamReader(loc))
                    for (int i = 0; !sr.EndOfStream; i++)
                    {
                        id = sr.ReadLine();
                        idNumber = Convert.ToInt32(id, 16);

                        if (!_e.Keys.Contains(idNumber))
                            _e.Add(idNumber, new EventInformation());

                        for (int i2 = 0; ; i2++)
                        {
                            string name = sr.ReadLine();
                            if (name == null) name = "";

                            if (name != "")
                            {
                                Array.Resize<string>(ref _e[idNumber]._paramNames, i2 + 1);
                                Array.Resize<string>(ref _e[idNumber]._paramDescs, i2 + 1);
                                _e[idNumber]._paramNames[i2] = name;
                                _e[idNumber]._paramDescs[i2] = sr.ReadLine();
                            }
                            else break;
                        }
                    }

            //Read the list containing the syntax to display each event with.
            loc = Application.StartupPath + "/MovesetData/EventSyntax.txt";
            if (File.Exists(loc))
                using (sr = new StreamReader(loc))
                    for (int i = 0; !sr.EndOfStream; i++)
                    {
                        string syntax = "";
                        id = sr.ReadLine();
                        try { idNumber = Convert.ToInt32(id, 16); }
                        catch { syntax = id; goto AddSyntax; } //Most likely syntax where the id should be

                        //Clear the existing syntax
                        _e[idNumber]._syntax = "";

                        syntax = sr.ReadLine();

                        if (!_e.Keys.Contains(idNumber))
                            _e.Add(idNumber, new EventInformation());

                        _e[idNumber]._id = idNumber;

                    AddSyntax:
                        while (syntax != "" && syntax != null)
                        {
                            _e[idNumber]._syntax += syntax;
                            syntax = sr.ReadLine();
                        }
                    }

            Dictionary<long, List<int>> list = new Dictionary<long, List<int>>();
            List<string> enums = new List<string>();
            loc = Application.StartupPath + "/MovesetData/Enums.txt";
            if (File.Exists(loc))
                using (sr = new StreamReader(loc))
                    while (!sr.EndOfStream)
                    {
                        list = new Dictionary<long, List<int>>();
                        enums = new List<string>();
                        while (!String.IsNullOrEmpty(id = sr.ReadLine()))
                        {
                            idNumber = Convert.ToInt32(id, 16);

                            if (!list.ContainsKey(idNumber))
                                list.Add(idNumber, new List<int>());

                            string p = null;
                            while (!String.IsNullOrEmpty(p = sr.ReadLine()))
                                list[idNumber].Add(int.Parse(p));
                        }

                        string val = null;
                        while (!String.IsNullOrEmpty(val = sr.ReadLine()))
                            enums.Add(val);

                        foreach (long ev in list.Keys)
                            if (_e.ContainsKey(ev))
                            {
                                _e[ev]._enums = new Dictionary<int, List<string>>();
                                foreach (int p in list[ev])
                                    _e[ev]._enums.Add(p, enums);
                            }
                    }

            //CreateEventDictionary();
        }

        /// <summary>
        /// Write out the code for the internal event dictionary
        /// </summary>
        public static void CreateEventDictionary()
        {
            string p1 = "_e.Add(";
            //idNumber
            string p2 = ", new ActionEventInfo(";
            //idNumber
            string p3 = ", \"";
            //name
            string p4 = "\",\n    \"";
            //description
            string p5 = "\",\n    new string[] {";
            //param name array
            string p6 = " },\n    new string[] {";
            //param description array
            string p7 = " },\n    \"";
            //syntax
            string p8 = "\",\n    new long[] {";
            //default params
            string p9 = " }));";

            string dic = "", idString = "";
            foreach (EventInformation i in _e.Values)
            {
                idString = "0x" + i._id.ToString("X").PadLeft(8, '0');
                dic += p1 + idString + p2 + idString + p3 + i._name.Replace("\"", "\\\"") + p4 + i._description.Replace("\"", "\\\"") + p5;
                bool first = true;
                if (i._paramNames != null)
                    foreach (string s in i._paramNames)
                    {
                        if (!first) dic += ",";
                        else first = false;
                        dic += " \"" + s.Replace("\"", "\\\"") + "\"";
                    }
                else dic += " ";
                dic += p6;
                first = true;
                if (i._paramDescs != null)
                    foreach (string s in i._paramDescs)
                    {
                        if (!first) dic += ",";
                        else first = false;
                        dic += " \"" + s.Replace("\"", "\\\"") + "\"";
                    }
                else dic += " ";
                dic += p7 + i._syntax.Replace("\\", "\\\\") + p8;
                first = true;
                if (i._defaultParams != null)
                    foreach (long s in i._defaultParams)
                    {
                        if (!first) dic += ",";
                        else first = false;
                        dic += " " + s;
                    }
                else dic += " ";
                dic += p9;
                Console.WriteLine(dic);
                dic = "";
            }
        }

        #endregion

        #region Other Data

        public static string[] iRequirements = null;
        public static string[] iAirGroundStats = null;
        public static string[] iCollisionStats = null;
        public static string[] iGFXFiles = null;
        public static AttributeInfo[] AttributeArray = null;
        public static Dictionary<string, SectionParamInfo> Params = null;

        public static bool _attributesChanged = false;

        public static void LoadOtherData()
        {
            StreamReader sr = null;
            string loc;

            //Read the list of Event Requirements.
            loc = Application.StartupPath + "/MovesetData/Requirements.txt";
            if (File.Exists(loc))
                using (sr = new StreamReader(loc))
                    for (int i = 0; !sr.EndOfStream; i++)
                    {
                        Array.Resize<string>(ref iRequirements, i + 1);
                        iRequirements[i] = sr.ReadLine();
                    }
            else
            {
                iRequirements = new string[124];
                iRequirements[0] = "Character Exists?";
                iRequirements[1] = "Animation End";
                iRequirements[2] = "Animation has looped?";
                iRequirements[3] = "On Ground";
                iRequirements[4] = "In Air";
                iRequirements[5] = "Holding a Ledge";
                iRequirements[6] = "On a Passable Floor";
                iRequirements[7] = "Compare";
                iRequirements[8] = "Bit is Set";
                iRequirements[9] = "Facing Right";
                iRequirements[10] = "Facing Left";
                iRequirements[11] = "Hitbox Connects";
                iRequirements[12] = "Touching a Floor, Wall, or Ceiling";
                iRequirements[13] = "Is Throwing Someone";
                iRequirements[14] = "0E";
                iRequirements[15] = "Button Tap?";
                iRequirements[16] = "10";
                iRequirements[17] = "11";
                iRequirements[18] = "12";
                iRequirements[19] = "13";
                iRequirements[20] = "Entering Hitlag; Is in Hitlag";
                iRequirements[21] = "Article Exists";
                iRequirements[22] = "Is Overstepping an Edge";
                iRequirements[23] = "Has a Floor Below the Player";
                iRequirements[24] = "18";
                iRequirements[25] = "19";
                iRequirements[26] = "1A";
                iRequirements[27] = "Change in Air/Ground State";
                iRequirements[28] = "Article Available";
                iRequirements[29] = "1D";
                iRequirements[30] = "1E";
                iRequirements[31] = "Holding Item";
                iRequirements[32] = "Holding Item of Type";
                iRequirements[33] = "Light Item is in Grabbing Range";
                iRequirements[34] = "Heavy Item is in Grabbing Range";
                iRequirements[35] = "Item of Type(?) is in Grabbing Range";
                iRequirements[36] = "Turning with Item";
                iRequirements[37] = "25";
                iRequirements[38] = "26";
                iRequirements[39] = "27";
                iRequirements[40] = "28";
                iRequirements[41] = "29";
                iRequirements[42] = "Is in Water";
                iRequirements[43] = "Roll A Die";
                iRequirements[44] = "Subaction Exists";
                iRequirements[45] = "2D";
                iRequirements[46] = "Button Mashing? Status Expired (sleep/bury/freeze)?";
                iRequirements[47] = "Is Not in Damaging Magnifier";
                iRequirements[48] = "Button Press";
                iRequirements[49] = "Button Release";
                iRequirements[50] = "Button Pressed";
                iRequirements[51] = "Button Not Pressed";
                iRequirements[52] = "Stick Direction Pressed?";
                iRequirements[53] = "Stick Direction Not Pressed?";
                iRequirements[54] = "36";
                iRequirements[55] = "Is Being Thrown by Someone (1)";
                iRequirements[56] = "Is Being Thrown by Someone (2)";
                iRequirements[57] = "Hasn't Tethered 3 Times";
                iRequirements[58] = "Has Passed Over an Edge (Forward)";
                iRequirements[59] = "Has Passed Over an Edge (Backward)";
                iRequirements[60] = "Is Holding Someone in a Grab";
                iRequirements[61] = "Hitbox has Connected";
                iRequirements[62] = "3E";
                iRequirements[63] = "3F";
                iRequirements[64] = "40";
                iRequirements[65] = "41";
                iRequirements[66] = "42";
                iRequirements[67] = "43";
                iRequirements[68] = "44";
                iRequirements[69] = "45";
                iRequirements[70] = "46";
                iRequirements[71] = "Pick up an Item";
                iRequirements[72] = "48";
                iRequirements[73] = "49";
                iRequirements[74] = "4A";
                iRequirements[75] = "4B";
                iRequirements[76] = "Has Been Hit by Cape Effect";
                iRequirements[77] = "4D";
                iRequirements[78] = "4E";
                iRequirements[79] = "4F";
                iRequirements[80] = "50";
                iRequirements[81] = "51";
                iRequirements[82] = "52";
                iRequirements[83] = "53";
                iRequirements[84] = "54";
                iRequirements[85] = "55";
                iRequirements[86] = "56";
                iRequirements[87] = "57";
                iRequirements[88] = "58";
                iRequirements[89] = "59";
                iRequirements[90] = "5A";
                iRequirements[91] = "5B";
                iRequirements[92] = "5C";
                iRequirements[93] = "5D";
                iRequirements[94] = "5E";
                iRequirements[95] = "5F";
                iRequirements[96] = "60";
                iRequirements[97] = "61";
                iRequirements[98] = "62";
                iRequirements[99] = "63";
                iRequirements[100] = "64";
                iRequirements[101] = "270F";
                iRequirements[102] = "2710";
                iRequirements[103] = "In walljump situation";
                iRequirements[104] = "In wallcling situation";
                iRequirements[105] = "Is within footstool range.";
                iRequirements[106] = "2714";
                iRequirements[107] = "2715";
                iRequirements[108] = "Is falling+hit down";
                iRequirements[109] = "Has Smash Ball";
                iRequirements[110] = "2718";
                iRequirements[111] = "Can Pick Up Another Item";
                iRequirements[112] = "271A";
                iRequirements[113] = "271B";
                iRequirements[114] = "271C";
                iRequirements[115] = "F-Smash Shortcut";
                iRequirements[116] = "271E";
                iRequirements[117] = "271F";
                iRequirements[118] = "2720";
                iRequirements[119] = "2721";
                iRequirements[120] = "2722";
                iRequirements[121] = "2723";
                iRequirements[122] = "2724";
                iRequirements[123] = "Tap Jump on";
            }

            for (int i = 0; i < iRequirements.Length; i++)
            {
                string s2 = iRequirements[i];
                if (!String.IsNullOrEmpty(s2))
                    iRequirements[i] = Manager.TextInfo.ToTitleCase(s2.Replace("(", "").Replace(")", "")).Replace(" ", "");
            }

            //Read the list of Air Ground Stats.
            loc = Application.StartupPath + "/MovesetData/AirGroundStats.txt";
            if (File.Exists(loc))
                using (sr = new StreamReader(loc))
                    for (int i = 0; !sr.EndOfStream; i++)
                    {
                        Array.Resize<string>(ref iAirGroundStats, i + 1);
                        iAirGroundStats[i] = sr.ReadLine();
                    }
            else
            {
                iAirGroundStats = new string[7];
                iAirGroundStats[0] = "In Air";
                iAirGroundStats[1] = "Floating On Ground";
                iAirGroundStats[2] = "Undefined(2)";
                iAirGroundStats[3] = "Undefined(3)";
                iAirGroundStats[4] = "Undefined(4)";
                iAirGroundStats[5] = "Undefined(5)";
                iAirGroundStats[6] = "On Ground";
            }

            //Read the list of Collision Stats.
            loc = Application.StartupPath + "/MovesetData/CollisionStats.txt";
            if (File.Exists(loc))
                using (sr = new StreamReader(loc))
                    for (int i = 0; !sr.EndOfStream; i++)
                    {
                        Array.Resize<string>(ref iCollisionStats, i + 1);
                        iCollisionStats[i] = sr.ReadLine();
                    }
            else
            {
                iCollisionStats = new string[5];
                iCollisionStats[0] = "Normal";
                iCollisionStats[1] = "Invincible";
                iCollisionStats[2] = "Intangible";
                iCollisionStats[3] = "Intangible No Flashing";
                iCollisionStats[4] = "Intangible Quick Flashing";
            }

            //Read the list of GFX Files.
            loc = Application.StartupPath + "/MovesetData/GFXFiles.txt";
            if (File.Exists(loc))
                using (sr = new StreamReader(loc))
                    for (int i = 0; !sr.EndOfStream; i++)
                    {
                        Array.Resize<string>(ref iGFXFiles, i + 1);
                        iGFXFiles[i] = sr.ReadLine();
                    }
            else
            {
                iGFXFiles = new string[305];
                iGFXFiles[0] = "Common";
                iGFXFiles[1] = "Mario";
                iGFXFiles[2] = "DK";
                iGFXFiles[3] = "Link ";
                iGFXFiles[4] = "Samus";
                iGFXFiles[5] = "Yoshi";
                iGFXFiles[6] = "Kirby";
                iGFXFiles[7] = "Fox";
                iGFXFiles[8] = "Pikachu";
                iGFXFiles[9] = "Luigi";
                iGFXFiles[10] = "Falcon";
                iGFXFiles[11] = "Ness";
                iGFXFiles[12] = "Bowser";
                iGFXFiles[13] = "Peach";
                iGFXFiles[14] = "Zelda";
                iGFXFiles[15] = "Sheik";
                iGFXFiles[16] = "Popo";
                iGFXFiles[17] = "Nana (nulled)";
                iGFXFiles[18] = "Marth";
                iGFXFiles[19] = "Game&Watch";
                iGFXFiles[20] = "Falco";
                iGFXFiles[21] = "Ganondorf";
                iGFXFiles[22] = "Wario";
                iGFXFiles[23] = "Meta Knight";
                iGFXFiles[24] = "Pit";
                iGFXFiles[25] = "Zamus";
                iGFXFiles[26] = "Olimar";
                iGFXFiles[27] = "Lucas";
                iGFXFiles[28] = "Diddy";
                iGFXFiles[29] = "Pokemon Trainer";
                iGFXFiles[30] = "Charizard";
                iGFXFiles[31] = "Squirtle";
                iGFXFiles[32] = "Ivysaur";
                iGFXFiles[33] = "Dedede";
                iGFXFiles[34] = "Lucario";
                iGFXFiles[35] = "Ike";
                iGFXFiles[36] = "Rob";
                iGFXFiles[37] = "Pramai (nulled)";
                iGFXFiles[38] = "Jigglypuff";
                iGFXFiles[39] = "Mewtwo (nulled)";
                iGFXFiles[40] = "Roy (nulled)";
                iGFXFiles[41] = "Dr. Mario (nulled)";
                iGFXFiles[42] = "Toon Link";
                iGFXFiles[43] = "Toon Zelda (nulled)";
                iGFXFiles[44] = "Toon Sheik (nulled)";
                iGFXFiles[45] = "Wolf";
                iGFXFiles[46] = "Dixie(nulled)";
                iGFXFiles[47] = "Snake";
                iGFXFiles[48] = "Sonic";
                iGFXFiles[49] = "Giga Bowser";
                iGFXFiles[50] = "Battlefield";
                iGFXFiles[51] = "Final Destination";
                iGFXFiles[52] = "Delfino Plaza";
                iGFXFiles[53] = "Luigi's Mansion";
                iGFXFiles[54] = "Mushroomy Kingdom";
                iGFXFiles[55] = "Mario Circuit";
                iGFXFiles[56] = "75m";
                iGFXFiles[57] = "Rumble Falls";
                iGFXFiles[58] = "Pirate Ship";
                iGFXFiles[59] = "-(nulled)";
                iGFXFiles[60] = "Norfair";
                iGFXFiles[61] = "Frigate Orpheon";
                iGFXFiles[62] = "Yoshi's Island (Brawl)";
                iGFXFiles[63] = "Halberd";
                iGFXFiles[64] = "-(nulled)";
                iGFXFiles[65] = "-(nulled)";
                iGFXFiles[66] = "-(nulled)";
                iGFXFiles[67] = "-(nulled)";
                iGFXFiles[68] = "Lylat Cruise";
                iGFXFiles[69] = "Pokemon Stadium 2";
                iGFXFiles[70] = "Spear Pillar";
                iGFXFiles[71] = "Port Town Aero Dive";
                iGFXFiles[72] = "Summit";
                iGFXFiles[73] = "Flat Zone 2";
                iGFXFiles[74] = "Castle Siege";
                iGFXFiles[75] = "Castle Siege_00";
                iGFXFiles[76] = "Castle Siege_01";
                iGFXFiles[77] = "WarioWare Inc.";
                iGFXFiles[78] = "Distant Planet";
                iGFXFiles[79] = "Skyworld";
                iGFXFiles[80] = "Mario Bros.";
                iGFXFiles[81] = "New Pork City";
                iGFXFiles[82] = "Smashville";
                iGFXFiles[83] = "Shadow Moses Island";
                iGFXFiles[84] = "Green Hill Zone";
                iGFXFiles[85] = "Pictochat";
                iGFXFiles[86] = "Hanenbow";
                iGFXFiles[87] = "-(nulled)";
                iGFXFiles[88] = "-(nulled)";
                iGFXFiles[89] = "-(nulled)";
                iGFXFiles[90] = "Temple";
                iGFXFiles[91] = "Yoshi's Island (Melee)";
                iGFXFiles[92] = "Jungle Japes";
                iGFXFiles[93] = "Onett";
                iGFXFiles[94] = "Green Greens";
                iGFXFiles[95] = "Pokemon Stadium";
                iGFXFiles[96] = "Rainbow Cruise";
                iGFXFiles[97] = "Corneria";
                iGFXFiles[98] = "Big Blue";
                iGFXFiles[99] = "Brinstar";
                iGFXFiles[100] = "Bridge of Eldin";
                iGFXFiles[101] = "Homerun";
                iGFXFiles[102] = "Edit";
                iGFXFiles[103] = "AdvCloud";
                iGFXFiles[104] = "AdvJungle";
                iGFXFiles[105] = "AdvRiver";
                iGFXFiles[106] = "AdvGrass";
                iGFXFiles[107] = "AdvZoo";
                iGFXFiles[108] = "AdvFortress";
                iGFXFiles[109] = "AdvLakeside";
                iGFXFiles[110] = "AdvCave";
                iGFXFiles[111] = "AdvRuinfront";
                iGFXFiles[112] = "AdvRuin";
                iGFXFiles[113] = "AdvWild";
                iGFXFiles[114] = "AdvCliff";
                iGFXFiles[115] = "AdvHalberdOut";
                iGFXFiles[116] = "AdvHalberdIn";
                iGFXFiles[117] = "AdvAncientOut";
                iGFXFiles[118] = "AdvFactory";
                iGFXFiles[119] = "AdvDimension";
                iGFXFiles[120] = "AdvStadium";
                iGFXFiles[121] = "AdvHalberdSide";
                iGFXFiles[122] = "AdvStore";
                iGFXFiles[123] = "AdvFlyingPlate";
                iGFXFiles[124] = "AdvEscape";
                iGFXFiles[125] = "Goomba";
                iGFXFiles[126] = "Paratroopa";
                iGFXFiles[127] = "Hammer Bro (SSE enemy)";
                iGFXFiles[128] = "Bullet Bill";
                iGFXFiles[129] = "Met";
                iGFXFiles[130] = "Dry Bones";
                iGFXFiles[131] = "Giant Goomba";
                iGFXFiles[132] = "Blown";
                iGFXFiles[133] = "Ploum";
                iGFXFiles[134] = "Gal";
                iGFXFiles[135] = "Galthunder";
                iGFXFiles[136] = "Galfire";
                iGFXFiles[137] = "Galice";
                iGFXFiles[138] = "Melorin";
                iGFXFiles[139] = "Popperam";
                iGFXFiles[140] = "Whauel";
                iGFXFiles[141] = "Bitan";
                iGFXFiles[142] = "Mechcannon";
                iGFXFiles[143] = "Mizuo";
                iGFXFiles[144] = "Roada";
                iGFXFiles[145] = "Bombhead";
                iGFXFiles[146] = "Blossa";
                iGFXFiles[147] = "Gyraan";
                iGFXFiles[148] = "Bucyulus";
                iGFXFiles[149] = "Tautau";
                iGFXFiles[150] = "Bubot";
                iGFXFiles[151] = "Flows";
                iGFXFiles[152] = "Aroaros";
                iGFXFiles[153] = "Botron";
                iGFXFiles[154] = "Jyakeel";
                iGFXFiles[155] = "Dyeburn";
                iGFXFiles[156] = "Torista";
                iGFXFiles[157] = "Wiiems";
                iGFXFiles[158] = "Ghamgha";
                iGFXFiles[159] = "Kyan";
                iGFXFiles[160] = "Pacci";
                iGFXFiles[161] = "Faulong";
                iGFXFiles[162] = "Deathpod";
                iGFXFiles[163] = "Byushi";
                iGFXFiles[164] = "Spar";
                iGFXFiles[165] = "Konkon";
                iGFXFiles[166] = "Jdus";
                iGFXFiles[167] = "Arrians";
                iGFXFiles[168] = "Mite";
                iGFXFiles[169] = "Shelly";
                iGFXFiles[170] = "Ngagog";
                iGFXFiles[171] = "Gunnatter";
                iGFXFiles[172] = "Cymal";
                iGFXFiles[173] = "Teckin";
                iGFXFiles[174] = "Cataguard";
                iGFXFiles[175] = "Siralamos";
                iGFXFiles[176] = "Boobas";
                iGFXFiles[177] = "Arman";
                iGFXFiles[178] = "Prim";
                iGFXFiles[179] = "Waddle Dee";
                iGFXFiles[180] = "Waddle Doo";
                iGFXFiles[181] = "Blade Knight";
                iGFXFiles[182] = "Bronto Burt";
                iGFXFiles[183] = "Robo";
                iGFXFiles[184] = "Bonkers";
                iGFXFiles[185] = "Petey Piranha";
                iGFXFiles[186] = "Rayquaza";
                iGFXFiles[187] = "Porkystatue";
                iGFXFiles[188] = "Porky";
                iGFXFiles[189] = "Galleom";
                iGFXFiles[190] = "Ridley";
                iGFXFiles[191] = "Duon";
                iGFXFiles[192] = "Meta Ridley";
                iGFXFiles[193] = "Tabuu";
                iGFXFiles[194] = "Masterhand";
                iGFXFiles[195] = "Crazyhand";
                iGFXFiles[196] = "Falconflyer";
                iGFXFiles[197] = "Lakitu";
                iGFXFiles[198] = "Samurai Goroh";
                iGFXFiles[199] = "Knuckle Joe";
                iGFXFiles[200] = "Waluigi";
                iGFXFiles[201] = "Mr. Resetti";
                iGFXFiles[202] = "Nintendog";
                iGFXFiles[203] = "Gray Fox";
                iGFXFiles[204] = "Shadow";
                iGFXFiles[205] = "Excitebike";
                iGFXFiles[206] = "Devil";
                iGFXFiles[207] = "Hammer Bro (assist trophy)";
                iGFXFiles[208] = "Metroid";
                iGFXFiles[209] = "Ridley (nulled assist trophy)";
                iGFXFiles[210] = "Dr. Wright";
                iGFXFiles[211] = "Starfy";
                iGFXFiles[212] = "Tingle";
                iGFXFiles[213] = "Kat & Ana";
                iGFXFiles[214] = "Lyn";
                iGFXFiles[215] = "Custom Robo";
                iGFXFiles[216] = "Little Mac";
                iGFXFiles[217] = "Soldier";
                iGFXFiles[218] = "Jeff";
                iGFXFiles[219] = "Helirin";
                iGFXFiles[220] = "Barbara";
                iGFXFiles[221] = "Isaac";
                iGFXFiles[222] = "Saki";
                iGFXFiles[223] = "Kururi";
                iGFXFiles[224] = "Mario FS";
                iGFXFiles[225] = "DK FS";
                iGFXFiles[226] = "Link FS";
                iGFXFiles[227] = "Toon Link FS";
                iGFXFiles[228] = "Samus FS";
                iGFXFiles[229] = "Yoshi FS";
                iGFXFiles[230] = "Kirby FS";
                iGFXFiles[231] = "FoxFalco FS";
                iGFXFiles[232] = "Pikachu FS";
                iGFXFiles[233] = "Luigi FS";
                iGFXFiles[234] = "Falcon FS";
                iGFXFiles[235] = "Ness FS";
                iGFXFiles[236] = "Bowser FS";
                iGFXFiles[237] = "Peach FS";
                iGFXFiles[238] = "ZeldaSheik FS";
                iGFXFiles[239] = "PopoNana FS";
                iGFXFiles[240] = "Marth FS";
                iGFXFiles[241] = "Game&Watch FS";
                iGFXFiles[242] = "Ganondorf FS";
                iGFXFiles[243] = "Wario FS";
                iGFXFiles[244] = "Meta Knight FS";
                iGFXFiles[245] = "Pit FS";
                iGFXFiles[246] = "Zamus FS";
                iGFXFiles[247] = "Olimar FS";
                iGFXFiles[248] = "Lucas FS";
                iGFXFiles[249] = "Diddy FS";
                iGFXFiles[250] = "Pokemon Trainer FS";
                iGFXFiles[251] = "Dedede FS";
                iGFXFiles[252] = "Lucario FS";
                iGFXFiles[253] = "Ike FS";
                iGFXFiles[254] = "Rob FS";
                iGFXFiles[255] = "Jigglypuff FS";
                iGFXFiles[256] = "Wolf FS";
                iGFXFiles[257] = "Snake FS";
                iGFXFiles[258] = "Sonic FS";
                iGFXFiles[259] = "SYS_ADV";
                iGFXFiles[260] = "SYS_Pokemon";
                iGFXFiles[261] = "Kirby Mario";
                iGFXFiles[262] = "Kirby DK";
                iGFXFiles[263] = "Kirby Link";
                iGFXFiles[264] = "Kirby Samus";
                iGFXFiles[265] = "Kirby Yoshi";
                iGFXFiles[266] = "Kirby Fox";
                iGFXFiles[267] = "Kirby Pikachu";
                iGFXFiles[268] = "Kirby Luigi";
                iGFXFiles[269] = "Kirby Falcon";
                iGFXFiles[270] = "Kirby Ness";
                iGFXFiles[271] = "Kirby Koopa";
                iGFXFiles[272] = "Kirby Peach";
                iGFXFiles[273] = "Kirby Zelda";
                iGFXFiles[274] = "Kirby Sheik";
                iGFXFiles[275] = "Kirby Popo";
                iGFXFiles[276] = "Kirby Marth";
                iGFXFiles[277] = "Kirby Game&Watch";
                iGFXFiles[278] = "Kirby Falco";
                iGFXFiles[279] = "Kirby Ganondorf";
                iGFXFiles[280] = "Kirby Wario";
                iGFXFiles[281] = "Kirby Meta Knight";
                iGFXFiles[282] = "Kirby Pit";
                iGFXFiles[283] = "Kirby Zamus";
                iGFXFiles[284] = "Kirby Olimar";
                iGFXFiles[285] = "Kirby Lucas";
                iGFXFiles[286] = "Kirby Diddy";
                iGFXFiles[287] = "Kirby Charizard";
                iGFXFiles[288] = "Kirby Squirtle";
                iGFXFiles[289] = "Kirby Ivysaur";
                iGFXFiles[290] = "Kirby Dedede";
                iGFXFiles[291] = "Kirby Lucario";
                iGFXFiles[292] = "Kirby Ike";
                iGFXFiles[293] = "Kirby R.O.B.";
                iGFXFiles[294] = "Kirby Jigglypuff";
                iGFXFiles[295] = "Kirby Toon Link";
                iGFXFiles[296] = "Kirby Wolf";
                iGFXFiles[297] = "Kirby Snake";
                iGFXFiles[298] = "Kirby Sonic";
                iGFXFiles[299] = "Target Smash";
                iGFXFiles[300] = "Warioman (unused)";
                iGFXFiles[301] = "Red Alloy (unused)";
                iGFXFiles[302] = "Blue Alloy (unused)";
                iGFXFiles[303] = "Yellow Alloy (unused)";
                iGFXFiles[304] = "Green Alloy (unused)";
            }

            for (int i = 0; i < iGFXFiles.Length; i++)
            {
                string s1 = iGFXFiles[i];
                if (!String.IsNullOrEmpty(s1))
                    iGFXFiles[i] = s1.Replace("(", "").Replace(")", "").Replace(" ", "");
            }

            //string s = "iGFXFiles = new string[" + iGFXFiles.Length + "];";
            //string e = "";
            //int x = 0;
            //foreach (string v in iGFXFiles)
            //    e += "\niGFXFiles[" + x++ + "] = \"" + v + "\";";
            //Console.WriteLine(s + e);

            //string s = "iRequirements = new string[" + iRequirements.Length + "];";
            //string e = "";
            //int x = 0;
            //foreach (string v in iRequirements)
            //    e += "\niRequirements[" + x++ + "] = \"" + v + "\";";
            //Console.WriteLine(s + e);

            //s = "iAirGroundStats = new string[" + iAirGroundStats.Length + "];";
            //e = "";
            //x = 0;
            //foreach (string v in iAirGroundStats)
            //    e += "\niAirGroundStats[" + x++ + "] = \"" + v + "\";";
            //Console.WriteLine(s + e);

            //s = "iCollisionStats = new string[" + iCollisionStats.Length + "];";
            //e = "";
            //x = 0;
            //foreach (string v in iCollisionStats)
            //    e += "\niCollisionStats[" + x++ + "] = \"" + v + "\";";
            //Console.WriteLine(s + e);

            AttributeArray = new AttributeInfo[185];
            sr = null;
            loc = Application.StartupPath + "/MovesetData/Attributes.txt";

            //Read known attributes and their descriptions.
            if (File.Exists(loc))
                using (sr = new StreamReader(loc))
                    for (int i = 0; !sr.EndOfStream && i < 185; i++)
                    {
                        AttributeArray[i] = new AttributeInfo();
                        AttributeArray[i]._name = sr.ReadLine();
                        AttributeArray[i]._description = sr.ReadLine();
                        AttributeArray[i]._type = int.Parse(sr.ReadLine());

                        if (AttributeArray[i]._description == "")
                            AttributeArray[i]._description = "No Description Available.";

                        sr.ReadLine();
                    }
            else
            {
                AttributeArray = new AttributeInfo[185];
                AttributeArray[0] = new AttributeInfo() { _name = "0x000 Walk Initial Velocity", _description = "The starting velocity obtained the moment the character starts walking.", _type = 0 };
                AttributeArray[1] = new AttributeInfo() { _name = "0x004 Walk Acceleration", _description = "The speed of acceleration while walking.", _type = 0 };
                AttributeArray[2] = new AttributeInfo() { _name = "0x008 Walk Maximum Velocity", _description = "The maximum velocity obtainable while walking.", _type = 0 };
                AttributeArray[3] = new AttributeInfo() { _name = "0x00C Stopping Velocity", _description = "The speed at which the character is able to stop at.", _type = 0 };
                AttributeArray[4] = new AttributeInfo() { _name = "0x010 Dash & StopTurn Initial Velocity", _description = "The starting velocity obtained the moment the character starts a Dash.", _type = 0 };
                AttributeArray[5] = new AttributeInfo() { _name = "0x014 StopTurn Deceleration", _description = "The speed at which the character decelerates upon performing a StopTurn.", _type = 0 };
                AttributeArray[6] = new AttributeInfo() { _name = "0x018 StopTurn Acceleration", _description = "The speed at which the character accelerates after performing a StopTurn", _type = 0 };
                AttributeArray[7] = new AttributeInfo() { _name = "0x01C Run Initial Velocity", _description = "The starting velocity obtained the moment the Dash turns into a Run.", _type = 0 };
                AttributeArray[8] = new AttributeInfo() { _name = "0x020 Run Acceleration?", _description = "Possibly the time it takes for the character to reach full speed during a run?", _type = 0 };
                AttributeArray[9] = new AttributeInfo() { _name = "0x024", _description = "No Description Available.", _type = 0 };
                AttributeArray[10] = new AttributeInfo() { _name = "*0x028 Dash Cancel Frame Window?", _description = "Possibly the amount of frames you have to cancel your dash animation.", _type = 1 };
                AttributeArray[11] = new AttributeInfo() { _name = "0x02C Guard On Max Momentum", _description = "The maximum horizontal momentum you can have when entering shield.", _type = 0 };
                AttributeArray[12] = new AttributeInfo() { _name = "*0x030 Jump Startup Time", _description = "The time in frames it takes for your character to life off of the ground when jumping.", _type = 1 };
                AttributeArray[13] = new AttributeInfo() { _name = "0x034 Jump H Initial Velocity", _description = "The starting horizontal velocity the character obtains when jumping.", _type = 0 };
                AttributeArray[14] = new AttributeInfo() { _name = "0x038 Jump V Initial Velocity", _description = "The starting vertical velocity the character obtains when jumping.", _type = 0 };
                AttributeArray[15] = new AttributeInfo() { _name = "0x03C Ground to Air Jump Momentum Multiplier", _description = "The amount of horizontal momentum from the ground that gets added onto a jump.", _type = 0 };
                AttributeArray[16] = new AttributeInfo() { _name = "0x040 Jump H Maximum Velocity", _description = "The maximum horizontal velocity the character is able to obtain at the start of a jump.", _type = 0 };
                AttributeArray[17] = new AttributeInfo() { _name = "0x044 Hop V Initial Velocity", _description = "The maximum vertical velocity the character obtains when hopping.", _type = 0 };
                AttributeArray[18] = new AttributeInfo() { _name = "0x048 Air Jump Multiplier", _description = "The velocity obtained from an air jump proportional to the Jump V Initial Velocity.", _type = 0 };
                AttributeArray[19] = new AttributeInfo() { _name = "0x04C Some Kind of Multiplier?", _description = "No Description Available.", _type = 0 };
                AttributeArray[20] = new AttributeInfo() { _name = "0x050 Footstool V Initial Velocity", _description = "The starting vertical velocity the character obtains upon performing a footstool jump.", _type = 0 };
                AttributeArray[21] = new AttributeInfo() { _name = "0x054", _description = "No Description Available.", _type = 0 };
                AttributeArray[22] = new AttributeInfo() { _name = "0x058", _description = "No Description Available.", _type = 0 };
                AttributeArray[23] = new AttributeInfo() { _name = "0x05C", _description = "No Description Available.", _type = 0 };
                AttributeArray[24] = new AttributeInfo() { _name = "*0x060 Jumps", _description = "The number of consecutive jumps the character is able to perform.", _type = 1 };
                AttributeArray[25] = new AttributeInfo() { _name = "0x064 Gravity", _description = "The speed at which the character accelerates downward.", _type = 0 };
                AttributeArray[26] = new AttributeInfo() { _name = "0x068 Terminal Velocity", _description = "The maximum velocity obtainable due to gravity.", _type = 0 };
                AttributeArray[27] = new AttributeInfo() { _name = "0x06C", _description = "No Description Available.", _type = 0 };
                AttributeArray[28] = new AttributeInfo() { _name = "0x070", _description = "No Description Available.", _type = 0 };
                AttributeArray[29] = new AttributeInfo() { _name = "0x074 Air Mobility", _description = "The speed at which the character is able to maneuver in air.", _type = 0 };
                AttributeArray[30] = new AttributeInfo() { _name = "0x078 Air Stopping Mobility", _description = "The speed at which the character is able to stop themselves in air.", _type = 0 };
                AttributeArray[31] = new AttributeInfo() { _name = "0x07C Maximum H Air Velocity", _description = "The maximum horizontal velocity the character is able to obtain in air.", _type = 0 };
                AttributeArray[32] = new AttributeInfo() { _name = "0x080 Horizontal Momentum Decay", _description = "The speed at which the character's horizontal momentum decreases on its own.", _type = 0 };
                AttributeArray[33] = new AttributeInfo() { _name = "0x084 Fastfall Terminal Velocity", _description = "The initial fastfalling speed of the character. ", _type = 0 };
                AttributeArray[34] = new AttributeInfo() { _name = "0x088", _description = "No Description Available.", _type = 0 };
                AttributeArray[35] = new AttributeInfo() { _name = "*0x08C Glide Frame Window", _description = "The amount of time (in frames) that the character has to begin a glide after jumping. Leave at 0 for no glide.", _type = 1 };
                AttributeArray[36] = new AttributeInfo() { _name = "0x090", _description = "No Description Available.", _type = 0 };
                AttributeArray[37] = new AttributeInfo() { _name = "0x094", _description = "No Description Available.", _type = 0 };
                AttributeArray[38] = new AttributeInfo() { _name = "0x098", _description = "No Description Available.", _type = 0 };
                AttributeArray[39] = new AttributeInfo() { _name = "*0x09C Forward Tilt 2 Flag", _description = "?", _type = 1 };
                AttributeArray[40] = new AttributeInfo() { _name = "*0x0A0 Forward Tilt 3 Flag", _description = "?", _type = 1 };
                AttributeArray[41] = new AttributeInfo() { _name = "0x0A4 Forward Smash 2 Flag", _description = "No Description Available.", _type = 0 };
                AttributeArray[42] = new AttributeInfo() { _name = "0x0A8", _description = "No Description Available.", _type = 0 };
                AttributeArray[43] = new AttributeInfo() { _name = "0x0AC", _description = "No Description Available.", _type = 0 };
                AttributeArray[44] = new AttributeInfo() { _name = "0x0B0 Weight", _description = "How resiliant the character is to knockback.", _type = 0 };
                AttributeArray[45] = new AttributeInfo() { _name = "0x0B4 Size", _description = "The scaling of the character from their original model size. 1 = 100%, 0.5 = 50%.", _type = 0 };
                AttributeArray[46] = new AttributeInfo() { _name = "0x0B8 Size on Results Screen", _description = "The scaling of the character on the results screen. 1 = 100%, 0.5 = 50%.", _type = 0 };
                AttributeArray[47] = new AttributeInfo() { _name = "0x0BC", _description = "No Description Available.", _type = 0 };
                AttributeArray[48] = new AttributeInfo() { _name = "0x0C0", _description = "No Description Available.", _type = 0 };
                AttributeArray[49] = new AttributeInfo() { _name = "0x0C4 Shield Size", _description = "The size of the character's shield bubble when it is at full strength.", _type = 0 };
                AttributeArray[50] = new AttributeInfo() { _name = "0x0C8 Shield Break Bounce Velocity", _description = "The velocity at which the character bounces upwards upon having their shield broken.", _type = 0 };
                AttributeArray[51] = new AttributeInfo() { _name = "0x0CC", _description = "No Description Available.", _type = 0 };
                AttributeArray[52] = new AttributeInfo() { _name = "0x0D0", _description = "No Description Available.", _type = 0 };
                AttributeArray[53] = new AttributeInfo() { _name = "0x0D4", _description = "No Description Available.", _type = 0 };
                AttributeArray[54] = new AttributeInfo() { _name = "0x0D8", _description = "No Description Available.", _type = 0 };
                AttributeArray[55] = new AttributeInfo() { _name = "0x0DC", _description = "No Description Available.", _type = 0 };
                AttributeArray[56] = new AttributeInfo() { _name = "0x0E0", _description = "No Description Available.", _type = 0 };
                AttributeArray[57] = new AttributeInfo() { _name = "0x0E4", _description = "No Description Available.", _type = 0 };
                AttributeArray[58] = new AttributeInfo() { _name = "*0x0E8", _description = "No Description Available.", _type = 1 };
                AttributeArray[59] = new AttributeInfo() { _name = "*0x0EC", _description = "No Description Available.", _type = 1 };
                AttributeArray[60] = new AttributeInfo() { _name = "*0x0F0", _description = "No Description Available.", _type = 1 };
                AttributeArray[61] = new AttributeInfo() { _name = "0x0F4", _description = "No Description Available.", _type = 0 };
                AttributeArray[62] = new AttributeInfo() { _name = "0x0F8 Edge Jump H Velocity", _description = "The starting horizontal velocity obtained from an Edge Jump.", _type = 0 };
                AttributeArray[63] = new AttributeInfo() { _name = "0x0FC Edge Jump V Velocity", _description = "The starting vertical velocity obtained from an Edge Jump.", _type = 0 };
                AttributeArray[64] = new AttributeInfo() { _name = "0x100", _description = "No Description Available.", _type = 0 };
                AttributeArray[65] = new AttributeInfo() { _name = "0x104", _description = "No Description Available.", _type = 0 };
                AttributeArray[66] = new AttributeInfo() { _name = "0x108", _description = "No Description Available.", _type = 0 };
                AttributeArray[67] = new AttributeInfo() { _name = "0x10C", _description = "No Description Available.", _type = 0 };
                AttributeArray[68] = new AttributeInfo() { _name = "0x110", _description = "No Description Available.", _type = 0 };
                AttributeArray[69] = new AttributeInfo() { _name = "*0x114", _description = "No Description Available.", _type = 1 };
                AttributeArray[70] = new AttributeInfo() { _name = "0x118 Item Throw Strength", _description = "The speed at which an item is projected when thrown.", _type = 0 };
                AttributeArray[71] = new AttributeInfo() { _name = "0x11C", _description = "No Description Available.", _type = 0 };
                AttributeArray[72] = new AttributeInfo() { _name = "0x120", _description = "No Description Available.", _type = 0 };
                AttributeArray[73] = new AttributeInfo() { _name = "0x124", _description = "No Description Available.", _type = 0 };
                AttributeArray[74] = new AttributeInfo() { _name = "0x128 Projectile Weapon Use Move Speed", _description = "The speed at which the character is able to move at while firing a projectile weapon.", _type = 0 };
                AttributeArray[75] = new AttributeInfo() { _name = "0x12C Projectile Weapon Use F Dash Speed", _description = "The speed at which a character moves during a forward dash while firing a projectile weapon.", _type = 0 };
                AttributeArray[76] = new AttributeInfo() { _name = "0x130 Projectile Weapon Use B Dash Speed", _description = "The speed at which a character moves during a backward dash while firing a projectile weapon.", _type = 0 };
                AttributeArray[77] = new AttributeInfo() { _name = "0x134", _description = "No Description Available.", _type = 0 };
                AttributeArray[78] = new AttributeInfo() { _name = "0x138", _description = "No Description Available.", _type = 0 };
                AttributeArray[79] = new AttributeInfo() { _name = "0x13C Normal Landing Lag", _description = "The length of the character's normal landing lag in frames.", _type = 0 };
                AttributeArray[80] = new AttributeInfo() { _name = "0x140 Nair Landing Lag?", _description = "The length of the character's nair landing lag animation in frames.", _type = 0 };
                AttributeArray[81] = new AttributeInfo() { _name = "0x144 Fair Landing Lag?", _description = "The length of the character's fair landing lag animation in frames.", _type = 0 };
                AttributeArray[82] = new AttributeInfo() { _name = "0x148 Bair Landing Lag?", _description = "The length of the character's bair landing lag animation in frames.", _type = 0 };
                AttributeArray[83] = new AttributeInfo() { _name = "0x14C Uair Landing Lag?", _description = "The length of the character's uair landing lag animation in frames.", _type = 0 };
                AttributeArray[84] = new AttributeInfo() { _name = "0x150 Dair Landing Lag", _description = "The length of the character's dair landing lag animation in frames.", _type = 0 };
                AttributeArray[85] = new AttributeInfo() { _name = "*0x154", _description = "No Description Available.", _type = 1 };
                AttributeArray[86] = new AttributeInfo() { _name = "*0x158", _description = "No Description Available.", _type = 1 };
                AttributeArray[87] = new AttributeInfo() { _name = "0x15C", _description = "No Description Available.", _type = 0 };
                AttributeArray[88] = new AttributeInfo() { _name = "0x160", _description = "No Description Available.", _type = 0 };
                AttributeArray[89] = new AttributeInfo() { _name = "0x164 Walljump H Velocity", _description = "The starting horazontal velocity obtained from a Walljump.", _type = 0 };
                AttributeArray[90] = new AttributeInfo() { _name = "0x168 Walljump V Velocity", _description = "The starting vertical velocity obtained from a Walljump.", _type = 0 };
                AttributeArray[91] = new AttributeInfo() { _name = "0x16C", _description = "No Description Available.", _type = 0 };
                AttributeArray[92] = new AttributeInfo() { _name = "0x170", _description = "No Description Available.", _type = 0 };
                AttributeArray[93] = new AttributeInfo() { _name = "*0x174", _description = "No Description Available.", _type = 1 };
                AttributeArray[94] = new AttributeInfo() { _name = "0x178", _description = "No Description Available.", _type = 0 };
                AttributeArray[95] = new AttributeInfo() { _name = "0x17C", _description = "No Description Available.", _type = 0 };
                AttributeArray[96] = new AttributeInfo() { _name = "*0x180", _description = "No Description Available.", _type = 1 };
                AttributeArray[97] = new AttributeInfo() { _name = "*0x184", _description = "No Description Available.", _type = 1 };
                AttributeArray[98] = new AttributeInfo() { _name = "0x188", _description = "No Description Available.", _type = 0 };
                AttributeArray[99] = new AttributeInfo() { _name = "0x18C", _description = "No Description Available.", _type = 0 };
                AttributeArray[100] = new AttributeInfo() { _name = "0x190", _description = "No Description Available.", _type = 0 };
                AttributeArray[101] = new AttributeInfo() { _name = "0x194", _description = "No Description Available.", _type = 0 };
                AttributeArray[102] = new AttributeInfo() { _name = "0x198", _description = "No Description Available.", _type = 0 };
                AttributeArray[103] = new AttributeInfo() { _name = "0x19C", _description = "No Description Available.", _type = 0 };
                AttributeArray[104] = new AttributeInfo() { _name = "0x1A0", _description = "No Description Available.", _type = 0 };
                AttributeArray[105] = new AttributeInfo() { _name = "0x1A4", _description = "No Description Available.", _type = 0 };
                AttributeArray[106] = new AttributeInfo() { _name = "*0x1A8", _description = "No Description Available.", _type = 1 };
                AttributeArray[107] = new AttributeInfo() { _name = "0x1AC", _description = "No Description Available.", _type = 0 };
                AttributeArray[108] = new AttributeInfo() { _name = "*0x1B0", _description = "No Description Available.", _type = 1 };
                AttributeArray[109] = new AttributeInfo() { _name = "0x1B4", _description = "No Description Available.", _type = 0 };
                AttributeArray[110] = new AttributeInfo() { _name = "*0x1B8", _description = "No Description Available.", _type = 1 };
                AttributeArray[111] = new AttributeInfo() { _name = "*0x1BC", _description = "No Description Available.", _type = 1 };
                AttributeArray[112] = new AttributeInfo() { _name = "0x1C0", _description = "No Description Available.", _type = 0 };
                AttributeArray[113] = new AttributeInfo() { _name = "*0x1C4", _description = "No Description Available.", _type = 1 };
                AttributeArray[114] = new AttributeInfo() { _name = "0x1C8", _description = "No Description Available.", _type = 0 };
                AttributeArray[115] = new AttributeInfo() { _name = "*0x1CC", _description = "No Description Available.", _type = 1 };
                AttributeArray[116] = new AttributeInfo() { _name = "*0x1D0", _description = "No Description Available.", _type = 1 };
                AttributeArray[117] = new AttributeInfo() { _name = "*0x1D4", _description = "No Description Available.", _type = 1 };
                AttributeArray[118] = new AttributeInfo() { _name = "*0x1D8", _description = "No Description Available.", _type = 1 };
                AttributeArray[119] = new AttributeInfo() { _name = "0x1DC", _description = "No Description Available.", _type = 0 };
                AttributeArray[120] = new AttributeInfo() { _name = "*0x1E0", _description = "No Description Available.", _type = 1 };
                AttributeArray[121] = new AttributeInfo() { _name = "0x1E4", _description = "No Description Available.", _type = 0 };
                AttributeArray[122] = new AttributeInfo() { _name = "0x1E8", _description = "No Description Available.", _type = 0 };
                AttributeArray[123] = new AttributeInfo() { _name = "0x1EC", _description = "No Description Available.", _type = 0 };
                AttributeArray[124] = new AttributeInfo() { _name = "0x1F0", _description = "No Description Available.", _type = 0 };
                AttributeArray[125] = new AttributeInfo() { _name = "0x1F4", _description = "No Description Available.", _type = 0 };
                AttributeArray[126] = new AttributeInfo() { _name = "0x1F8", _description = "No Description Available.", _type = 0 };
                AttributeArray[127] = new AttributeInfo() { _name = "0x1FC Camera Size Front", _description = "The camera pushing effect's distance in front of the character.", _type = 0 };
                AttributeArray[128] = new AttributeInfo() { _name = "0x200 Camera Size Back", _description = "The camera pushing effect's distance behind the character.", _type = 0 };
                AttributeArray[129] = new AttributeInfo() { _name = "0x204 Camera Size Top", _description = "The camera pushing effect's distance above the character.", _type = 0 };
                AttributeArray[130] = new AttributeInfo() { _name = "0x208 Camera Size Bottom", _description = "The camera pushing effect's distance below the character.", _type = 0 };
                AttributeArray[131] = new AttributeInfo() { _name = "0x20C Opposite of previous?", _description = "?", _type = 0 };
                AttributeArray[132] = new AttributeInfo() { _name = "0x210 Zoom Camera Size Front", _description = "The zoom boundaries on the character in the front.", _type = 0 };
                AttributeArray[133] = new AttributeInfo() { _name = "0x214 Zoom Camera Size Back", _description = "The zoom boundaries on the character in the back.", _type = 0 };
                AttributeArray[134] = new AttributeInfo() { _name = "0x218 Zoom Camera Size Top", _description = "The zoom boundaries on the character on the top.", _type = 0 };
                AttributeArray[135] = new AttributeInfo() { _name = "0x21C Zoom Camera Size Bottom", _description = "The zoom boundaries on the character on the bottom.", _type = 0 };
                AttributeArray[136] = new AttributeInfo() { _name = "*0x220", _description = "No Description Available.", _type = 1 };
                AttributeArray[137] = new AttributeInfo() { _name = "0x224", _description = "No Description Available.", _type = 0 };
                AttributeArray[138] = new AttributeInfo() { _name = "0x228", _description = "No Description Available.", _type = 0 };
                AttributeArray[139] = new AttributeInfo() { _name = "0x22C", _description = "No Description Available.", _type = 0 };
                AttributeArray[140] = new AttributeInfo() { _name = "0x230", _description = "No Description Available.", _type = 0 };
                AttributeArray[141] = new AttributeInfo() { _name = "*0x234", _description = "No Description Available.", _type = 1 };
                AttributeArray[142] = new AttributeInfo() { _name = "0x238", _description = "No Description Available.", _type = 0 };
                AttributeArray[143] = new AttributeInfo() { _name = "0x23C", _description = "No Description Available.", _type = 0 };
                AttributeArray[144] = new AttributeInfo() { _name = "0x240", _description = "No Description Available.", _type = 0 };
                AttributeArray[145] = new AttributeInfo() { _name = "0x244 Magnifying Glass Shrink Ratio", _description = "The size of the character in the magifying glass.", _type = 0 };
                AttributeArray[146] = new AttributeInfo() { _name = "0x248", _description = "No Description Available.", _type = 0 };
                AttributeArray[147] = new AttributeInfo() { _name = "0x24C", _description = "No Description Available.", _type = 0 };
                AttributeArray[148] = new AttributeInfo() { _name = "0x250", _description = "No Description Available.", _type = 0 };
                AttributeArray[149] = new AttributeInfo() { _name = "0x254", _description = "No Description Available.", _type = 0 };
                AttributeArray[150] = new AttributeInfo() { _name = "0x258", _description = "No Description Available.", _type = 0 };
                AttributeArray[151] = new AttributeInfo() { _name = "0x25C", _description = "No Description Available.", _type = 0 };
                AttributeArray[152] = new AttributeInfo() { _name = "0x260", _description = "No Description Available.", _type = 0 };
                AttributeArray[153] = new AttributeInfo() { _name = "0x264", _description = "No Description Available.", _type = 0 };
                AttributeArray[154] = new AttributeInfo() { _name = "0x268", _description = "No Description Available.", _type = 0 };
                AttributeArray[155] = new AttributeInfo() { _name = "0x26C", _description = "No Description Available.", _type = 0 };
                AttributeArray[156] = new AttributeInfo() { _name = "0x270", _description = "No Description Available.", _type = 0 };
                AttributeArray[157] = new AttributeInfo() { _name = "0x274", _description = "No Description Available.", _type = 0 };
                AttributeArray[158] = new AttributeInfo() { _name = "0x278", _description = "No Description Available.", _type = 0 };
                AttributeArray[159] = new AttributeInfo() { _name = "0x27C", _description = "No Description Available.", _type = 0 };
                AttributeArray[160] = new AttributeInfo() { _name = "0x280", _description = "No Description Available.", _type = 0 };
                AttributeArray[161] = new AttributeInfo() { _name = "0x284", _description = "No Description Available.", _type = 0 };
                AttributeArray[162] = new AttributeInfo() { _name = "0x288", _description = "No Description Available.", _type = 0 };
                AttributeArray[163] = new AttributeInfo() { _name = "*0x28C", _description = "No Description Available.", _type = 1 };
                AttributeArray[164] = new AttributeInfo() { _name = "*0x290", _description = "No Description Available.", _type = 1 };
                AttributeArray[165] = new AttributeInfo() { _name = "*0x294", _description = "No Description Available.", _type = 1 };
                AttributeArray[166] = new AttributeInfo() { _name = "0x298", _description = "No Description Available.", _type = 0 };
                AttributeArray[167] = new AttributeInfo() { _name = "*0x29C", _description = "No Description Available.", _type = 1 };
                AttributeArray[168] = new AttributeInfo() { _name = "0x2A0", _description = "No Description Available.", _type = 0 };
                AttributeArray[169] = new AttributeInfo() { _name = "0x2A4", _description = "No Description Available.", _type = 0 };
                AttributeArray[170] = new AttributeInfo() { _name = "0x2A8", _description = "No Description Available.", _type = 0 };
                AttributeArray[171] = new AttributeInfo() { _name = "0x2AC", _description = "No Description Available.", _type = 0 };
                AttributeArray[172] = new AttributeInfo() { _name = "0x2B0", _description = "No Description Available.", _type = 0 };
                AttributeArray[173] = new AttributeInfo() { _name = "0x2B4", _description = "No Description Available.", _type = 0 };
                AttributeArray[174] = new AttributeInfo() { _name = "0x2B8", _description = "No Description Available.", _type = 0 };
                AttributeArray[175] = new AttributeInfo() { _name = "0x2BC", _description = "No Description Available.", _type = 0 };
                AttributeArray[176] = new AttributeInfo() { _name = "*0x2C0", _description = "No Description Available.", _type = 1 };
                AttributeArray[177] = new AttributeInfo() { _name = "*0x2C4", _description = "No Description Available.", _type = 1 };
                AttributeArray[178] = new AttributeInfo() { _name = "*0x2C8", _description = "No Description Available.", _type = 1 };
                AttributeArray[179] = new AttributeInfo() { _name = "*0x2CC", _description = "No Description Available.", _type = 1 };
                AttributeArray[180] = new AttributeInfo() { _name = "*0x2D0", _description = "No Description Available.", _type = 1 };
                AttributeArray[181] = new AttributeInfo() { _name = "*0x2D4", _description = "No Description Available.", _type = 1 };
                AttributeArray[182] = new AttributeInfo() { _name = "*0x2D8", _description = "No Description Available.", _type = 1 };
                AttributeArray[183] = new AttributeInfo() { _name = "*0x2DC", _description = "No Description Available.", _type = 1 };
                AttributeArray[184] = new AttributeInfo() { _name = "*0x2E0", _description = "No Description Available.", _type = 1 };
            }

            //string s = "AttributeArray = new AttributeInfo[185];";
            //string e = "";
            //int x = 0;
            //foreach (AttributeInfo v in AttributeArray)
            //    e += "\nAttributeArray[" + x++ + "] = new AttributeInfo() { _name = \"" + v._name + "\", _description = \"" + (v._description == "" ? "No Description Available." : v._description) + "\", _type = " + v._type.ToString().ToLower() + " };";
            //Console.WriteLine(s + e);
        }
        #endregion

        #endregion

        #region Data Saving
        public static void SaveAllTextData()
        {
            _written = new List<string>();
            SaveEvents();
            SaveParameters();
            SaveEventSyntax();
            SaveAttributes();
            SaveRequirements();
            SaveEnums();
            if (_written.Count > 0)
            {
                string s = "The following files have been modified:\n";
                foreach (string r in _written)
                    s += r.Substring(Application.StartupPath.Length) + Environment.NewLine;
                MessageBox.Show(s);
                _written.Clear();
            }
        }
        static List<string> _written;
        const string overSyntax = "Do you want to overwrite {0} in the MovesetData folder?";
        const string title = "Overwrite Permission";
        public static void SaveEvents()
        {
            if (!Directory.Exists(Application.StartupPath + "/MovesetData"))
                Directory.CreateDirectory(Application.StartupPath + "/MovesetData");

            string events = Application.StartupPath + "/MovesetData/Events.txt";
            if (!File.Exists(events) || MessageBox.Show(String.Format(overSyntax, "Events.txt"), title, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (StreamWriter file = new StreamWriter(events))
                    foreach (EventInformation info in Events.Values)
                    {
                        file.WriteLine(Util.Hex8(info._id));
                        file.WriteLine(info._name);
                        file.WriteLine(info._description);
                        string s = "";
                        foreach (int i in info._defaultParams)
                            s += i;
                        file.WriteLine(s);
                        file.WriteLine();
                    }
                _written.Add(events);
            }
        }

        public static void SaveParameters()
        {
            if (!Directory.Exists(Application.StartupPath + "/MovesetData"))
                Directory.CreateDirectory(Application.StartupPath + "/MovesetData");

            string parameters = Application.StartupPath + "/MovesetData/Parameters.txt";
            if (!File.Exists(parameters) || MessageBox.Show(String.Format(overSyntax, "Parameters.txt"), title, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (StreamWriter file = new StreamWriter(parameters))
                    foreach (EventInformation info in Events.Values)
                        if (info._paramNames != null && info._paramNames.Length > 0)
                        {
                            file.WriteLine(Util.Hex8(info._id));
                            for (int i = 0; i < info._paramNames.Length; i++)
                            {
                                file.WriteLine(info._paramNames[i]);
                                if (info._paramDescs.Length > i)
                                    file.WriteLine(info._paramDescs[i]);
                                else
                                    file.WriteLine();
                            }
                            file.WriteLine();
                        }
                _written.Add(parameters);
            }
        }

        public static void SaveEventSyntax()
        {
            if (!Directory.Exists(Application.StartupPath + "/MovesetData"))
                Directory.CreateDirectory(Application.StartupPath + "/MovesetData");

            string syntax = Application.StartupPath + "/MovesetData/EventSyntax.txt";
            if (!File.Exists(syntax) || MessageBox.Show(String.Format(overSyntax, "EventSyntax.txt"), title, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (StreamWriter file = new StreamWriter(syntax))
                    foreach (EventInformation info in Events.Values)
                    {
                        if (String.IsNullOrEmpty(info._syntax))
                            continue;

                        file.WriteLine(Util.Hex8(info._id));
                        file.WriteLine(info._syntax);
                        file.WriteLine();
                    }
                _written.Add(syntax);
            }
        }

        public static void SaveAttributes()
        {
            if (!Directory.Exists(Application.StartupPath + "/MovesetData"))
                Directory.CreateDirectory(Application.StartupPath + "/MovesetData");

            string attributes = Application.StartupPath + "/MovesetData/Attributes.txt";
            if (!File.Exists(attributes) || MessageBox.Show(String.Format(overSyntax, "Attributes.txt"), title, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (StreamWriter file = new StreamWriter(attributes))
                    foreach (AttributeInfo i in AttributeArray)
                    {
                        file.WriteLine(i._name);
                        file.WriteLine(i._description);
                        file.WriteLine(i._type);
                        file.WriteLine();
                    }
                _written.Add(attributes);
            }
        }

        public static void SaveRequirements()
        {
            if (!Directory.Exists(Application.StartupPath + "/MovesetData"))
                Directory.CreateDirectory(Application.StartupPath + "/MovesetData");

            string requirements = Application.StartupPath + "/MovesetData/Requirements.txt";
            if (!File.Exists(requirements) || MessageBox.Show(String.Format(overSyntax, "Requirements.txt"), title, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (StreamWriter file = new StreamWriter(requirements))
                    foreach (string i in iRequirements)
                        file.WriteLine(i);
                _written.Add(requirements);
            }
        }

        public static void SaveEnums()
        {
            if (!Directory.Exists(Application.StartupPath + "/MovesetData"))
                Directory.CreateDirectory(Application.StartupPath + "/MovesetData");

            string airground = Application.StartupPath + "/MovesetData/AirGroundStats.txt";
            string collision = Application.StartupPath + "/MovesetData/CollisionStats.txt";
            string gfx = Application.StartupPath + "/MovesetData/GFXFiles.txt";
            string enums = Application.StartupPath + "/MovesetData/Enums.txt";

            if (!File.Exists(airground) || MessageBox.Show(String.Format(overSyntax, "AirGroundStats.txt"), title, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (StreamWriter file = new StreamWriter(airground))
                    foreach (string i in iAirGroundStats)
                        file.WriteLine(i);
                _written.Add(airground);
            }
            if (!File.Exists(collision) || MessageBox.Show(String.Format(overSyntax, "CollisionStats.txt"), title, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (StreamWriter file = new StreamWriter(collision))
                    foreach (string i in iCollisionStats)
                        file.WriteLine(i);
                _written.Add(collision);
            }
            if (!File.Exists(gfx) || MessageBox.Show(String.Format(overSyntax, "GFXFiles.txt"), title, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (StreamWriter file = new StreamWriter(gfx))
                    foreach (string i in iGFXFiles)
                        file.WriteLine(i);
                _written.Add(gfx);
            }
            if (!File.Exists(enums) || MessageBox.Show(String.Format(overSyntax, "Enums.txt"), title, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                //list of enum dictionaries
                List<Dictionary<int, List<string>>> EnumList = new List<Dictionary<int, List<string>>>();
                //enum index, event id, param index list
                Dictionary<int, Dictionary<long, List<int>>> EnumIds = new Dictionary<int, Dictionary<long, List<int>>>();
                //remap events and enums
                foreach (EventInformation info in Events.Values)
                {
                    if (info._enums != null && info._enums.Count > 0)
                    {
                        List<int> p = new List<int>();

                        bool has = false; int index = -1;
                        foreach (Dictionary<int, List<string>> t in EnumList)
                        {
                            bool match = true;
                            index++;
                            foreach (int g in info._enums.Keys)
                            {
                                if (t.ContainsKey(g))
                                {
                                    int r = 0;
                                    foreach (string s in t[g])
                                    {
                                        if (s != info._enums[g][r++])
                                        {
                                            match = false;
                                            break;
                                        }
                                    }
                                }
                                else match = false;
                            }
                            if (match)
                            {
                                has = true;
                                break;
                            }
                            else
                                continue;
                        }

                        if (!has)
                        {
                            EnumList.Add(info._enums);
                            index = EnumList.Count - 1;
                        }

                        if (!EnumIds.ContainsKey(index))
                            EnumIds.Add(index, new Dictionary<long, List<int>>());

                        foreach (int i in info._enums.Keys) p.Add(i);
                        EnumIds[index].Add(info._id, p);
                    }
                }

                using (StreamWriter file = new StreamWriter(enums))
                {
                    int i = 0;
                    foreach (Dictionary<int, List<string>> d in EnumList)
                    {
                        Dictionary<long, List<int>> events = EnumIds[i];
                        foreach (var ev in events)
                        {
                            file.WriteLine(Util.Hex8(ev.Key));
                            foreach (int v in ev.Value)
                                file.WriteLine(v);
                            file.WriteLine();
                        }
                        file.WriteLine();
                        foreach (int x in d.Keys)
                            if (d[x] != null && d[x].Count > 0)
                                foreach (string value in d[x])
                                    file.WriteLine(value);
                        file.WriteLine();
                        i++;
                    }
                }
                _written.Add(enums);
            }
        }

        #endregion
    }
}
